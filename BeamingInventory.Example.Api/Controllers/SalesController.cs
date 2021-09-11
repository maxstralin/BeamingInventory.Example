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
            return Ok(_inventoryManager.Sell(vm.Count));
        }
    }
}
