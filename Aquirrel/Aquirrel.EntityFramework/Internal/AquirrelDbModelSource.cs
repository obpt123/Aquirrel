﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aquirrel.EntityFramework.Internal
{
    public class AquirrelDbModelSource : SqlServerModelSource
    {
        
        public AquirrelDbModelSource(IDbSetFinder setFinder, ICoreConventionSetBuilder coreConventionSetBuilder, IModelCustomizer modelCustomizer, IModelCacheKeyFactory modelCacheKeyFactory)
            : base(setFinder, coreConventionSetBuilder, modelCustomizer, modelCacheKeyFactory)
        {
            Console.WriteLine("AquirrelDbModelSource.ctor");
        }
        protected override ConventionSet CreateConventionSet(IConventionSetBuilder conventionSetBuilder)
        {
            Console.WriteLine("AquirrelDbModelSource.CreateConventionSet");
            var x = base.CreateConventionSet(conventionSetBuilder);
            x.ModelBuiltConventions.Add(new DefaultStringLengthConvention(32));
            Console.WriteLine("AquirrelDbModelSource.ModelBuiltConventions add DefaultStringLengthConvention");
            return x;
        }
    }
}
