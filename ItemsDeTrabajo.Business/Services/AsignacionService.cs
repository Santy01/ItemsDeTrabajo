using ItemsDeTrabajo.Models;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace ItemsDeTrabajo.Business.Services
{
    public class AsignacionService
    {
        private readonly HttpClient _httpClient;
        private readonly IItemstrabajoService _itemService;
        private readonly string _gestionUsuariosApiUrl; // (configurado en appsettings.json)

        public AsignacionService(HttpClient httpClient, IItemstrabajoService itemService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _itemService = itemService;
            // La URL base del API de GestiónDeUsuarios se obtiene de la configuración
            _gestionUsuariosApiUrl = configuration.GetValue<string>("GestionUsuariosApiUrl")
                                      ?? throw new InvalidOperationException("No se encontró la URL de GestiónDeUsuarios en la configuración.");
        }

        public async Task<Itemstrabajo> CreateAndAssignWorkItemAsync(Itemstrabajo item)
        {            
            // 1. Consultar la lista de usuarios disponibles
            var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>($"{_gestionUsuariosApiUrl}/api/usuarios/ListaUsuarios");
            if (usuarios == null || !usuarios.Any())
            {
                throw new Exception("No se encontraron usuarios en GestiónDeUsuarios.");
            }

            // 2. Excluir usuarios saturados (más de 2 ítems altamente relevantes; se asume saturación al llegar a 3)
            var candidatos = usuarios.Where(u => u.AltamenteRelevantes < 3).ToList();
            if (!candidatos.Any())
            {
                throw new Exception("No hay usuarios disponibles para asignar el ítem.");
            }

            // 3. Aplicar la lógica de asignación
            var diasParaEntrega = (item.Fechaentrega - DateTime.UtcNow).TotalDays;
            Usuario usuarioElegido;

            if (diasParaEntrega < 3)
            {
                // Si la fecha de entrega es próxima, se asigna al usuario con menos ítems asignados (total)
                usuarioElegido = candidatos.OrderBy(u => u.TotalAsignados).First();
            }
            else if (item.Relevancia.Equals("alta", StringComparison.OrdinalIgnoreCase))
            {
                // Si el ítem es altamente relevante, se asigna al usuario con menos ítems pendientes
                usuarioElegido = candidatos.OrderBy(u => u.Pendientes).First();
            }
            else
            {
                // En otro caso, asignar al usuario con menos ítems asignados
                usuarioElegido = candidatos.OrderBy(u => u.TotalAsignados).First();
            }

            // Actualizar el usuario en el microservicio GestiónDeUsuarios
            usuarioElegido.TotalAsignados += 1;
            usuarioElegido.Pendientes += 1;
            if (item.Relevancia.Equals("alta", StringComparison.OrdinalIgnoreCase))
            {
                usuarioElegido.AltamenteRelevantes += 1;
            }

            // Llamada PUT para actualizar el usuario (la ruta usa [Route("ActualizarUsuario")])
            var updateResponse = await _httpClient.PutAsJsonAsync($"{_gestionUsuariosApiUrl}/api/Usuarios/ActualizarUsuario?username={usuarioElegido.Username}", usuarioElegido);
            if (!updateResponse.IsSuccessStatusCode)
            {
                throw new Exception("Error al actualizar el usuario en el microservicio GestiónDeUsuarios.");
            }

            // Asignar el usuario al ítem y guardarlo en la base de datos local
            var fechaEntrega = item.Fechaentrega;
            item.Fechaentrega = Convert.ToDateTime(fechaEntrega);
            item.Usuarioasignado = usuarioElegido.Username;
            item.Createdat = DateTime.Now;
            item.Createdat = DateTime.Now;

            var createdItem = await _itemService.CreateItemAsync(item);
            return createdItem;
        }
    }
}
