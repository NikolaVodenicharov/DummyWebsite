using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Shared.DataTransferObjects
{
    public class TestTyreSize
    {
        public Width Width { get; set; }
        public Ratio Ratio { get; set; }
        public Diameter Diameter { get; set; }
    }
}
