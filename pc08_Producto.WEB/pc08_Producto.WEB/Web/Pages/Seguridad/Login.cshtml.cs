using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Reglas;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Web.Pages.Seguridad
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginRequest loginInfo { get; set; } = default!;
        [BindProperty]
        public Token token { get; set; } = default!;
        private IConfiguracion _configuracion;

        public LoginModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                // 1. Hashear la contraseña
                var hash = Autenticacion.GenerarHash(loginInfo.Password);
                loginInfo.PasswordHash = Autenticacion.ObtenerHash(hash);

                // 2. NombreUsuario derivado del correo
                loginInfo.NombreUsuario = loginInfo.CorreoElectronico.Split("@")[0];

                // 3. Llamar al API de Seguridad
                string endpoint = _configuracion.ObtenerMetodo("ApiEndPointsSeguridad", "Login");
                var client = new HttpClient();
                var respuesta = await client.PostAsJsonAsync<LoginBase>(endpoint,
                    new LoginBase
                    {
                        NombreUsuario = loginInfo.NombreUsuario,
                        CorreoElectronico = loginInfo.CorreoElectronico,
                        PasswordHash = loginInfo.PasswordHash
                    });
                respuesta.EnsureSuccessStatusCode();

                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                token = JsonSerializer.Deserialize<Token>(
                    await respuesta.Content.ReadAsStringAsync(), opciones);

                if (token.ValidacionExitosa)
                {
                    // 4. Leer JWT y generar claims para la cookie
                    JwtSecurityToken? jwtToken = Autenticacion.leerToken(token.AccessToken);
                    var claims = Autenticacion.GenerarClaims(jwtToken, token.AccessToken);
                    await EstablecerAutenticacion(claims);

                    // 5. Redirigir a ReturnUrl o al índice de productos
                    var urlRedirigir = $"{HttpContext.Request.Query["ReturnUrl"]}";
                    if (string.IsNullOrEmpty(urlRedirigir))
                        return RedirectToPage("/Productos/Index");
                    return Redirect(urlRedirigir);
                }
            }
            return Page();
        }

        private async Task EstablecerAutenticacion(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}
