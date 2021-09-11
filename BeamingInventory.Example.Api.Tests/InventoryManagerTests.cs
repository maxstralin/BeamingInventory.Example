using System;
using BeamingInventory.Example.Api.Services;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace BeamingInventory.Example.Api.Tests
{
    public class InventoryManagerTests
    {
        private readonly InventoryManager _inventoryManager;

        public InventoryManagerTests()
        {
            _inventoryManager = new InventoryManager(new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public void CanInsert()
        {
            const int expected = 2;
            
            var result = _inventoryManager.Insert(expected);

            Assert.Equal(expected, result.CurrentCount);
        }

        [Fact]
        public void CanSell()
        {
            const int toInsert = 4;
            const int toSell = 2;
            const int expected = 2;

            _inventoryManager.Insert(toInsert);
            var result = _inventoryManager.Sell(toSell);

            Assert.Equal(expected, result.CurrentCount);
        }

        [Fact]
        public void AssertCurrentReturnValuesAreCorrect()
        {
            const int toInsert = 4;
            const int toSell = 2;
            const int expectedAfterInsert = 4;
            const int expectedAfterSell = expectedAfterInsert - toSell;
            const int expectedInitialCount = 0;

            var initialCount = _inventoryManager.GetCurrentCount();
            var afterInsert = _inventoryManager.Insert(toInsert);
            var afterSell = _inventoryManager.Sell(toSell);

            Assert.Equal(expectedInitialCount, initialCount);
            Assert.Equal(expectedAfterInsert, afterInsert.CurrentCount);
            Assert.Equal(expectedAfterSell, afterSell.CurrentCount);
        }

        [Fact]
        public void AssertPreviousReturnValuesAreCorrect()
        {
            const int toInsert = 4;
            const int toSell = 2;
            const int expectedAfterInsert = 4;
            const int expectedInitialCount = 0;

            var afterInsert = _inventoryManager.Insert(toInsert);
            var afterSell = _inventoryManager.Sell(toSell);

            Assert.Equal(expectedInitialCount, afterInsert.PreviousCount);
            Assert.Equal(expectedAfterInsert, afterSell.PreviousCount);
        }

        [Fact]
        public void ThrowsIfNoneToSell()
        {
            const int toSell = 2;

            Assert.Throws<InvalidOperationException>(() => _inventoryManager.Sell(toSell));
        }
    }
}
