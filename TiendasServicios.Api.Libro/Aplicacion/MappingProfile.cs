using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendasServicios.Api.Libro.Modelo;

namespace TiendasServicios.Api.Libro.Aplicacion
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<LibreriaMaterial, LibroMaterialDto>();
        }
    }
}
