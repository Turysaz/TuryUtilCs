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
    public class LinearSystem_Tests
    {
        private LinearSystem GenerateLinearSystem(double[] coefficiants)
        {
            int length = coefficiants.Length;
            Matrix system = new Matrix(length, length + 1);
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                double result = 0;
                for (int j = 0; j < length; j++)
                {
                    double randVal = rand.NextDouble();
                    system[i, j] = randVal;
                    result += coefficiants[j] * randVal;
                }
                system[i, length] = result;
            }
            return new LinearSystem(system);
        }

        [TestMethod()]
        public void SolveGauss_Test()
        {
            Random rand = new Random();
            // different sizes of linear systems
            for(int n = 2; n <= 5; n++)
            {
                // multiple tests on same size
                for (int k = 0; k < 5; k++)
                {
                    double[] coeff = new double[n];
                    for (int i = 0; i < n; i++)
                    {
                        // subtract 0.5 to ensure usage of negative values
                        // multiply by n+1 to generate larger numbers
                        coeff[i] = (rand.NextDouble() - 0.5) * (n + 1);
                    }
                    LinearSystem system = GenerateLinearSystem(coeff);
                    double[] results = system.SolveGauss(15);

                    for (int i = 0; i < n; i++)
                    {
                        double expected = coeff[i];
                        double actual = results[i];
                        Assert.AreEqual(expected, actual, 0.0001);
                    }
                }
            }
        }
    }
}