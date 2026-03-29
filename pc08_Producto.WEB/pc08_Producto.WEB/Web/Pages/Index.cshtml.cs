using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        public List<ProductoResponse> productos { get; set; }

        public async Task OnGet()
        {
            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync("https://localhost:7130/api/producto/ObtenerTodos");

            if (respuesta.IsSuccessStatusCode)
            {
                var contenido = await respuesta.Content.ReadAsStringAsync();

                productos = JsonSerializer.Deserialize<List<ProductoResponse>>(contenido, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                productos = new List<ProductoResponse>();
            }
        }
    }
}

