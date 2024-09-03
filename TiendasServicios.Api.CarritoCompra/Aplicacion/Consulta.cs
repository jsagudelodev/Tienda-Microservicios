using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendasServicios.Api.CarritoCompra.Persistencia;
using TiendasServicios.Api.CarritoCompra.RemoteService;

namespace TiendasServicios.Api.CarritoCompra.Aplicacion
{
    public class Consulta
    {
        public class Ejecuta : IRequest<CarritoDto> { 
         public int CarritoSesionId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, CarritoDto>
        {
            private readonly CarritoContexto _contexto;
            private readonly LibrosService _libroService;
            public Manejador(LibrosService librosService, CarritoContexto contexto)
            {
                _contexto = contexto;
                _libroService = librosService;
            }
            public async Task<CarritoDto> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion =await _contexto.CarritoSesion.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle=await _contexto.CarritoSesionDetalle.Where(x=>x.CarritoSesionId==request.CarritoSesionId).ToListAsync();
                var listaCarritoDto = new List<CarritoDetalleDto>();
                
                foreach (var libro in carritoSesionDetalle)
                {

                    var response = await _libroService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado) {
                        var objetoLibro = response.libro;
                        var caaritoDetalle = new CarritoDetalleDto {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion= objetoLibro.FechaPublicacion,
                            LibroId= objetoLibro.LibreriaMaterialId
                        };

                        listaCarritoDto.Add(caaritoDetalle);

                    }
                }

                var carritoSesionDto = new CarritoDto
                {

                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion=carritoSesion.FechaCreacion,
                    ListaProductos=listaCarritoDto
                };
                return carritoSesionDto;

            }
        }
    }
}
