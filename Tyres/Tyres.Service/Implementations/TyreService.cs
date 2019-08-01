using System;
using System.Collections.Generic;
using System.Linq;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Products.Data.Models;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Service.Models;

namespace Tyres.Service.Implementations
{
    public class TyreService : AbstractService, ITyreService
    {
        public TyreService(TyresDbContext db) 
            : base(db)
        {
        }

        public IEnumerable<TyreSummary> AllListing(Width width, Ratio ratio, Diameter diameter, Season season, int page = PageConstants.DefaultPage)
        {
            var tyresQuery = base.db
                .Tyres
                .AsQueryable();

            if (width != Width.All)
            {
                tyresQuery = tyresQuery.Where(t => t.Width == width);
            }
            if (ratio != Ratio.All)
            {
                tyresQuery = tyresQuery.Where(t => t.Ratio == ratio);
            }
            if (diameter != Diameter.All)
            {
                tyresQuery = tyresQuery.Where(t => t.Diameter == diameter);
            }
            if (season != Season.All)
            {
                tyresQuery = tyresQuery.Where(t => t.Season == season);
            }

            var listedTyres = tyresQuery
                .OrderBy(t => t.Price)
                .Skip(PageConstants.PageSize * (page - 1))
                .Take(PageConstants.PageSize)
                .Select(t => new TyreSummary
                {
                    Id = t.Id,
                    Brand = t.Brand,
                    Model = t.Model,
                    Price = t.Price,
                    Width = t.Width,
                    Ratio = t.Ratio,
                    Diameter = t.Diameter,
                    Season = t.Season
                })
                .ToList();

            return listedTyres;
        }
    }
}
