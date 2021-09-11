using Microsoft.AspNetCore.Mvc;
using BeamingInventory.Example.Api.Models;
using BeamingInventory.Example.Api.Services;
using BeamingInventory.Example.Api.ViewModels;

namespace BeamingInventory.Example.Api.Controllers
{
    public class InventoryController : ApiControllerBase
    {
        private readonly IInventoryManager _inventoryManager;

        public InventoryController(IInventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
        }

        [HttpGet]
        public IActionResult GetInventory()
        {
            return Ok(new InventoryInfo { CurrentCount = _inventoryManager.GetCurrentCount() });
        }

        [HttpPost]
        public IActionResult AddInventory([FromBody] PostSaleVm vm)
        {
            return Ok(_inventoryManager.Insert(vm.Count));
        }
    }
}
