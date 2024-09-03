using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendasServicios.Api.Libro.Modelo;

namespace TiendasServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria() {  }
        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options)
        {

        }
        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }/*POermite sobreescribir*/
    }

    }
