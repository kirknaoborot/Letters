using Letters.Service.Model;

namespace Letters.Service.Interfaces
{
    public interface ICaptchaService
    {
        public CapthcaModel GetCapcha();

        Core.Models.CaptchaModel UpdateCaptcha(Guid key);

        public byte[] Test(string key = "БВГД");
    }
}
