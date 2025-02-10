using ItemsDeTrabajo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsDeTrabajo.Business.Services
{
    public class ItemstrabajoService : IItemstrabajoService
    {
        private readonly ItemsTrabajoContext _context;

        public ItemstrabajoService(ItemsTrabajoContext context)
        {
            _context = context;
        }

        public async Task<List<Itemstrabajo>> GetAllItemsAsync()
        {
            // Consulta todos los ítems de trabajo desde la base de datos.
            return await _context.Itemstrabajos.AsNoTracking().ToListAsync();
        }

        public async Task<Itemstrabajo> CreateItemAsync(Itemstrabajo item)
        {
            // Agregar el nuevo ítem a la base de datos.
            _context.Itemstrabajos.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
