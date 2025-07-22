using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Data.ReplicaRouting;
using WebApi.Extenstions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WriteDbConnection"),
    o => o.EnableRetryOnFailure()));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CircuitBreakerPipelineRegistry>();

builder.Services.AddSingleton(provider =>
{
    var circuit = provider.GetRequiredService<CircuitBreakerPipelineRegistry>();
    return new ReadReplicaSelector(
    [
        builder.Configuration.GetConnectionString("replica1")!,
        builder.Configuration.GetConnectionString("replica2")!,
    ], circuit);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.Logger.LogInformation("Environment: {EnvironmentName}", app.Environment.EnvironmentName);
if (app.Environment.IsDevelopment())
{
    await Task.Delay(7000); // Wait for the Debezium to be ready
    await app.MigrateDatabase();
    app.Logger.LogInformation("Database has been migrated.");
    await app.SendDebeziumConfig();
    app.Logger.LogInformation("Debezium has been configured.");

}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
