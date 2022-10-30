using Letters.Core.Models;
using Letters.Service.InputModels;

namespace Letters.Service.Interfaces
{
    public interface IFormService
    {
        Task<FormModel> GetForm(Guid receptionId);

        Task AddLetters(ILetterInputModel model);
    }
}
