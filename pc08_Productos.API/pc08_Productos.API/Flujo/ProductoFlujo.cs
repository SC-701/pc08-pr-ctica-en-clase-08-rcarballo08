using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Servicios;
using Abstracciones.Modelos;
using Abstracciones.Modelos.Abstracciones.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo
{
    public class ProductoFlujo : IProductoFlujo
    {
        private IProductoDA _productoDA;
        private ITipoCambio _tipoCambioServicio;

        public ProductoFlujo(IProductoDA productoDA, ITipoCambio tipoCambioServicio)
        {
            _productoDA = productoDA;
            _tipoCambioServicio = tipoCambioServicio;
        }

        public Task<Guid> Agregar(ProductoRequest producto)
        {
            return _productoDA.Agregar(producto);
        }

        public Task<Guid> Editar(Guid Id, ProductoRequest producto)
        {
            return _productoDA.Editar(Id, producto);
        }

        public Task<Guid> Eliminar(Guid Id)
        {
            return _productoDA.Eliminar(Id);
        }

        public async Task<IEnumerable<ProductoResponse>> Obtener()
        {
            var productos = await _productoDA.Obtener();

            if (productos != null && productos.Any())
            {
                var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioVentaAsync();
                foreach (var producto in productos)
                {
                    producto.PrecioUSD = Math.Round(producto.Precio / tipoCambio, 2);
                }
            }

            return productos;
        }

        public async Task<ProductoResponse> Obtener(Guid Id)
        {
            var producto = await _productoDA.Obtener(Id);

            if (producto != null)
            {
                var tipoCambio = await _tipoCambioServicio.ObtenerTipoCambioVentaAsync();
                producto.PrecioUSD = Math.Round(producto.Precio / tipoCambio, 2);
            }

            return producto;
        }
    }
}
