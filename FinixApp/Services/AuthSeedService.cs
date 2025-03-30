namespace backend.Services
{
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using backend.Data.ModelsEF;

    public class AuthSeedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;


        public AuthSeedService(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var adminRole = await db.TRoles.FirstOrDefaultAsync(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new TRole { Name = "admin" };
                db.TRoles.Add(adminRole);
                await db.SaveChangesAsync();
            }

            var existingUser = await db.TUsers.FirstOrDefaultAsync(u => u.Email == _config["AdminUser:Email"]!);
            if (existingUser == null)
            {
                var hasher = new PasswordHasher<TUser>();
                var adminUser = new TUser
                {
                    Username = _config["AdminUser:Username"]!,
                    Email = _config["AdminUser:Email"]!,
                    RoleId = adminRole.RoleId,
                };

                adminUser.PasswordHash = hasher.HashPassword(adminUser, _config["AdminUser:Password"]!);

                db.TUsers.Add(adminUser);
                await db.SaveChangesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }


}
