using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendasServicios.Api.CarritoCompra.RemoteModel;

namespace TiendasServicios.Api.CarritoCompra.RemoteInterface
{
   public interface ILibroService
    {
        Task<(bool resultado,LibroRemote libro,string ErrorMessage)> GetLibro(Guid LibroId);
    }
}
