using Microsoft.EntityFrameworkCore;
using TesteKeyworks.Data;
using TesteKeyworks.Repositories;
using TesteKeyworks.Services.Avaliacoes;
using TesteKeyworks.Services.Filmes;
using TesteKeyworks.Services.Streamings;
namespace TesteKeyworks
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TesteKeyworksContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TesteKeyworksContext") ?? throw new InvalidOperationException("Connection string 'TesteKeyworksContext' not found.")));


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IFilmeService, FilmeService>();
            builder.Services.AddScoped<IAvaliacaoService, AvaliacaoService>();
            builder.Services.AddScoped<IStreamingService, StreamingService>();
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TesteKeyworksContext>();
                await dbContext.Database.MigrateAsync();

                var dbInitializer = new DatabaseInitializer(scope.ServiceProvider.GetRequiredService<IServiceScopeFactory>());
                await dbInitializer.InitializeAsync();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Filmes}/{action=Index}/{id?}");

            app.Run();
        }


    }
}
