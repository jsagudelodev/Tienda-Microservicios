using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Autor.Modelo;
using TiendaServicios.Api.Autor.Persistencia;

using FluentValidation;

namespace TiendaServicios.Api.Autor.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest { 
        
           public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }

        }


        public class EjecutaValidacion : AbstractValidator<Ejecuta> {

            public EjecutaValidacion()
            {
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
            }
        }



        //Clase q maneje la logica


        public class Manejador : IRequestHandler<Ejecuta>
        {
            public readonly ContextoAutor _contextoAutor;


            public Manejador(ContextoAutor contextoAutor)
            {
                _contextoAutor = contextoAutor;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento

                };

                _contextoAutor.AutorLibro.Add(autorLibro);
               var valor=await  _contextoAutor.SaveChangesAsync();
                if (valor > 0)
                {
                    return Unit.Value;
                         
                }
                throw new Exception("No se pudo insertar el libro");
                     


            }
        }

    }
}
