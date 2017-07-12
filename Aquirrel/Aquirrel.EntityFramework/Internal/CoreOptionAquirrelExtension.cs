﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Aquirrel.EntityFramework.Internal
{
    public class CoreOptionAquirrelExtension : IDbContextOptionsExtension
    {
        /// <summary>
        /// 自动加载的entity
        /// </summary>
        public Assembly[] EntityAssebmlys { get; set; }

        /// <summary>
        /// 自动mapping的
        /// </summary>
        public Assembly[] EntityMappingsAssebmlys { get; set; }
        /// <summary>
        /// string默认长度32位
        /// </summary>

        public bool EnableStringDefault32Length { get; set; } = true;

        public string LogFragment => "Aquirrel Core Db Options";

        IServiceCollection services;

        public bool ApplyServices(IServiceCollection services)
        {
            this.services = services;
            return true;
        }

        public long GetServiceProviderHashCode()
        {
            return this.services.GetHashCode();
        }

        public void Validate(IDbContextOptions options)
        {
        }
    }
}
