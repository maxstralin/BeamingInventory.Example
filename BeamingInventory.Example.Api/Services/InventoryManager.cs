using System;
using BeamingInventory.Example.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BeamingInventory.Example.Api.Services
{
    //We could assume most of these methods would be async but for this example, we'll let them be synchronous 
    public class InventoryManager : IInventoryManager
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "INVENTORY_COUNT";

        public InventoryManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public int GetCurrentCount() => _cache.GetOrCreate(CacheKey, entry => 0);

        private int SetCurrentCount(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), count, "Cannot set the inventory to less than zero");
            _cache.Set(CacheKey, count);
            return _cache.Get<int>(CacheKey);
        }

        public InventoryChange Insert(int count)
        {
            var initialCount = GetCurrentCount();
            var newValue = initialCount + count;
            if (newValue < 0)
                throw new InvalidOperationException($"Cannot modify inventory by {count} as it would become less than zero, current inventory: {initialCount}");
            return new InventoryChange { PreviousCount = initialCount, CurrentCount = SetCurrentCount(newValue)};
        }

        public InventoryChange Sell(int count) => Insert(count * -1);

    }
}
