using System.Collections.Generic;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Products.Data.Models;
using Tyres.Service.Interfaces;

namespace Tyres.Service.Implementations
{
    public class TyreService : AbstractService, ITyreService
    {
        public TyreService(TyresDbContext db) 
            : base(db)
        {
        }
        public IEnumerable<Tyre> All(Width width, Ratio ration, Diameter diameter, Season season)
        {
            throw new System.NotImplementedException();
        }
    }
}
