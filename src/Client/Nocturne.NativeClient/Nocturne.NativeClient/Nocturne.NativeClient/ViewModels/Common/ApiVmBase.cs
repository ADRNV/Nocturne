using Nocturne.NativeClient.ViewModels.Common;
using Nocturne.Api.Client.Common;

namespace Nocturne.NativeClient.ViewModels.Auth
{
    public class ApiVmBase<T> : VmBase<T>
    {
        protected INocturneApiClientProxy _apiClient;

        public ApiVmBase(INocturneApiClientProxy apiClient)
        {
            _apiClient = apiClient;
        }

        public override void Dispose()
        {
            _apiClient = null;
        }
    }
}
