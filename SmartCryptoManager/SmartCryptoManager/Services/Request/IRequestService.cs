using System.Threading.Tasks;

namespace SmartCryptoManager.Services
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "", bool IsMocked = false);

        Task<TResult> PostAsync<TResult>(string uri, TResult data, string token = "", bool IsMocked = false);

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data, string token = "", bool IsMocked = false);

        Task<TResult> PutAsync<TResult>(string uri, TResult data, string token = "", bool IsMocked = false);

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data, string token = "", bool IsMocked = false);
    }
}