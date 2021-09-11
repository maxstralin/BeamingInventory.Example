using BeamingInventory.Example.Api.Models;

namespace BeamingInventory.Example.Api.Services
{
    public interface IInventoryManager
    {
        int GetCurrentCount();
        InventoryChange Insert(int count);
        InventoryChange Sell(int count);
    }
}