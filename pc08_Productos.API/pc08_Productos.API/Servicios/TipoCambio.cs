using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos.Banco_Central;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Servicios
{
    public class TipoCambio : ITipoCambio
    {
        private readonly HttpClient _httpClient;
        private readonly string _urlBase;
        private readonly string _bearerToken;

        public TipoCambio(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _urlBase = configuration["BancoCentralCR:UrlBase"]
                ?? throw new ArgumentNullException("BancoCentralCR:UrlBase no configurado");
            _bearerToken = configuration["BancoCentralCR:BearerToken"]
                ?? throw new ArgumentNullException("BancoCentralCR:BearerToken no configurado");
        }

        public async Task<decimal> ObtenerTipoCambioVentaAsync()
        {
            var fecha = DateTime.Now.ToString("yyyy/MM/dd");
            var url = $"{_urlBase}?fechaInicio={fecha}&fechaFin={fecha}&idioma=ES";

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _bearerToken);

            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error BCCR: {response.StatusCode} - {json}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultado = JsonSerializer.Deserialize<BccrRespuesta>(json, options);

            var tipoCambio = resultado?
                .Datos?.FirstOrDefault()?
                .Indicadores?.FirstOrDefault()?
                .Series?.FirstOrDefault()?
                .ValorDatoPorPeriodo ?? 0;

            if (tipoCambio == 0)
                throw new Exception("No se pudo obtener el tipo de cambio del BCCR.");

            return tipoCambio;
        }
    }
}