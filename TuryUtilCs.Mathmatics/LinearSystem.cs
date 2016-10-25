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
    using System;

    /// <summary>
    /// Implements algorithm(s) for solving linear equation systems.
    /// </summary>
    public class LinearSystem
    {
        /// <summary>
        /// Matrix representing the linear equation system. See documentation
        /// of property <c>Values</c> for detailed information.
        /// </summary>
        private Matrix _values;

        /// <summary>
        /// A Matrix of the size (n x n+1) that represents
        /// the linear equation system.
        /// The entries (0,0) to (n-1, n-1) contain the coefficients
        /// of the equations, while the last column contains the solution.
        /// <example>
        /// <code>
        /// The equations
        ///     
        ///     3a + 4b = 7
        ///     7a + 2b = 3
        /// 
        /// shall be represented by the matrix
        /// 
        ///     ( 3  4  7)
        ///     ( 7  2  3)
        ///
        /// </code>
        /// </example>
        /// </summary>
        public Matrix Values
        {
            get
            {
                return _values;
            }
            set
            {
                if (value.Rows != value.Columns - 1)
                {
                    throw new ArgumentException(
                        "The linear system matrix must be of the dimensions nx(n+1)!");
                }
                _values = value;
            }
        }

        /// <summary>
        /// Creates a new linar system from a matrix.
        /// </summary>
        /// <param name="system">linear system in matrix representation</param>
        public LinearSystem(Matrix system)
        {
            Values = system;
        }

        /// <summary>
        /// Solves the linear equation system using the Gaussion
        /// elimination method.
        /// </summary>
        /// <returns>
        /// Solutions for the variables of the linear equation
        /// system in the order they appear in the system.
        /// <example>
        /// The solution of the system:
        /// <code>
        ///     ( 3 2 7 )
        ///     ( 2 1 4 )
        /// </code>
        /// will return the solution
        /// <code>
        /// [1,2]
        /// </code>
        /// .
        /// </example>
        /// <param name="accuracy">
        /// Index of the decimal that numbers (zeroes)
        /// will be rounded to.
        /// </param>
        /// </returns>
        public double[] SolveGauss(int accuracy)
        {
            //helpful to save temporarily
            int lastCol = Values.Columns - 1;

            //Console.WriteLine(Values.ToString());

            // 1st step: triangle matrix
            for (int i = 0; i < Values.Rows; i++)
            {
                double upperLineValue = Values[i, i];
                for (int j = i + 1; j < Values.Rows; j++)
                {
                    double lowerLineValue = Values[j, i];
                    if (lowerLineValue == 0) { continue; }
                    for (int k = i; k < Values.Columns; k++)
                    {
                        Values[j, k] = Values[i, k] * lowerLineValue - Values[j, k] * upperLineValue;
                    }
                    //Console.WriteLine(Values.ToString());
                }
            }

            //2nd step: sort
            for (int i = 0; i < Values.Rows; i++)
            {
                for (int j = 0; j < Values.Columns; j++)
                {
                    if (Values[i, j] != 0)
                    {
                        if (i != j)
                        {
                            Values.SwapRows(i, i + 1);
                            //Console.WriteLine("swap lines.\n" + Values.ToString());
                        }
                        break;
                    }
                }
            }

            //3rd step: check amount of solutions
            for (int i = 0; i < Values.Rows; i++)
            {
                // have to round, zeroes would not be found otherwise...
                // special case: if this is used to find eigenvectors,
                // the accuray needs to be round to the same accuracy that
                // has been set to find the eigenvalues!
                Values[i, i] = Math.Round(Values[i, i], accuracy);
                if (Values[i, i] != 0) { continue; }
                if (Values[i, lastCol] != 0)
                {
                    // due to the upper two lines, this case occurs when,
                    // for example one line of the system looks like this:
                    // ( 0  0  0 | 3 )
                    // this is not solvable, while a line like
                    // ( 0  0  0 | 0 )
                    // would be.
                    throw new Exception("Linear system not solvable!\n" + Values.ToString());
                }
                //Console.WriteLine("Infinite amount of solutions. Choosing default = 1.");
                Values[i, i] = 1;
                Values[i, lastCol] = 1;
                //Console.WriteLine(Values.ToString());
            }

            //4th step: diagonalize matrix
            for (int i = 0; i < Values.Rows; i++)
            {

                for (int j = i + 1; j < Values.Rows; j++)
                {
                    double upperLineValue = Values[i, j];
                    double lowerLineValue = Values[j, j];
                    for (int k = i; k < Values.Columns; k++)
                    {
                        Values[i, k] = Values[i, k] * lowerLineValue - Values[j, k] * upperLineValue;
                    }
                    //Console.WriteLine(Values.ToString());
                }
            }


            //5th step: transform to identity matrix
            for (int i = 0; i < Values.Rows; i++)
            {
                Values[i, lastCol] /= Values[i, i];
                Values[i, i] = 1;
            }
            //Console.WriteLine(Values.ToString());



            //6th step: extract solution
            double[] result = new double[Values.Rows];
            for (int i = 0; i < Values.Rows; i++)
            {
                result[i] = Values[i, lastCol];
            }

            return result;
        }
    }
}
