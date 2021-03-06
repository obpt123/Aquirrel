﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal
{
    public class HasMaxLengthAttributeConvention : PropertyAttributeConvention<HasMaxLengthAttribute>
    {
        public override InternalPropertyBuilder Apply(InternalPropertyBuilder propertyBuilder, HasMaxLengthAttribute attribute, MemberInfo clrMember)
        {
            propertyBuilder.HasMaxLength(ConfigurationSource.DataAnnotation, attribute.HasMaxLength);
            return propertyBuilder;
        }
    }
}
