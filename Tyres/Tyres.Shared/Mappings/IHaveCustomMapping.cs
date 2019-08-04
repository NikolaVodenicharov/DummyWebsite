using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tyres.Shared.Mappings
{
    /// <summary>
    /// We use this class to make custom mapping between properties of source and destination type, when the names
    /// of the properties are not the same. In that case the default mapping cant map them. In the ConfigureMapping 
    /// method, each destination type must configure the properties that are with different names from the source type.
    /// And the reflection in the AutoMapperProfile will make the additional mappings.
    /// </summary>
    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile profile);
    }
}
