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
    using System; // needed because of Math namespace

    // Represents a polynomial equation by it's coefficiants.
    public class Polynomial
    {
        /// <summary>
        /// The coefficiants of the Polynomial.
        /// See property <c>this[int i]</c> for detailed information.
        /// </summary>
        private double[] _coefficients;

        /// <summary>
        /// The coefficiants of the polynomial.
        /// The index i of a coefficiant is equal to the power
        /// of the variable.
        /// So, the indices of the coefficiants of the equation 
        /// 
        /// f(x) = ax^2 + bx + c
        /// 
        /// are:
        ///     
        ///     a: 2,
        ///     b: 1,
        ///     c: 0.
        ///     
        /// </summary>
        /// <param name="i">index of the coefficiant</param>
        /// <returns>coefficiant with the index i</returns>
        public double this[int i]
        {
            get { return _coefficients[i]; }
            set { _coefficients[i] = value; }
        }

        /// <summary>
        /// The highest power to the variable in the equation.
        /// </summary>
        public int Degree
        {
            get { return _coefficients.Length - 1; }
        }

        /// <summary>
        /// Creates a Polynomial object with specified coefficiants.
        /// The degree of the polynomial will be set to the smallest
        /// possible.
        /// </summary>
        /// <param name="coefficiants">coefficiants of the Polynomial</param>
        public Polynomial(double[] coefficiants)
        {
            _coefficients = coefficiants;
        }

        /// <summary>
        /// Create a Polynomial with a specified degree.
        /// The coefficiants will all be set to zero.
        /// You need to set them to the desired values afterwards.
        /// </summary>
        /// <param name="degree"></param>
        public Polynomial(int degree)
        {
            _coefficients = new double[degree + 1];
        }

        /// <summary>
        /// Computes the result of the Polynomial at a specified
        /// parameter value.
        /// </summary>
        /// <param name="parameter">
        /// parameter value for the variable of
        /// the polynomial
        /// </param>
        /// <returns>
        /// Result of the equation at the specified position
        /// </returns>
        public double Value(double parameter)
        {
            double subtotal = 0;

            for (int i = 0; i <= Degree; i++)
            {
                subtotal += Math.Pow(parameter, i) * this[i];
            }
            return subtotal;
        }


        /// <summary>
        /// Calculates the derivation of the Polynomial at a
        /// specified, single position.
        /// </summary>
        /// <param name="derivDegree">
        /// "First derivation, second derivation, ..."?
        /// </param>
        /// <param name="x">position</param>
        /// <returns>[degree]th derivation at the given position</returns>
        public double Derivation(int derivDegree, double x)
        {
            if(derivDegree > Degree) { return 0; }

            double subtotal = 0;
            double derivFactor;

            for (int i = derivDegree; i <= Degree; i++)
            {
                derivFactor = i;
                for(int j = 1; j < derivDegree; j++)
                {
                    derivFactor *= i - j;
                }
                subtotal += this[i] * derivFactor *
                    Math.Pow(x, i - derivDegree);
            }

            return subtotal;
        }

        /// <summary>
        /// Computes the derivation of the polynomial.
        /// </summary>
        /// <param name="derivDegree">
        /// "First derivation, second derivation, ..."?
        /// </param>
        /// <returns>[degree]th derivation of the polynomial</returns>
        public Polynomial Derivation(int derivDegree)
        {
            Polynomial derivation = new Polynomial(Degree - derivDegree);

            double derivFactor;

            for(int i = 0; i <= derivation.Degree; i++)
            {
                derivFactor = i+1;
                for (int j = 1; j < derivDegree; j++)
                {
                    derivFactor *= i+j+1;
                }
                derivation[i] = this[i + derivDegree] * derivFactor;
            }
            return derivation;
        }

        /// <summary>
        /// Finds one zero position of the equation between the left
        /// and the right position by using the sekant method.
        ///
        /// If the signs of the values of the
        /// Polynomial at both positions is equal, this method might
        /// either find a zero position out of the given boundaries or
        /// no one. (Instable in this case!)
        /// The accuracy of the result will be set to 5 decimals (default).
        /// </summary>
        /// <param name="left">left boundary position</param>
        /// <param name="right">right boundary position</param>
        /// <returns>position of the zero / "root"</returns>
        public double ZeroSekant(double left, double right)
        {
            return ZeroSekant(left, right, 5);
        }

        /// <summary>
        /// Finds one zero position of the equation between the left
        /// and the right position by using the sekant method.
        /// If the signs of the values of the
        ///
        /// Polynomial at both positions is equal, this method might
        /// either find a zero position out of the given boundaries or
        /// no one. (Instable in this case!)
        /// </summary>
        /// <param name="left">left boundary position</param>
        /// <param name="right">right boundary position</param>
        /// <param name="accuracy">
        /// Amount of correctly computed decimals.
        /// </param>
        /// <returns>position of the zero / "root"</returns>
        public double ZeroSekant(double left, double right, int accuracy)
        {
            double allowedError = Math.Pow(10, -accuracy);

            // values of the equation at both boundary positions
            double valLeft = Value(left);
            double valRight = Value(right);

            // cannot find a zero position if the connecting line
            // between both points is horizontal!
            if(valLeft == valRight) {
                throw new Exception(
                    "The function values at both initial positions need to be different!");
            }

            // first assumption about the position of the zero.
            double predicted = left - valLeft * (right - left) / (valRight - valLeft);
            double valPredicted = Value(predicted);

            int iterations = 1;
            // dont get stuck here forever!
            int maxIterations = (int)Math.Pow(10, accuracy);

            // do as long as the value of the predicted point is not 
            // close enough to zero
            while (System.Math.Abs(valPredicted) > allowedError)
            {
                iterations++;

                // like I said: don't geht stuck here forever!
                if(iterations == maxIterations)
                {
                    throw new Exception("Too many iterations!");
                }

                // choose which side to alter
                if (Math.Abs(valLeft) > Math.Abs(valRight))
                {
                    left = predicted;
                    valLeft = valPredicted;

                }
                else
                {
                    right = predicted;
                    valRight = valPredicted;
                }


                // If the connecting line between both points (left, right)
                // gets horizontal but the value of the predicted zero is still
                // not close enough to zero, the algorithm got stuck at a local
                // extremum.
                if (valLeft == valRight)
                {
                    throw new Exception(
                        "Stuck at local extremum!");
                }

                // do new assumption about the predicted zero position.
                predicted = left - valLeft * (right - left) / (valRight - valLeft);
                valPredicted = Value(predicted);
            }

            return predicted;

        }

        /// <summary>
        /// Searches a zero position by using the Halley method.
        /// Way faster than the sekant zero search, but it is harder
        /// to determine which zero position will be found.
        /// </summary>
        /// <param name="init">initial position for the search</param>
        /// <param name="accuracy">
        /// Amount of correctly computed decimals
        /// </param>
        /// <returns>A zero position near the initial position.</returns>
        public double ZeroHalley(double init, int accuracy)
        {
            //
            //      HALLEY METHOD ( iterative )
            //
            //                          2 * f(x_k) * f'(x_k) 
            //     x_k+1 = x_k -  ---------------------------------
            //                    2 * f'(x_k)^2 - f(x_k) * f''(x_k)
            //


            double allowedError = Math.Pow(10, -accuracy);

            double predicted = init;
            double valPredicted = Value(predicted);

            double
                numerator,
                denominator,
                firDerivPredicted,
                secDerivPredicted;

            int iterations = 0;
            while (Math.Abs(valPredicted) > allowedError)
            {
                iterations++;
                firDerivPredicted = Derivation(1,predicted);
                secDerivPredicted = Derivation(2,predicted);
                numerator = 2 * valPredicted * firDerivPredicted;
                denominator = 2 * Math.Pow(firDerivPredicted, 2) - valPredicted * secDerivPredicted;
                predicted -= (numerator / denominator);
                valPredicted = Value(predicted);
            }
            return predicted;
            
        }


        /// <summary>
        /// Only for polynomials of the degree 2!
        /// Returns both zero positions of the polynomial,
        /// if they are real.
        /// </summary>
        /// <returns>both zero positions of the polynomial</returns>
        public double[] ZeroSetN2()
        {
            if(Degree != 2)
            {
                throw new Exception(
                    "Only for polynomials with degree 2!");
            }
            if (this[2] == 0)
            {
                throw new Exception(
                    "Not a quadratic function, first coefficient = 0!");
            }

            //normalize
            Polynomial nForm;
            if (this[2] != 1)
            {
                nForm = new Polynomial(3);
                nForm[0] = this[0] / this[2];
                nForm[1] = this[1] / this[2];
                nForm[2] = 1;
            }
            else
            {
                nForm = this; // normal form
            }

            return GeneralMath.QuadraticEquation(nForm[1], nForm[0]);
        }

        /// <summary>
        /// Only for polynomials of the degree 3!
        /// Finds all zero positions of the polynomial, if they
        /// are real.
        /// This method searches one zero position by using
        /// the Halley method. The polynomial then will be divided
        /// by polynomial division by the first found zero position.
        /// The remaining two zero positions will be found by
        /// solving the remaining quadratic equation.
        /// </summary>
        /// <param name="accuracy">
        /// Number of correctly computed decimals
        /// </param>
        /// <returns>
        /// List of all three zero positions.
        /// </returns>
        public double[] ZeroSetN3(int accuracy)
        {
            double allowedError = Math.Pow(10, -accuracy);

            if (Degree != 3)
            {
                throw new Exception("Only for polynomials with degree 3!");
            }

            if (this[3] == 0)
            {
                throw new Exception("Not a cubic function, first coefficient = 0!");
            }

            // normalise term
            Polynomial nForm;
            if (this[3] != 1)
            {
                nForm = new Polynomial(3);
                nForm[3] = 1;
                for (int i = 0; i < 3; i++)
                {
                    nForm[i] = this[i] / this[3];
                }
            }
            else
            {
                nForm = this; // normal form
            }

            // check if all zeros are real
            double reducedP, reducedQ;
            reducedP = nForm[1] - (Math.Pow(nForm[2], 2) / 3);
            reducedQ = 2 * Math.Pow(nForm[2], 3) / 27 -
                nForm[2] * nForm[1] / 3 +
                nForm[0];
            if (Math.Pow(reducedQ / 2, 2) + Math.Pow(reducedP / 3, 3) > 0)
            {
                throw new Exception("not all zeroes are real!");
            }

            //
            // Implementation of the Method of Deiters and Macias-Salinas
            //
            double
                lowerBoundry,
                upperBoundry,
                initialValue,
                firstZero = 0,
                xInflection,
                yInflection,
                auxD; // "D" in Wikipedia: "Kubische Gleichungen"

            xInflection = -nForm[2] / 3;
            yInflection = Value(xInflection);
            if (yInflection == 0)
            {
                firstZero = xInflection;
            }
            else
            {
                auxD = Math.Pow(nForm[2], 2) - 3 * nForm[1];

                if (auxD == 0)
                {
                    firstZero = xInflection - Math.Pow(yInflection, 1.0 / 3);
                }
                else
                {
                    // original: find first zero iterative by halley method, now tangent

                    lowerBoundry = xInflection - (2.0 / 3) * Math.Sqrt(Math.Abs(auxD));
                    upperBoundry = xInflection + (2.0 / 3) * Math.Sqrt(Math.Abs(auxD));

                    if(auxD > 0 && yInflection > 0)
                    {
                        initialValue = lowerBoundry;
                    }else if(auxD > 0 && yInflection < 0)
                    {
                        initialValue = upperBoundry;
                    }else
                    {
                        initialValue = xInflection;
                    }

                    firstZero = ZeroHalley(initialValue, accuracy);
                }
            }

            // polynomial division
            double p, q;
            double[]
                sqret = new double[2],
                ret = new double[3];
            p = nForm[2] + firstZero;
            q = p * firstZero + nForm[1];
            ret[0] = firstZero;
            
            // find remaining zero positions by solving quadratic equation
            sqret = GeneralMath.QuadraticEquation(p, q);
            ret[1] = sqret[0];
            ret[2] = sqret[1];

            return ret;
        }

        /// <summary>
        /// Turns the polynomial into a string (human readable).
        /// </summary>
        /// <returns>String showing the polynomial</returns>
        public override string ToString()
        {
            string ret = "";
            for (int i = Degree; i>= 0; i--)
            {
                if(this[i] == 0) { continue; }
                if (i < Degree)
                {
                    ret += " + ";
                }
                ret += this[i];
                if (i > 0) {
                    ret += "x^" + i;
                }

            }
            return ret;
        }
    }
}
