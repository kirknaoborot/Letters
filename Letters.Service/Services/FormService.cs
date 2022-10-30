using Letters.Core.Models;
using Letters.Data;
using Letters.Data.Reception;
using Letters.Service.Extensions;
using Letters.Service.InputModels;
using Letters.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Letters.Service.Services
{
    public class FormService : IFormService
    {
        private readonly ReceptionContext _receptionContext;
        private readonly ICaptchaService _captchaService;

        public FormService(ReceptionContext receptionContext, ICaptchaService captchaService)
        {
            _receptionContext = receptionContext;
            _captchaService = captchaService;
        }

        public async Task<FormModel> GetForm(Guid receptionId)
        {
            var recepients = await _receptionContext.Recipients
                .Where(_ => _.ReceptionId == receptionId)
                .Select(_ => _.Name).ToListAsync();

            var captcha = _captchaService.GetCapcha();

            var captchaModel = new CaptchaModel
            {
                Id = captcha.Id,
                Audio = captcha.Audio,
                Image = captcha.Image
            };

            return new FormModel
            {
                Recipients = recepients,
                Captcha = captchaModel
            };
        }

        public async Task AddLetters(ILetterInputModel model)
        {
            var fio = string.IsNullOrEmpty(model.LastName) ? string.Empty : model.LastName;
            fio += string.IsNullOrEmpty(model.FirstName) ? string.Empty : model.FirstName;
            fio += string.IsNullOrEmpty(model.MiddleName) ? string.Empty : model.MiddleName;

            var letter = new Letter
            {
                IsProcessed = false,
                Text = model.Text,
                Email = model.Email,
                Address = string.IsNullOrEmpty(model.Address) ? string.Empty : model.Address,
                Recipient = string.IsNullOrEmpty(model.Recipient) ? string.Empty : model.Recipient,
                Phone = string.IsNullOrEmpty(model.Phone) ? string.Empty : model.Phone,
                SocialStatus = string.IsNullOrEmpty(model.SocialStatus) ? string.Empty : model.SocialStatus,
                Fio = fio
            };

            await _receptionContext.AddAsync(letter);

            if (model.File is not null)
            {
                foreach (var file in model.File)
                {
                    var attach = new Attach
                    {
                        LetterId = letter.Id,
                        Content = await file.GetBytes(),
                        FileName = file.FileName
                    };

                    await _receptionContext.AddAsync(attach);
                }
            }

            await _receptionContext.SaveChangesAsync();
        }
    }
}
