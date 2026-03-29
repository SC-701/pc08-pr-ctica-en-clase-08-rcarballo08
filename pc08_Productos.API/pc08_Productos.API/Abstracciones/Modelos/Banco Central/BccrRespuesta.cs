using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos.Banco_Central
{
    public class BccrRespuesta
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public List<BccrDato> Datos { get; set; } = new();
    }

    public class BccrDato
    {
        public string Titulo { get; set; } = string.Empty;
        public string Periodicidad { get; set; } = string.Empty;
        public List<BccrIndicador> Indicadores { get; set; } = new();
    }

    public class BccrIndicador
    {
        public string CodigoIndicador { get; set; } = string.Empty;
        public string NombreIndicador { get; set; } = string.Empty;
        public List<BccrSerie> Series { get; set; } = new();
    }

    public class BccrSerie
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal ValorDatoPorPeriodo { get; set; }
    }
}