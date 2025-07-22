using Microsoft.EntityFrameworkCore;
using worker_service_for_read_repicas.Data;

namespace worker_service_for_read_repicas.Extenstions;

public static partial class HostExtensions
{
    public async static Task<IHost> MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();

        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<AppDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception)
        {
            throw;
        }
        return host;
    }
}
