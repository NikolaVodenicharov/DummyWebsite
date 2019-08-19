using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Products.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Tyres;
using static Tyres.Service.Constants.PageConstants;

namespace Tyres.Service.Implementations
{
    public class TyreService : AbstractService, ITyreService
    {
        public TyreService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public async Task<TyreDetailsDTO> Get(int id)
        {
            var tyre = await base.db
                .Tyres
                .FindAsync(id);

            if (tyre == null)
            {
                return null;
            }

            var model = base.mapper
                .Map<TyreDetailsDTO>(tyre);

            return model;
        }

        public async Task<IEnumerable<TyreSummaryDTO>> GetAllListingAsync(Width width, Ratio ratio, Diameter diameter, Season season, int page = DefaultPage)
        {
            var tyresQuery = this.CreateQueryByParameters(width, ratio, diameter, season);

            var listedTyres = tyresQuery
                .OrderBy(t => t.Price)
                .Skip(PageSize * (page - 1))
                .Take(PageSize)
                .Select(t => new TyreSummaryDTO
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
                .ToListAsync();

            return await listedTyres;
        }

        public async Task<int> GetPagesCount (Width width, Ratio ratio, Diameter diameter, Season season)
        {
            var tyresQuery = this.CreateQueryByParameters(width, ratio, diameter, season);
            var tyresCountByParameters = await tyresQuery.CountAsync();

            var pages = (int)Math.Ceiling(
                (double)tyresCountByParameters / PageSize);

            return pages;
        }

        public async Task<bool> Create<T>(CreateTyreDTO<T> model)
        {
            var tyre = new Tyre
            {
                Brand = model.Brand,
                Model = model.Model,
                Price = model.Price,
                Quantity = model.Quantity,
                Width = model.Width,
                Ratio = model.Ratio,
                Diameter = model.Diameter,
                Season = model.Season,
                Speed = model.Speed,
                Load = model.Load,
                FuelEfficient = model.FuelEfficient,
                WetGrip = model.WetGrip,
                Noice = model.Noice
            };

            db.Tyres.Add(tyre);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateQuantity(int id, int quantity)
        {
            var tyre = await db.Tyres.FindAsync(id);

            if (tyre == null || quantity < 0)
            {
                return false;
            }

            tyre.Quantity = quantity;
            await db.SaveChangesAsync();

            return true;
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
