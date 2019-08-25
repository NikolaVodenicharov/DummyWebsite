using System;
using System.Collections.Generic;
using System.Linq;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models.Products;

namespace Tyres.Data
{
    public static class DbInitializer
    {
        public static void Initialize(TyresDbContext db)
        {
            db.Database.EnsureCreated();


            TyreSeeder(db);
        }

        private static void TyreSeeder(TyresDbContext db)
        {
            if (db.Tyres.Any())
            {
                return;
            }

            #region DataArrays 
            var brands = new string[]
            {
                "Goodyear",
                "Michelin",
                "Dunlop",
                "Pirelli",
                "Continental",
                "Bridgestone"
            };

            var models = new string[]
            {
                "Pilot",
                "Sport",
                "Max efficient",
                "Primary",
                "Corsser"
            };

            var widths = Enum.GetValues(typeof(Width));
            var ratios = Enum.GetValues(typeof(Ratio));
            var diameters = Enum.GetValues(typeof(Diameter));
            var seasons = Enum.GetValues(typeof(Season));
            #endregion

            var random = new Random();
            var tyres = new List<Tyre>();

            for (int i = 0; i < 500; i++)
            {
                var brandIndex = random.Next(brands.Length);
                var modelIndex = random.Next(models.Length);
                var widthIndex = random.Next(1, widths.Length);
                var ratioIndex = random.Next(1, ratios.Length);
                var diameterIndex = random.Next(1, diameters.Length);
                var seasonIndex = random.Next(1, seasons.Length);

                var tyre = new Tyre
                {
                    Brand = brands[brandIndex],
                    Model = models[modelIndex],
                    Price = random.Next(100, 200),
                    Quantity = random.Next(10, 30),
                    Width = (Width)widths.GetValue(widthIndex),
                    Ratio = (Ratio)ratios.GetValue(ratioIndex),
                    Diameter = (Diameter)diameters.GetValue(diameterIndex),
                    Season = (Season)seasons.GetValue(seasonIndex),
                    Speed = SpeedIndex.H,
                    Load = LoadIndex._75,
                    FuelEfficient = FuelEfficient.A,
                    WetGrip = WetGrip.A,
                    Noice = Noice._70
                };

                tyres.Add(tyre);
            }

            db.Tyres.AddRange(tyres);
            db.SaveChanges();
        }
    }
}
