using Letters.Data;

namespace Letters.Handler.Interfaces
{
    public interface ILetterSenderService
    {
       Task Send(ReceptionContext receptionContext);
    }
}
