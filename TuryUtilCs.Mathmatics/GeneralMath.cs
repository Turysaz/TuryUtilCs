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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuryUtilCs.Mathmatics
{
    // gonna need System.Math here, so I have to override Math
    using System;

    /// <summary>
    /// Some general, everyday math functions
    /// </summary>
    public class GeneralMath
    {
        /// <summary>
        /// Converts an angle from degrees to radiants.
        /// </summary>
        /// <param name="deg">angle in degrees</param>
        /// <returns>angle in radiants</returns>
        public static double DegToRad(double deg)
        {
            return (deg / 360.0) * 2 * System.Math.PI;
        }

        /// <summary>
        /// Converts an angle from radiants to degrees.
        /// </summary>
        /// <param name="rad">angle in radiants</param>
        /// <returns>angle in degrees</returns>
        public static double RadToDeg(double rad)
        {
            return (rad / (2 * System.Math.PI)) * 360;
        }

        /// <summary>
        /// Solves quadratic equations of the format <para/>
        /// x^2 + px + q = 0 <para/>
        /// and returns the two solutions of it if they are real.
        /// </summary>
        /// <param name="p">p</param>
        /// <param name="q">q</param>
        /// <returns>double[2] that contains the solutions of the equation</returns>
        public static double[] QuadraticEquation(double p, double q)
        {
            double[] res = new double[2];
            res[0] = 0.5 * ( -p - Math.Sqrt(p * p - 4 * q));
            res[1] = 0.5 * ( -p + Math.Sqrt(p * p - 4 * q));

            return res;
        }

        // copied from https://dotnet-snippets.de/snippet/fakultaet-einzeiler/11012
        /// <summary>
        /// Calculates the faculty of a number.
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        long Faculty(int n) { return n > 0 ? n * Faculty(n - 1) : 1; }

    }
}
