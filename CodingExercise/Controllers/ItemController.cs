using CodingExercise.CustomAttributes;
using CodingExercise.Model;
using CodingExercise.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingExercise.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemService itemService, ILogger<ItemController> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {       
            var items = await _itemService.GetAsync();

            return Ok(items);
        }

        [HttpGet]
        [Route("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var item = await _itemService.GetAsync(key);

            if(item == null)
            {
                return NotFound();
            }

            _logger.LogInformation($"Request {HttpContext.Request.Method}: {HttpContext.Request.Path.Value} Ok");
            return Ok(item);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Item newItem)
        {
            var createdItem = await _itemService.CreateAsync(newItem);

            return CreatedAtAction(nameof(Get), new { key = createdItem.Key }, createdItem);
        }

        [HttpPut]
        [Route("{key}")]
        [ValidateModel]
        public async Task<IActionResult> Put(string key, [FromBody] Item changedItem)
        {
            if(key != changedItem.Key)
            {
                return BadRequest();
            }

            var updatedItem = await _itemService.UpdateAsync(changedItem);

            if(updatedItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [Route("{key}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string key)
        {
            var removedItem = await _itemService.DeleteAsync(key);

            if (removedItem == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
