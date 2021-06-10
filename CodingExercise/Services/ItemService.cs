using CodingExercise.CustomExeptions;
using CodingExercise.Infrastructure;
using CodingExercise.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Services
{
    public class ItemService : IItemService
    {
        private readonly CodingExerciseContext _context;

        public ItemService(CodingExerciseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAsync()
        {
            var items = await _context.Items.ToListAsync();

            return items;
        }

        public async Task<Item> GetAsync(string key)
        {
            var item = await GetSingleItemFromDb(key);

            return item;
        }

        public async Task<Item> CreateAsync(Item newItem)
        {
            if(!ItemExistInDb(newItem.Key))
            {
                _context.Add(newItem);

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ItemAlreadyExistException(newItem.Key);
            }

            return newItem;
        }

        public async Task<Item> UpdateAsync(Item changedItem)
        {
            var item = await GetSingleItemFromDb(changedItem.Key);

            if (item != null)
            {
                try
                {
                    item.Value = changedItem.Value;

                    _context.Entry(item).State = EntityState.Modified;
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                    
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();

                    string customExceptionMessage = databaseEntry == null ? "The item was deleted by another user" :
                        "The item you attempted to edit was recently modified by another user. If you still want to edit, repeat operation";
                    ex.Data.Add("customMessage", customExceptionMessage);

                    throw;
                }
            }

            return item;       
        }

        public async Task<Item> DeleteAsync(string key)
        {
            var item = await GetSingleItemFromDb(key);

            if(item != null)
            {
                try
                {
                    _context.Entry(item).State = EntityState.Deleted;
                    _context.Items.Remove(item);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();

                    string customExceptionMessage = databaseEntry == null ? "The item is already deleted" :
                        "The item you attempted to delete was recently modified by another user. If you still want to delete, repeat operation";
                    ex.Data.Add("customMessage", customExceptionMessage);

                    throw;
                }
            }

            return item;           
        }

        private async Task<Item> GetSingleItemFromDb(string key)
        {
            return await _context.Items.SingleOrDefaultAsync(i => i.Key == key);
        }

        private bool ItemExistInDb(string key)
        {
            return _context.Items.Any(i => i.Key == key);
        }     
    }
}
