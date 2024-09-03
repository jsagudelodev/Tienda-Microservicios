using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendasServicios.Api.CarritoCompra.Aplicacion;

namespace TiendasServicios.Api.CarritoCompra.Controllers
{
    [Route("api/Controller")]
    [ApiController]
    public class CarritoComprasController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CarritoComprasController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
           return await _mediator.Send(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarritoDto>> GetCarrito(int  id) {

            return await _mediator.Send(new Consulta.Ejecuta{ CarritoSesionId = id });
        }

    }
}
