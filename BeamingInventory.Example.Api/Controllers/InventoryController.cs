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
            var info = new InventoryInfo {CurrentCount = _inventoryManager.GetCurrentCount()};
            var response =
                new ApiResponse<InventoryInfo>(true, info)
                {
                    Message = $"In inventory: {info.CurrentCount}"
                };
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddInventory([FromBody] PostSaleVm vm)
        {
            var change = _inventoryManager.Insert(vm.Count);
            var response = new ApiResponse<InventoryChange>(true, change)
            {
                Message = $"Now in inventory: {change.CurrentCount}. Previously: {change.PreviousCount}"
            };
            return Ok(response);
        }
    }
}
