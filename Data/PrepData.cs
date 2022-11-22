using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PrepData
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var servicescope = app.ApplicationServices.CreateScope())
            {
                SeedData(servicescope.ServiceProvider.GetService<AppDBContext>());
            }
        }
        private static void SeedData(AppDBContext context){
            Console.WriteLine("Seeding Data");
            if(!context.Platforms.Any()){
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