﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using Aquirrel.EntityFramework.Sharding;

namespace Aquirrel.EntityFramework.Test
{

    public class Startup_RV
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var mysql = "Data Source=localhost;Port=3306;Database=test;User ID=root;Password=123456;CharSet=utf8;Allow User Variables=True";

            //IServiceCollection services = new ServiceCollection();

            services.AddEntityFrameworkMySql();

            var appsettings = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

            services.AddDbContext<RVDbContext>((sp, op) =>
            {
                op.UseInternalServiceProvider(sp);
                op.UseMySql(mysql, mysqlOption =>
                {
                    mysqlOption.CommandTimeout(10);
                    mysqlOption.UseRelationalNulls(false);
                });
                op.ConfigureEntityMappings(typeof(RVDbContext).Assembly);
            });
            services.AddAquirrelDb();
            services.AddLogging(op =>
            {
                op.AddConfiguration(appsettings.GetSection("FileLogging"));
                op.AddFile(appsettings.GetSection("FileLogging"));
                op.SetMinimumLevel(LogLevel.Trace);
                op.AddConsole(cp => cp.IncludeScopes = true);
            });

            Console.WriteLine("ConfigureServices finish");
            return services.BuildServiceProvider();
        }
    }

    public class Startup
    {
        public static string SqlConnectionString = "server=.;database=test_ef_core;uid=sa;pwd=sasa;";

        public static string SqlConnectionString_Log = "server=.;database=log_ef_core;uid=sa;pwd=sasa;";

        public static string mysql = "Data Source=localhost;Port=3306;Database=test;User ID=root;Password=123456;CharSet=utf8;Allow User Variables=True";
        IConfiguration appsettings;
        public Startup(IHostingEnvironment env)
        {
            appsettings = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
        public IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            Console.WriteLine("Startup   ConfigureServices");

            //serviceCollection.AddEntityFrameworkSqlServer();
            serviceCollection.AddEntityFrameworkMySql();

            serviceCollection.AddLogging(op =>
                {
                    op.AddConfiguration(this.appsettings.GetSection("FileLogging"));
                    op.AddDebug();
                    op.AddConsole(op2 => { op2.IncludeScopes = true; });
                    op.AddFile(this.appsettings.GetSection("FileLogging"));
                })

               .AddDbContext<TestDbContext>((sp, opt) =>
               {
                   opt.UseInternalServiceProvider(sp);

                   //opt.UseSqlServer(Startup.SqlConnectionString, sqlOpt =>
                   //{
                   //    sqlOpt.CommandTimeout(10);
                   //    sqlOpt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                   //    sqlOpt.UseRowNumberForPaging(true);
                   //    sqlOpt.UseRelationalNulls(false);
                   //});

                   opt.UseMySql(Startup.mysql, mysqlOption =>
                   {
                       mysqlOption.CommandTimeout(10);
                       mysqlOption.UseRelationalNulls(false);
                   });

                   opt.ConfigureEntityMappings(this.GetType().Assembly);
                   opt.ConfigureAutoEntityAssemblys(this.GetType().Assembly);

               })
               //.AddDbContext<LogDbContext>((sp, opt) =>
               //{
               //    opt.UseInternalServiceProvider(sp);
               //    opt.UseSqlServer(Startup.SqlConnectionString_Log, sqlOpt =>
               //    {
               //        sqlOpt.CommandTimeout(10);
               //        sqlOpt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
               //        sqlOpt.UseRowNumberForPaging(true);
               //        sqlOpt.UseRelationalNulls(false);
               //    });
               //    opt.ConfigureEntityMappings(typeof(project.Entity.Log).Assembly);
               //    opt.ConfigureAutoEntityAssemblys(typeof(project.Entity.Log).Assembly);
               //})
               .AddAquirrelDb();
            //.AddAquirrelShardingDb<MyShardingFactory>();

            serviceCollection.AddScoped<project.Repository.ShardTableRepo>();


            return serviceCollection.BuildServiceProvider();
        }
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

        }
    }
    class MyShardingFactory : Sharding.ShardingDbFactory
    {
        public MyShardingFactory(IServiceProvider provider) : base(provider)
        {
        }

        public override DbContextOptionsBuilder<TContext> BuilderDbContextOptions<TContext>(DbContextOptionsBuilder<TContext> builder,
            ShardingOptions options)
        {
            if (options.ShardingDbValue.IsNotNullOrEmpty())
            {
                if (typeof(LogDbContext).IsAssignableFrom(typeof(TContext)))
                {
                    var connStr = Startup.SqlConnectionString_Log.Replace("log_ef_core", "log_ef_core_" + options.ShardingDbValue);
                    builder.UseSqlServer(connStr);
                }
            }
            return builder;
        }
        public override string GetShardingTableName<TContext, TEntity>(ShardingOptions options)
        {
            if (options.ShardingTableValue.IsNotNullOrEmpty())
            {
                return options.ShardingTableValue;
            }
            return base.GetShardingTableName<TContext, TEntity>(options);
        }
    }
}
