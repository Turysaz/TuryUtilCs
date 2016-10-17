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
    public class Polynomial_Tests
    {
        [TestMethod()]
        public void Polynomial_Test_CreateByCoefficiantList()
        {
            double[] coeff;
            Polynomial p;

            coeff = new double[] { 1 };
            p = new Polynomial(coeff);

            Assert.AreEqual(0, p.Degree);

            coeff = new double[] { -2, 5, 6 };
            p = new Polynomial(coeff);

            Assert.AreEqual(2, p.Degree);

            coeff = new double[] { 0, 3, -1, 0.5 };
            p = new Polynomial(coeff);

            Assert.AreEqual(3, p.Degree);
        }

        [TestMethod()]
        public void Polynomial_Test_CreateByDegree()
        {
            Polynomial p = new Polynomial(3);
            Assert.AreEqual(3, p.Degree);
            for(int i = 0; i< p.Degree; i++)  
            {
                Assert.AreEqual(0, p[i]);
            }
        }

        [TestMethod()]
        public void Value_Test()
        {
            double[] coeff = new double[] { -3, 1, 2 };
            Polynomial p = new Polynomial(coeff);

            double[] sampleX = new double[] { -3, 0, 2, 6.4 };
            double[] sampleY = new double[] { 12, -3, 7, 85.32 };

            for(int i = 0; i<sampleX.Length; i++)
            {
                double expected = sampleY[i];
                double actual = p.Value(sampleX[i]);
                Assert.AreEqual(expected, actual, 0.0000000001);
            }
            
        }

        [TestMethod()]
        public void Derivation_Test_Value()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void Derivation_Test_Function()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ZeroSekant_Test()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ZeroHalley_Test()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ZeroSetN2_Test()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void ZeroSetN3_Test()
        {
            throw new NotImplementedException();
        }
    }
}