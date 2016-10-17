/*
* This file is part of TuryUtilCs, the free C# utility collection.
* Copyright (C) 2016 Till Fischer aka Turysaz
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License v2 as published
* by the Free Software Foundation.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along
* with this program; if not, write to the Free Software Foundation, Inc.,
* 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuryUtilCs.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuryUtilCs.Mathmatics.Tests
{
    [TestClass()]
    public class GeneralMath_Tests
    {
        private double[] inputDouble = new double[] { 0, 1, 170.98, -3800, System.Math.PI };


        [TestMethod()]
        public void DegToRad_Test()
        {
            foreach (double i in inputDouble)
            {
                double expected = i / 180.0 * System.Math.PI;
                double actual = GeneralMath.DegToRad(i);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void RadToDeg_Test()
        {
            foreach (double i in inputDouble)
            {
                double expected = i / System.Math.PI * 180.0;
                double actual = GeneralMath.RadToDeg(i);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod()]
        public void QuadraticEquation_Test()
        {
            List<double[]> coefficients = new List<double[]>();
            coefficients.Add(new double[] { 2, 0 });
            coefficients.Add(new double[] { -0.2, -4 });
            coefficients.Add(new double[] { 2.5, -100 });
            coefficients.Add(new double[] { 3, -7 });

            foreach (double[] pair in coefficients)
            {
                double[] zeroes = GeneralMath.QuadraticEquation(pair[0], pair[1]);
                double mid = (zeroes[0] + zeroes[1]) / 2;

                double actualZero0 = System.Math.Round(zeroes[0] * zeroes[0] + 
                    pair[0] * zeroes[0] + pair[1], 10);
                double actualZero1 = System.Math.Round(zeroes[1] * zeroes[1] + 
                    pair[0] * zeroes[1] + pair[1], 10);
                double actualmid = System.Math.Round(mid * mid + 
                    pair[0] * mid + pair[1], 10);

                Assert.AreEqual(0, actualZero0);
                Assert.AreEqual(0, actualZero1);
                Assert.AreNotEqual(0, mid);
            }
        }
    }
}