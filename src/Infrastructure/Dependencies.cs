﻿using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.eShopWeb.Infrastructure;

public static class Dependencies
{
    public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
    {
        bool useOnlyInMemoryDatabase = false;
        if (configuration["UseOnlyInMemoryDatabase"] != null)
        {
            useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);
        }

        if (useOnlyInMemoryDatabase)
        {
            services.AddDbContext<CatalogContext>(c =>
               c.UseInMemoryDatabase("Catalog"));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseInMemoryDatabase("Identity"));
        }
        else
        {
            #region keyvault
            var catalogDbConnection = configuration["CatalogDbConnection"];
            var identityDbConnection = configuration["IdentityDbConnection"];

            //use real database
            //Requires LocalDB which can be installed with SQL Server Express 2016
            //https://www.microsoft.com/en-us/download/details.aspx?id=54284
            services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer(catalogDbConnection));

            //Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(identityDbConnection));
            #endregion

            //services.AddDbContext<CatalogContext>(c =>
            //    c.UseSqlServer(configuration.GetConnectionString("CatalogConnection")));

            //services.AddDbContext<AppIdentityDbContext>(options =>
            //   options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
        }
    }
}
