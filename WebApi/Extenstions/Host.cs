using Microsoft.EntityFrameworkCore;
using System.Text;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Extenstions
{
    public static class HostExtensions
    {
        public async static Task<IHost> MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                await dbContext.Database.EnsureDeletedAsync();
                await dbContext.Database.MigrateAsync();
                await EnsureDatabaseSeeded(dbContext);
            }
            catch (Exception)
            {
                throw;
            }
            return host;
        }
        public static async Task EnsureDatabaseSeeded(ApplicationDbContext dbContext)
        {
            try
            {
                var itemInventories = new List<ItemInventory>
                {
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Wireless Mouse", Price = 25.99m, StockQty = 150 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Mechanical Keyboard", Price = 89.50m, StockQty = 100 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "27-inch Monitor", Price = 199.99m, StockQty = 75 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "USB-C Cable", Price = 9.99m, StockQty = 500 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Laptop Stand", Price = 34.95m, StockQty = 120 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Noise Cancelling Headphones", Price = 129.99m, StockQty = 60 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Webcam 1080p", Price = 49.90m, StockQty = 80 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Bluetooth Speaker", Price = 59.99m, StockQty = 95 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "External Hard Drive 1TB", Price = 75.00m, StockQty = 110 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Portable SSD 512GB", Price = 99.99m, StockQty = 90 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Graphics Tablet", Price = 120.00m, StockQty = 40 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Wireless Charger", Price = 22.50m, StockQty = 300 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Gaming Chair", Price = 199.00m, StockQty = 35 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Adjustable Desk", Price = 299.99m, StockQty = 20 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Smartwatch", Price = 150.00m, StockQty = 70 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Fitness Tracker", Price = 89.00m, StockQty = 85 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Smart LED Bulb", Price = 12.99m, StockQty = 250 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Tablet 10-inch", Price = 230.00m, StockQty = 45 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "E-book Reader", Price = 119.99m, StockQty = 55 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Smart Home Hub", Price = 89.99m, StockQty = 65 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Mini Projector", Price = 150.00m, StockQty = 25 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Gaming Headset", Price = 79.99m, StockQty = 105 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Streaming Microphone", Price = 99.00m, StockQty = 80 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Phone Tripod", Price = 24.95m, StockQty = 200 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "LED Light Strip", Price = 19.99m, StockQty = 220 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Drone with Camera", Price = 349.00m, StockQty = 15 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "3D Printer", Price = 450.00m, StockQty = 10 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Action Camera", Price = 130.00m, StockQty = 50 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Smart Door Lock", Price = 199.99m, StockQty = 30 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "WiFi Range Extender", Price = 39.99m, StockQty = 180 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Wireless Earbuds", Price = 59.99m, StockQty = 140 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Laptop Backpack", Price = 45.00m, StockQty = 100 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Ergonomic Mouse Pad", Price = 14.99m, StockQty = 160 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "HDMI Cable", Price = 7.99m, StockQty = 300 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Surge Protector", Price = 19.99m, StockQty = 130 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "USB Hub", Price = 17.99m, StockQty = 200 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Cable Organizer", Price = 9.49m, StockQty = 220 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Screen Cleaner Kit", Price = 11.99m, StockQty = 180 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Wireless Presentation Remote", Price = 29.99m, StockQty = 95 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Photo Printer", Price = 159.99m, StockQty = 25 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Laptop Cooling Pad", Price = 21.99m, StockQty = 110 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Smart Plug", Price = 15.49m, StockQty = 260 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Fingerprint Flash Drive", Price = 44.95m, StockQty = 70 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Gaming Monitor 144Hz", Price = 289.00m, StockQty = 30 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Curved Monitor", Price = 249.99m, StockQty = 25 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "VR Headset", Price = 399.99m, StockQty = 12 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "USB Fan", Price = 8.99m, StockQty = 210 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Laptop Docking Station", Price = 109.00m, StockQty = 60 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Dual Monitor Arm", Price = 79.95m, StockQty = 40 },
                    new ItemInventory { ProductId = Guid.NewGuid(), ProductName = "Webcam Cover", Price = 6.99m, StockQty = 400 },
                };
                //var orders = new List<Order>
                //{
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 120.50m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 300.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 89.99m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 150.75m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 215.30m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 55.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 432.10m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 199.99m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 125.20m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 345.60m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 78.99m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 98.45m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 187.30m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 410.99m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 72.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 255.55m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 142.90m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 67.80m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 312.60m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 230.40m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 144.70m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 390.15m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 85.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 215.25m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 123.90m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 275.40m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 92.60m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 330.80m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 155.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 180.10m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 460.90m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 130.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 102.40m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 134.50m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 299.99m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 185.60m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 421.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 114.90m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Processing", TotalAmount = 163.75m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 305.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Shipped", TotalAmount = 88.10m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Cancelled", TotalAmount = 0.00m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Completed", TotalAmount = 412.40m },
                //    new Order { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Status = "Pending", TotalAmount = 110.55m }
                //};

                await dbContext.Inventories.AddRangeAsync(itemInventories);
                //await dbContext.Orders.AddRangeAsync(orders);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<IHost> SendDebeziumConfig(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var httpClient = services.GetRequiredService<HttpClient>();
                var json = """
                    {
                      "name": "my-postgres-connector",
                      "config": {
                        "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
                        "plugin.name": "pgoutput",
                        "database.hostname": "writer_postgres",
                        "database.port": "5432",
                        "database.user": "myapp_user",
                        "database.password": "mysecretpassword",
                        "database.dbname": "myapp_db",
                        "database.server.name": "writer_postgres",
                        "slot.name": "debezium_slot",
                        "publication.name": "debezium_pub",
                        "table.include.list": "public.Inventories,public.Orders,public.OrderItems",
                        "topic.prefix": "ReadReplicas",
                        "key.converter": "org.apache.kafka.connect.json.JsonConverter",
                        "value.converter": "org.apache.kafka.connect.json.JsonConverter",
                        "key.converter.schemas.enable": "false",
                        "value.converter.schemas.enable": "false",
                        "decimal.handling.mode": "double"
                      }
                    }
                    """;
                var config = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync("http://connect:8083/connectors", config);
            }
            catch (Exception)
            {
                throw;
            }
            return host;
        }
    }
}
