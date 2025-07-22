using Microsoft.EntityFrameworkCore;
using worker_service_for_read_repicas.CDCService;
using worker_service_for_read_repicas.CDCService.Implementation;
using worker_service_for_read_repicas.CDCService.Interfaces;
using worker_service_for_read_repicas.Data;
using worker_service_for_read_repicas.Extenstions;
using worker_service_for_read_repicas.Kafka;
using worker_service_for_read_repicas.Models;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("readDbConnection")));
builder.Services.AddScoped<ICdcEventHandler, InventoryCdcHandler>();
builder.Services.AddScoped<CDCEventDeserializer>();
builder.Services.AddScoped<StrategyCdcEvent>();
builder.Services.AddScoped(e =>
{
    return new KafkaAdmin(builder.Configuration.GetConnectionString("KafkaConnection")!);
});
builder.Services.AddSingleton<IHostedService>(provider =>
{
    return new KafkaConsumerService<ItemInventory>(
        provider.GetRequiredService<ILogger<KafkaConsumerService<ItemInventory>>>(),
        provider.GetRequiredService<IConfiguration>(),
        provider.GetRequiredService<StrategyCdcEvent>(),
        topic: "ReadReplicas.public.Inventories",
        tableName: "Inventories");
});


var host = builder.Build();
await host.MigrateDatabase();
await host.EnsureKafkaTopics();
await host.RunAsync();




//builder.Services.AddSingleton<IHostedService>(provider =>
//{
//    return new KafkaConsumerService<Order>(
//        provider.GetRequiredService<ILogger<KafkaConsumerService<Order>>>(),
//        provider.GetRequiredService<IConfiguration>(),
//        provider.GetRequiredService<StrategyCdcEvent>(),
//        topic: "ReadReplicas.public.Orders",
//        tableName: "Orders");
//});

//builder.Services.AddSingleton<IHostedService>(provider =>
//{
//    return new KafkaConsumerService<OrderItem>(
//        provider.GetRequiredService<ILogger<KafkaConsumerService<OrderItem>>>(),
//        provider.GetRequiredService<IConfiguration>(),
//        provider.GetRequiredService<StrategyCdcEvent>(),
//        topic: "ReadReplicas.public.OrderItems",
//        tableName: "OrderItems");
//});