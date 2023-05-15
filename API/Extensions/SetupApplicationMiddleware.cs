using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class SetupApplicationMiddleware
    {
        public static async Task SetupMiddleware(this WebApplication app)
        {
            //ensure database and table exists
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                context.Database.Migrate();
                await Seed.SeedData(context);
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();
            app.MapControllers();
        }
    }
}