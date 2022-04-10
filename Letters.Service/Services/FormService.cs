using Letters.Core.Models;
using Letters.Data;
using Letters.Data.Reception;
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

        public async Task Validate ()
        {

        }

        public async Task AddLetters(string text, string email, string address, string recipient, string phone, 
                                     string socialStatus, string firstName, string lastName, string middleName, byte[] file = null)
        {
            var fio = string.IsNullOrEmpty(lastName) ? string.Empty : lastName;
            fio += string.IsNullOrEmpty(firstName) ? string.Empty : firstName;
            fio += string.IsNullOrEmpty(middleName) ? string.Empty : middleName;

            var letter = new Letter
            {
                IsProcessed = false,
                Text = text,
                Email = email,
                Address = string.IsNullOrEmpty(address) ? string.Empty : address,
                Recipient = string.IsNullOrEmpty(recipient) ? string.Empty : recipient,
                Phone = string.IsNullOrEmpty(phone) ? string.Empty : phone,
                SocialStatus = string.IsNullOrEmpty(socialStatus) ? string.Empty : socialStatus,
                Fio = fio,
                Attach = file != null && file.Length > 0 ? file : null            
            };

            await _receptionContext.AddAsync(letter);

            await _receptionContext.SaveChangesAsync();
        }
    }
}
