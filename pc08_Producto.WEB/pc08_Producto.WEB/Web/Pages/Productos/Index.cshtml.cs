using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Productos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<ProductoResponse> productos { get; set; } = default!;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");
            Console.WriteLine($"URL completa: {endpoint}");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.SendAsync(new HttpRequestMessage(HttpMethod.Get, endpoint));
            respuesta.EnsureSuccessStatusCode();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productos = JsonSerializer.Deserialize<List<ProductoResponse>>(
                await respuesta.Content.ReadAsStringAsync(), opciones);
        }

        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Token");
            var cliente = new HttpClient();
            if (tokenClaim != null)
                cliente.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenClaim.Value);
            return cliente;
        }
    }
}
