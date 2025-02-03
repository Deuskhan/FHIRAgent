using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public class AuthenticationService
    {
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> SignInWithGoogleAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("signInWithGoogle");
        }

        public async Task SignOutAsync()
        {
            await _jsRuntime.InvokeVoidAsync("signOut");
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            return await _jsRuntime.InvokeAsync<bool>("isUserAuthenticated");
        }

        public async Task<string> GetUserTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("getUserToken");
        }
    }
} 