using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiendasServicios.Api.Libro.Aplicacion;
using TiendasServicios.Api.Libro.Modelo;
using TiendasServicios.Api.Libro.Persistencia;
using Xunit;
namespace TiendaServicios.Api.Libro.Tests
{
   public class LibrosServiceTest
    {

        private Mock<ContextoLibreria> CreacionContexto() {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();


            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            dbSet.As<AsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
            .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));


            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x => x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
            //     dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());
        }


        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba() {

            A.Configure<LibreriaMaterial>()
                   .Fill(x => x.Titulo).AsArticleTitle()
                   .Fill(x => x.LibreriaMaterialId, () => { return new Guid(); });


            var lista = A.ListOf<LibreriaMaterial>(30);
            lista[0].LibreriaMaterialId = Guid.Empty;


            return lista;
        
        }


        [Fact]
        public async void GetLibroPorId() {
            var mockContexto = CreacionContexto();

            var mapconfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapconfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();
            request.LibroId = Guid.Empty;


            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);
           var libro=await manejador.Handle(request, new System.Threading.CancellationToken());
            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);

        }


        [Fact]
        public async void GetLibros() {

          //  System.Diagnostics.Debugger.Launch();
            /*1 . Emular instancia de entity*/

            //var mockContexto = new Mock<ContextoLibreria>();
            var mockContexto = CreacionContexto();

            /*2 Emular al mapping IMapper*/

            var mapConfig = new MapperConfiguration( cfg=> {
                cfg.AddProfile(new MappingTest());
            });
            var mapper = mapConfig.CreateMapper();

            //3. Instanciar  a las clase manejador con los parametros del mocks que hemos creado

            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.Ejecuta request = new Consulta.Ejecuta();
           var lista= await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());


        }



        [Fact]
        public async void GuardarLibro() {

            System.Diagnostics.Debugger.Launch();

            var options = new DbContextOptionsBuilder<ContextoLibreria>()
             .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
             .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro Microservice";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

           var libro= await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }

    }
}
