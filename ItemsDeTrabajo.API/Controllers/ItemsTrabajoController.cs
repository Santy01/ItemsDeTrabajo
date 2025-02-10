using ItemsDeTrabajo.Business.Services;
using ItemsDeTrabajo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemsDeTrabajo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsTrabajoController : ControllerBase
    {
        private readonly IItemstrabajoService _itemService;
        private readonly AsignacionService _asignacionService;

        public ItemsTrabajoController(IItemstrabajoService itemService, AsignacionService asignacionService)
        {
            _itemService = itemService;
            _asignacionService = asignacionService;
        }
        // POST: api/ItemsTrabajo/AsignarItems
        [HttpPost]
        [Route("AsignarItems")]
        public async Task<IActionResult> CreateAndAssignItem([FromBody] Itemstrabajo item)
        {
            if (item == null)
            {
                return BadRequest("El ítem de trabajo no puede ser nulo.");
            }
            try
            {
                var createdItem = await _asignacionService.CreateAndAssignWorkItemAsync(item);
                return CreatedAtAction(nameof(GetItems), new { id = createdItem.Id }, createdItem);
            }
            catch (Exception ex)
            {
                // Se recomienda loggear la excepción para análisis en producción
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/ItemsTrabajo
        [HttpGet]
        [Route("ListaItems")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        // POST: api/ItemsTrabajo
        [HttpPost]
        [Route("CrearItem")]
        public async Task<IActionResult> CreateItem([FromBody] Itemstrabajo item)
        {
            if (item == null)
            {
                return BadRequest("El ítem no puede ser nulo.");
            }

            // Opcional: asignar valores por defecto antes de persistir.
            item.Createdat = DateTime.Now;
            item.Createdat = DateTime.Now;

            var createdItem = await _itemService.CreateItemAsync(item);
            return CreatedAtAction(nameof(GetItems), new { id = createdItem.Id }, createdItem);
        }
    }
}
