using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PrepData
    {
        public static void PrepPopulation(IApplicationBuilder app,bool IsProduction)
        {
            using(var servicescope = app.ApplicationServices.CreateScope())
            {
                SeedData(servicescope.ServiceProvider.GetService<AppDBContext>(), IsProduction);
            }
        }
        private static void SeedData(AppDBContext context,bool IsProduction){

            if(IsProduction){
                Console.WriteLine("Migration started in SQL Server");
                try{
                    context.Database.Migrate();
                }
                catch(Exception ex){
                    Console.WriteLine($"Couldn't migrate --> {ex.Message}");
                }
            }

            if(!context.Platforms.Any()){
                Console.WriteLine("Seeding Data");
                context.Platforms.AddRange(
                    new Platform(){Name="Dot Net",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="SQL Server",Publisher="Microsoft",Cost="Free"},
                    new Platform(){Name="Kubernetes",Publisher="Cloud Native Computing Foundation",Cost="Free"}
                    );
                context.SaveChanges();
            }else{
                Console.WriteLine("Data already present");
            }
        }
    }
}