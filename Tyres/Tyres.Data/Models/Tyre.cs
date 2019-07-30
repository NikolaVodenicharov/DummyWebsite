﻿using System.ComponentModel.DataAnnotations;
using Tyres.Data.Enums;
using static Tyres.Data.Constants.Validations.TyreValidationConstants;

namespace Tyres.Data.Models
{
    public class Tyre
    {
        public int Id { get; set; }

        [Required, MaxLength(BrandMaxLength)]
        public string Brand { get; set; }

        [Required, MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        public Width Width { get; set; }
        public Ratio Ratio { get; set; }
        public Diameter Diameter { get; set; }
        public Season Season { get; set; }
        public SpeedIndex Speed { get; set; }
        public LoadIndex Load { get; set; }
        public FuelEfficient FuelEfficient { get; set; }
        public WetGrip WetGrip { get; set; }
        public Noice Noice { get; set; }
    }
}