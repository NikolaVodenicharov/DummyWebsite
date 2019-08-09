using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Products.Data.Models;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Service.Models;
using static Tyres.Service.Constants.PageConstants;

namespace Tyres.Service.Implementations
{
    public class TyreService : AbstractService, ITyreService
    {
        public TyreService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public TyreDetails Get(int id)
        {
            var tyre = base.db
                .Tyres
                .Find(id);

            if (tyre == null)
            {
                return null;
            }

            var model = base.mapper
                .Map<TyreDetails>(tyre);

            return model;
        }

        public IEnumerable<TyreSummary> GetAllListing(Width width, Ratio ratio, Diameter diameter, Season season, int page = DefaultPage)
        {
            var tyresQuery = this.CreateQueryByParameters(width, ratio, diameter, season);

            var listedTyres = tyresQuery
                .OrderBy(t => t.Price)
                .Skip(PageSize * (page - 1))
                .Take(PageSize)
                .Select(t => new TyreSummary
                {
                    Id = t.Id,
                    Brand = t.Brand,
                    Model = t.Model,
                    Price = t.Price,
                    Width = (int)t.Width,
                    Ratio = (int)t.Ratio,
                    Diameter = (int)t.Diameter,
                    Season = t.Season.ToString()
                })
                .ToList();

            return listedTyres;
        }

        public int GetPagesCount (Width width, Ratio ratio, Diameter diameter, Season season)
        {
            var tyresQuery = this.CreateQueryByParameters(width, ratio, diameter, season);
            var tyresCountByParameters = tyresQuery.Count();

            var pages = (int)Math.Ceiling(
                (double)tyresCountByParameters / PageSize);

            return pages;
        }

        private IQueryable<Tyre> CreateQueryByParameters (Width width, Ratio ratio, Diameter diameter, Season season)
        {
            var tyresQuery = base.db
                .Tyres
                .Where(t => t.Quantity > 0)
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

            return tyresQuery;
        }
    }
}
