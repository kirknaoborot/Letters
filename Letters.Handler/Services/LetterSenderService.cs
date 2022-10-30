using DigitalDesignService;
using Letters.Data;
using Letters.Data.Reception;
using Letters.Handler.Interfaces;
using Letters.Handler.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ServiceModel;
using System.Text;
using static DigitalDesignService.IntegrationSystem_IntegrationServiceClient;

namespace Letters.Handler.Services
{
    internal class LetterSenderService : ILetterSenderService 
    {
        private readonly IntegrationSystem_IntegrationServiceClient _client;
        private readonly IOptions<ServiceSettings> _options;

        public LetterSenderService(IOptions<ServiceSettings> options)
        {
            _client = new IntegrationSystem_IntegrationServiceClient(EndpointConfiguration.BasicHttpBinding_IntegrationSystem_IntegrationService1);
            _options = options;
        }

        public async System.Threading.Tasks.Task Send(ReceptionContext receptionContext)
        {
            try
            {
                await _client.OpenAsync();

                var letters = await receptionContext.Letters
                    .Where(_ => !_.IsProcessed)
                    .Include(_ => _.Attaches)
                    .ToListAsync();

                if (!letters.Any())
                    return;

                foreach (var letter in letters)
                {
                    try
                    {
                        var searchParameter = new SearchParameter
                        {
                            Alias = "DocumentId",
                            Value = letter.Id.ToString()
                        };

                        var parameterList = new ParamsList
                        {
                            SearchParameters = new[] { searchParameter }       
                        };

                        var searchQuery = new SearchQuery
                        {
                            QueryParams = parameterList,
                            SearchPatternId = _options.Value.DocumentSeatchParameterNumber
                        };

                        var cardIds = await _client.GetSearchResultAsync(_options.Value.ProfileId, searchQuery);

                        if (cardIds.Any())
                        {
                            await WriteLog(letter.Id, $"Идентификатор карточки :{string.Join("; ", cardIds.ToList())} уже существует", receptionContext);

                            continue;
                        } 

                        var document = BuildDocument(letter.Id.ToString());

                        var documentId = await CreateDocument(document);

                        await AttachLetter(letter, documentId);

                        await SendNotification(documentId);

                        await WriteLog(letter.Id, string.Empty, receptionContext);

                        await RemoveLog(letter.Id, receptionContext);
                    }
                    catch (Exception ex)
                    {
                        await WriteLog(letter.Id, ex.Message, receptionContext);
                    }
                }

                await _client.CloseAsync();
            }
            catch (Exception ex)
            {
                await WriteLog(Guid.Empty, ex.Message, receptionContext);
            }
        }

        private async System.Threading.Tasks.Task SendNotification(Guid documentId)
        {
            var notification = new NotificationItem()
            {
                ProfileId = Guid.NewGuid()
            };

            var notificationRow = new NotificationRow
            {
                Date = DateTime.Now,
                DocumentId = documentId.ToString(),
                Status = NotificationStatus.Prepared,
                TypeId = "16cef537-eda7-0eb2-4fed-c9ff625b56c8",
                NotificationId = Guid.NewGuid().ToString()
            };

            notification.Notification = notificationRow;

            await _client.AddNotificationRecordAsync(notification);
        }

        private async System.Threading.Tasks.Task AttachLetter (Letter letter, Guid documentId)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"Дата поступления обращения: {letter.CreateDate}");
            builder.AppendLine($"Электронный адрес заявителя: {letter.Email}");
            builder.AppendLine($"Телефон заявителя: {letter.Phone}");
            builder.AppendLine($"ФИО заявителя: {letter.Fio}");
            builder.AppendLine($"Социальное положение: {letter.SocialStatus}");
            builder.AppendLine($"Адрес обращения: {letter.Address}");
            builder.AppendLine($"Кому адресовано: {letter.Recipient}");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine($"Текст обращения: {letter.Text}");

            var fileLetter = Encoding.UTF8.GetBytes(builder.ToString());

            var file = Encoding.UTF8.GetPreamble().Concat(fileLetter).ToArray();

            var fileItem = new FileItem
            {
                Content = file,
                File = new associatedFile { localName = "Обращение.txt" }
            };

            await _client.AddFileToDocumentAsync(documentId, _options.Value.ProfileId, fileItem);

            if (letter.Attaches is not null)
            {
                var fileItems = new List<FileItem>();

                foreach (var attach in letter.Attaches)
                {
                    fileItem = new FileItem
                    {
                        Content = attach.Content,
                        File = new associatedFile { localName = attach.FileName }
                    };

                    fileItems.Add(fileItem);
                }

                await _client.AddFilesToDocumentAsync(documentId, _options.Value.ProfileId, new FilesItem { Files = fileItems.ToArray()}); // протестить метод передачи массива
            }
        }

        private async Task<Guid> CreateDocument(document doc)
        {
            var communicationPartner = new communicationPartner { organization = _options.Value.OrganizationName };

            var documentItem = new DocumentItem
            {
                ProfileId = _options.Value.ProfileId,
                Document = new communication
                {
                    Items = new object[] { doc },
                    header = new communicationHeader { source = communicationPartner }
                }
            };

            var documentId = await _client.CreateDocumentAsync(documentItem);

            return documentId;
        }

        private  document BuildDocument(string letterId)
        {
            var document = new document
            {
                kind = new qualifiedValue
                {
                    Properties = new Property[] { new Property { Value =  _options.Value.DocumentKind} }
                },

                num = new documentNumber
                {
                    RegistratorDepartment = new qualifiedValue { id = _options.Value.RegistratorDepartmentId.ToString() }
                },

                SubjectDocument = new CitizenRequestDocument
                {
                    CitizenRequestType = CitizenRequestType.Electronic,
                    Applicant = new Applicant1 { Id = _options.Value.ApplicantDefaultId },
                    ReceiptDate = DateTime.Now
                },

                correspondents = Enumerable.Empty<correspondent>().ToArray(),

                RealAdresses = new[]
                {
                    new addressee()
                    {
                        organization = new qualifiedValue
                        {
                            Properties = new Property[] { new Property { Value = _options.Value.OrganizationName.ToString() } }
                        }
                    }
                },

                annotation = string.Empty,

                DocumentType = DocumentType1.Incoming,

                uid = letterId
            };

            return document;
        }

        private  async System.Threading.Tasks.Task WriteLog(Guid letterId, string messageLog, ReceptionContext receptionContext)
        {
            var log = new LetterLog
            {
                ModiFyDate = DateTime.Now,
                LetterId = letterId == Guid.Empty ? Guid.Empty : letterId,
                IsComplete = string.IsNullOrEmpty(messageLog),
                ErrorMessage = messageLog
            };

            var logDb = await receptionContext.Logs.LastOrDefaultAsync(_ => _.LetterId == letterId);

            if ((DateTime.Now - logDb.ModiFyDate).TotalHours < _options.Value.IntervalException)
                return;

            await receptionContext.AddAsync(log);

            await receptionContext.SaveChangesAsync();
        }

        private static async System.Threading.Tasks.Task RemoveLog(Guid letterId, ReceptionContext receptionContext)
        {
            var logs = await receptionContext.Logs.Where(_ => _.LetterId == letterId).ToListAsync();

            if (logs.Any())
            {
                logs.ForEach(log =>
                {
                    receptionContext.Remove(log);
                });

                await receptionContext.SaveChangesAsync();
            }
        }
    }
}
