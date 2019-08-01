using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Service.Models
{
    public class TyreSummary
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public Width Width { get; set; }

        public Ratio Ratio { get; set; }

        public Diameter Diameter { get; set; }

        public Season Season { get; set; }
    }
}
