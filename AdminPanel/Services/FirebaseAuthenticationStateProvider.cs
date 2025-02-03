using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminPanel.Services
{
    public class FirebaseAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationService _authenticationService;

        public FirebaseAuthenticationStateProvider(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isAuthenticated = await _authenticationService.IsUserAuthenticatedAsync();
            
            if (!isAuthenticated)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var token = await _authenticationService.GetUserTokenAsync();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, token),
            }, "Firebase");

            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
} 