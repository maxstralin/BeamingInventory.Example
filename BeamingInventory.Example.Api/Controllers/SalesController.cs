using System;
using Microsoft.AspNetCore.Mvc;
using BeamingInventory.Example.Api.Services;
using BeamingInventory.Example.Api.ViewModels;

namespace BeamingInventory.Example.Api.Controllers
{
    public class SalesController : ApiControllerBase
    {
        private readonly IInventoryManager _inventoryManager;

        public SalesController(IInventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        [HttpPost]
        public IActionResult PostSale([FromBody] PostSaleVm vm)
        {
            try
            {
                var res = _inventoryManager.Sell(vm.Count);
                return Ok(res);
            }
            //Would be better to have a more specific exception thrown by IInventoryManager but for the example,
            //we can assume here that it's more that is being sold than exists
            catch (InvalidOperationException)
            {
                return BadRequest("You cannot sell more than you have in the inventory");
            }
            
        }
    }
}
