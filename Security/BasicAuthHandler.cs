using FBapiService.DataDB;
using FBapiService.Models.GeneraQR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace FBapiService.Security
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        //private IUserService _userService;
        private UserDataCrud _userService;

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserDataCrud userService) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() 
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("no esta el header");

                bool result = false;

                try
                {
                    var autHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                    var credentialBytes = Convert.FromBase64String(autHeader.Parameter);
                    var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                    var username = credentials[0];
                    var password = credentials[1];
                    result = _userService.GetUserBasic(username, password);
                }
                catch
                {
                    return AuthenticateResult.Fail("algo paso");
                }

                if (!result)
                    return AuthenticateResult.Fail("usuario o contraseña incorrectos");

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "id"),
                    new Claim(ClaimTypes.Name, "user"),

                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
                   
        }
    }
}
