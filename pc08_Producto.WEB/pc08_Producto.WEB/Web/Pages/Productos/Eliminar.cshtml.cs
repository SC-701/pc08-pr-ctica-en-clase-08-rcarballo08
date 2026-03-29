using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Productos
{
    [Authorize]
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ProductoResponse producto { get; set; } = default!;

        public async Task<ActionResult> OnGet(Guid? id)
        {
            if (id == Guid.Empty) return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            Console.WriteLine($"URL completa: {string.Format(endpoint, id)}");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id)));
            respuesta.EnsureSuccessStatusCode();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            producto = JsonSerializer.Deserialize<ProductoResponse>(
                await respuesta.Content.ReadAsStringAsync(), opciones);
            return Page();
        }

        public async Task<ActionResult> OnPost(Guid? id)
        {
            if (id == Guid.Empty) return NotFound();
            if (!ModelState.IsValid) return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarProducto");
            Console.WriteLine($"URL completa: {string.Format(endpoint, id)}");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, string.Format(endpoint, id)));
            respuesta.EnsureSuccessStatusCode();
            return RedirectToPage("./Index");
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
