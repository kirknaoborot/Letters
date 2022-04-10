using Letters.Service.Model;

namespace Letters.Service.Interfaces
{
    public interface ICaptchaService
    {
        CapthcaModel GetCapcha();

        Core.Models.CaptchaModel Update(Guid key);

        byte[] Test(string key = "БВГД");

        bool Validate(Guid id, string value);
    }
}
