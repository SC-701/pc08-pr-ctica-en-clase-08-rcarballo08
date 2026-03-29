using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Modelos
{
    namespace Abstracciones.Modelos
    {
        public class ProductoBase
        {
            [Required(ErrorMessage = "La propiedad nombre es requerida")]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "La propiedad descripción es requerida")]
            [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres")]
            public string Descripcion { get; set; }

            [Required(ErrorMessage = "La propiedad precio es requerida")]
            [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal Precio { get; set; }

            [Required(ErrorMessage = "La propiedad stock es requerida")]
            [Range(0, 100000, ErrorMessage = "El stock no puede ser negativo")]
            public int Stock { get; set; }

            [Required(ErrorMessage = "La propiedad código de barras es requerida")]
            [RegularExpression(@"^\d{13}$", ErrorMessage = "El código de barras debe tener exactamente 13 dígitos numéricos")]
            public string CodigoBarras { get; set; }
        }

        public class ProductoRequest : ProductoBase
        {
            public Guid IdSubCategoria { get; set; }
        }

        public class ProductoResponse : ProductoBase
        {
            public Guid Id { get; set; }
            public string SubCategoria { get; set; }
            public string Categoria { get; set; }
            public decimal PrecioUSD { get; set; } 
        }
    }
}
