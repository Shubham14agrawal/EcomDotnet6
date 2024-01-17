using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (!context.ProductBrands.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductBrands ON;");
                        var brandsData = File.ReadAllText(path + @"/Data/SeedData/brands.json");
                        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                        foreach (var item in brands)
                        {
                            context.ProductBrands.Add(item);
                        }

                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }

                if (!context.ProductTypes.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.ProductTypes ON;");
                        var typesData = File.ReadAllText(path + @"/Data/SeedData/types.json");
                        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                        foreach (var item in types)
                        {
                            context.ProductTypes.Add(item);
                        }

                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }

                if (!context.Products.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                        var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                        foreach (var item in products)
                        {
                            context.Products.Add(item);
                        }

                        await context.SaveChangesAsync();

                        transaction.Commit();
                    }
                }

                if (!context.DeliveryMethods.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.DeliveryMethods ON;");
                        var dmData = File.ReadAllText(path + @"/Data/SeedData/delivery.json");
                        var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                        foreach (var item in methods)
                        {
                            context.DeliveryMethods.Add(item);
                        }

                        await context.SaveChangesAsync();
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
