﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TiendasServicios.Api.CarritoCompra.RemoteInterface;
using TiendasServicios.Api.CarritoCompra.RemoteModel;

namespace TiendasServicios.Api.CarritoCompra.RemoteService
{
    public class LibrosService : ILibroService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<ILibroService> _logger;
        public LibrosService(IHttpClientFactory httpClient, ILogger<ILibroService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

        }
        public async Task<(bool resultado, LibroRemote libro, string ErrorMessage)> GetLibro(Guid LibroId)
        {
            try
            {
                var cliente = _httpClient.CreateClient("Libros");
                var response=await cliente.GetAsync($"api/LibroMaterial/{LibroId}");
                if (response.IsSuccessStatusCode)
                {
                    var contenido =await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroRemote>(contenido,options);
                    return (true, resultado, null);
                }

                return (false, null, response.ReasonPhrase);

            }
            catch (Exception e)
            {

                _logger?.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
