using CodingExercise.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAsync();
        Task<Item> GetAsync(string key);
        Task<Item> CreateAsync(Item newItem);
        Task<Item> UpdateAsync(Item changedItem);
        Task<Item> DeleteAsync(string key);
    }
}
