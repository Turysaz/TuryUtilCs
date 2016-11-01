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

namespace TuryUtilCs.Mathmatics.Geometry
{
    /// <summary>
    /// A point in space.
    /// </summary>
    public class Point
    {
        public static bool PRINT_POINTS_AS_HASH = false;

        //DOKU
        private double[] _coordinates = new double[3];

        /// <summary>
        /// x-coordinate
        /// </summary>
        public double X
        {
            get { return _coordinates[0]; }
            set { _coordinates[0] = value; }
        }

        /// <summary>
        /// y-coordinate
        /// </summary>
        public double Y
        {
            get { return _coordinates[1]; }
            set { _coordinates[1] = value; }
        }


        /// <summary>
        /// z-coordinate
        /// </summary>
        public double Z
        {
            get { return _coordinates[2]; }
            set { _coordinates[2] = value; }
        }


        //DOKU
        public double this[int i]
        {
            get { return _coordinates[i]; }
            set { _coordinates[i] = value; }
        }


        /// <summary>
        /// Creates a new point by it's coordinates
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        public Point(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Returns the centroid of a point set.
        /// </summary>
        /// <param name="pts">Point set</param>
        /// <returns>centroid</returns>
        public static Point Center(Point[] pts)
        {
            double xSum = 0,
                ySum = 0,
                zSum = 0;

            foreach (Point p in pts)
            {
                xSum += p.X;
                ySum += p.Y;
                zSum += p.Z;
            }
            return new Point(xSum / pts.Length,
                ySum / pts.Length,
                zSum / pts.Length);
        }

        /// <summary>
        /// Calculates the center of two points.
        /// </summary>
        /// <param name="p1">first point</param>
        /// <param name="p2">second point</param>
        /// <returns>center of p1 and p2</returns>
        public static Point Center(Point p1, Point p2)
        {
            return new Point((p1.X + p2.X) / 2,
                (p1.Y + p2.Y) / 2,
                (p1.Z + p2.Z) / 2);
        }

        /// <summary>
        /// Returns the distance between this point and another one.
        /// </summary>
        /// <param name="p2">other point</param>
        /// <returns>distance between the points</returns>
        public double DistanceTo(Point p2)
        {
            return Math.Sqrt(SquareDistanceTo(p2));
        }

        /// <summary>
        /// Returns the squared distance between this point and another one.
        /// </summary>
        /// <param name="p2">other point</param>
        /// <returns>squared distance between the points</returns>
        public double SquareDistanceTo(Point p2)
        {
            return Math.Pow((this.X - p2.X), 2) +
                Math.Pow((this.Y - p2.Y), 2) +
                Math.Pow((this.Z - p2.Z), 2);

        }

        /// <summary>
        /// Returns the minimal distance between the point and a given line.
        /// </summary>
        /// <param name="l">line</param>
        /// <returns>minimal distance</returns>
        public double DistanceToLine(Line l)
        {
            Vector p = this.ToVector(),
                a = l.Point,
                b = l.Direction;

            double numerator, denominator;

            numerator = Vector.CrossProduct(b, Vector.Add(p, Vector.MultiplySkalar(a, -1))).Length();
            denominator = b.Length();

            return numerator / denominator;


        }


        /// <summary>
        /// Returns the minimal distance between the point and a given plane.
        /// TODO point distance to plane: bugs possible. See below. DOKU update after fix
        /// <b>Attention:</b> the distance can be smaller than 0. In Hesse normal
        /// form, that means that the point lies between origin and the plane.
        /// <b>But</b> in Hesse normal form, the normal always points away from origin.
        /// This is not necessarily the case with this framework (yet).
        /// </summary>
        /// <param name="p">plane</param>
        /// <returns>minimal distance</returns>
        public double DistanceToPlane(Plane p)
        {
            // Hesse normal form: 
            //---------------------
            // d(p,E) = p * n - d
            //
            // with
            // E - Plane
            // p - Point
            // n - plane's normal vector
            // d - plane's minimal distance to origin 

            Vector q = this.ToVector();

            return (Vector.ScalarProduct(q, p.Normal) - p.DistanceToOrigin);
        }

        /// <summary>
        /// Returnes the coordinates of the point as an array
        /// </summary>
        /// <returns>coordinates of the point: {x,y,z} </returns>
        [Obsolete]
        public double[] Coordinates()
        {
            return new double[] { X, Y, Z };
        }

        public bool Equals(Point p)
        {
            if (this.X == p.X &&
                this.Y == p.Y &&
                this.Z == p.Z)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the projection of the Point to a line.
        /// </summary>
        /// <param name="l">The line on which the point shall
        /// be projected</param>
        /// <returns>Projection of <b>this</b>.</returns>
        public Point ProjectionToLine(Line l)
        {
            // with p as this and u = l.direction and r = l.point:
            //  (*) are scalar products, (x) are multiplications
            //----------------------------------------------
            //  P_p = (((p-r)*u)/(u*u) x u) + r
            //      = (numerator/denominator) x u + r
            //      = lambda * u + r
            double numerator, denominator, lambda;

            numerator = Vector.ScalarProduct(
                Vector.Add(this.ToVector(),
                    Vector.MultiplySkalar(l.Point, -1)),
                l.Direction);
            denominator = Vector.ScalarProduct(l.Direction, l.Direction);
            lambda = numerator / denominator;

            return Vector.Add(Vector.MultiplySkalar(l.Direction, lambda), l.Point).ToPoint();
        }

        /// <summary>
        /// Returns the projection of this point to a given plane in space
        /// </summary>
        /// <param name="plane">plane</param>
        /// <returns>projection : Point</returns>
        public Point ProjectionToPlane(Plane plane)
        {
            // u and v are two orthogonal vectors lying on the plane
            //  (parallel to the plane)
            // p is the point that shall be projected onto plane, while
            // r0 is the support point of the plane
            //
            // the projection then is
            //
            //              <p-r0;u>       <p-r0;v>
            // P = r0 + u * -------- + v * --------       ; <x;y> = scalar product
            //               <u;u>          <v;v>
            //   
            //   = r0 + u * fac1  + v * fac2
            //
            //   = r0 + sum1  +  sum2

            #region declarations
            Vector[] vTang; // auxiliary vector array
            Vector u;       // vector parrallel to plane, orthogonal to v
            Vector v;       // vector parrallel to plane, orthogonal to u
            Vector r;       // support point of the plane
            Vector p;       // the point p transformed into a vector

            //auxiliary values
            double fac1, fac2;
            Vector sum1, sum2;
            Vector projection;
            #endregion

            #region init
            vTang = plane.OrthogonalParallelVectors();
            u = vTang[0];
            v = vTang[1];
            r = plane.Point;
            p = this.ToVector();
            #endregion

            fac1 = Vector.ScalarProduct(Vector.Subtract(p, r), u) / Vector.ScalarProduct(u, u);
            fac2 = Vector.ScalarProduct(Vector.Subtract(p, r), v) / Vector.ScalarProduct(v, v);
            sum1 = Vector.MultiplySkalar(u, fac1);
            sum2 = Vector.MultiplySkalar(v, fac2);

            projection = Vector.Add(sum1, sum2);
            projection = Vector.Add(r, projection);
            return projection.ToPoint();
        }

        /// <summary>
        /// Returns the point transformed into a vector.
        /// </summary>
        /// <returns>Transformation of the point.</returns>
        public Vector ToVector()
        {
            return new Vector(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns a string describing the point by it's coordinates
        /// </summary>
        /// <returns>"x; y; z"</returns>
        public override string ToString()
        {
            if (PRINT_POINTS_AS_HASH)
            {
                return ("P" + (int)(X * 1000 + Y * 100 + Z * 10) % 659);
            }
            else
            {
                return ("Point: (" + Math.Round(X, 2) + "; " +
                    Math.Round(Y, 2) + "; " +
                    Math.Round(Z, 2) + ")");
            }
        }
    }
}
