using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemsDeTrabajo.Models;

namespace ItemsDeTrabajo.Business.Services
{
    public interface IItemstrabajoService
    {
        Task<List<Itemstrabajo>> GetAllItemsAsync();
        Task<Itemstrabajo> CreateItemAsync(Itemstrabajo item);
    }
}
