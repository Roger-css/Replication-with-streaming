using Microsoft.EntityFrameworkCore;
using worker_service_for_read_repicas.CDCService.Interfaces;
using worker_service_for_read_repicas.Data;
using worker_service_for_read_repicas.Models;

namespace worker_service_for_read_repicas.CDCService.Implementation
{
    public class InventoryCdcHandler : ICdcEventHandler
    {
        private readonly AppDbContext _dbContext;

        public string EntityName => "Inventories";

        public InventoryCdcHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ProcessAsync<T>(GenericCDCEvent<T> cdcEvent)
        {
            if (cdcEvent.Operation is "c" or "r" && cdcEvent.After is ItemInventory inventoryCreate)
            {
                await CreateInventoryAsync(inventoryCreate);
            }
            else if (cdcEvent.Operation is "u" && cdcEvent.After is ItemInventory inventoryUpdate)
            {
                await UpdateInventoryAsync(inventoryUpdate);
            }
            else if (cdcEvent.Operation is "d" && cdcEvent.Before is ItemInventory inventoryDelete)
            {
                await DeleteInventoryAsync(inventoryDelete.ProductId);
            }
        }

        private async Task CreateInventoryAsync(ItemInventory inventory)
        {
            try
            {
                await _dbContext.Inventories.AddAsync(inventory);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task UpdateInventoryAsync(ItemInventory inventory)
        {
            try
            {
                var item = await _dbContext.Inventories
                    .FirstOrDefaultAsync(e => e.ProductId == inventory.ProductId);
                if (item is not null)
                {

                    await _dbContext.Inventories.Where(e => e.ProductId == inventory.ProductId)
                        .ExecuteUpdateAsync(e =>
                        e.SetProperty(e => e.ProductName, inventory.ProductName)
                        .SetProperty(e => e.Price, inventory.Price)
                        .SetProperty(e => e.StockQty, inventory.StockQty));
                }
                else
                {
                    throw new KeyNotFoundException($"Inventory with ProductId {inventory.ProductId} not found.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task DeleteInventoryAsync(Guid productId)
        {
            try
            {
                await _dbContext.Inventories.Where(e => e.ProductId == productId)
                    .ExecuteDeleteAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
