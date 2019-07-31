using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Products.Data.Models;

namespace Tyres.Service.Interfaces
{
    public interface ITyreService
    {
        IEnumerable<Tyre> All(Width width, Ratio ration, Diameter diameter, Season season); // Brand ?
    }
}
