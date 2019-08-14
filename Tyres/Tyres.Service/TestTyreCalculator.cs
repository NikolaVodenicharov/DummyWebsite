using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Shared.DataTransferObjects;

namespace Tyres.Service
{
    public class TestTyreCalculator
    {
        public double TyreDifference (TestTyreSize oldSize, TestTyreSize newSize)
        {
            var oldHeight = this.CalculateTyreHeight(oldSize);
            var newHeight = this.CalculateTyreHeight(newSize);

            var difference = oldHeight - newHeight;

            return difference;
        }

        private double CalculateTyreHeight(TestTyreSize tyre)
        {
            var widht = (int)tyre.Width;
            var ratio = (int)tyre.Ratio;
            var diameter = (int)tyre.Diameter;

            var profile = (widht * ratio) / 100;
            var rim = diameter * 2.54 * 10;

            var height = 2 * profile + rim;

            return height;
        }
    }
}