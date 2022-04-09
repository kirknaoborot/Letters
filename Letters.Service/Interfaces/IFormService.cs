using Letters.Core.Models;

namespace Letters.Service.Interfaces
{
    public interface IFormService
    {
        Task<FormModel> GetForm(Guid receptionId);

        Task AddLetters(string text, string email, string address, string recipient, string phone,
                                     string socialStatus, string firstName, string lastName, string middleName, byte[] file = null);
    }
}
