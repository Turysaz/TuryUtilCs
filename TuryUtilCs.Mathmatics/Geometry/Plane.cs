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
    /// Plane in space, defined by a supporting point and a normal vector
    /// </summary>
    public class Plane
    {

        #region fields
        /// <summary>
        /// Support point of the Plane
        /// </summary>
        protected Vector _point;
        /// <summary>
        /// Plane's normal vector.
        /// </summary>
        protected Vector _normal;
        /// <summary>
        /// Minimal distance between the Plane and origin.
        /// </summary>
        protected double _distanceToOrigin;
        #endregion fields

        #region properties

        /// <summary>
        /// Point, that lies on the plane. 
        /// This point defines the position of the plane.
        /// </summary>
        public Vector Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;

                if (Normal != null && value != null)
                {
                    UpdateDistToOrigin();
                }
            }
        }


        /// <summary>
        /// Normal Vector of the Plane. Defines the orientation. 
        /// Normalized when set!
        /// </summary>
        public Vector Normal
        {
            get { return _normal; }
            set
            {
                _normal = value.Normalize();

                if (Point != null && value != null)
                {
                    UpdateDistToOrigin();
                }
            }
        }

        /// <summary>
        /// Minimal distance between the Plane and Origin.
        /// </summary>
        public double DistanceToOrigin
        {
            // define: if the distance of the plane to the origin
            //  is changed, _point will be moved. it is not always
            //  possible to reach the set distance just by rearranging
            //  the normal vector!

            get { return _distanceToOrigin; }
            set
            {
                Vector auxVector;

                if (value >= 0)
                {
                    // move point along normal
                    auxVector = Vector.MultiplySkalar(_normal,
                        (value - _distanceToOrigin));
                    _point = Vector.Add(_point, auxVector);

                    _distanceToOrigin = value;
                }
                else
                {
                    throw new ArgumentException(
                        "The distance between the plane and origin must not be smaller than 0!");
                }
            }
        }

        #endregion properties

        /// <summary>
        /// Creates a new plane by a given location vector and a normal vector.
        /// </summary>
        /// <param name="point"></param>
        /// <param name="normal"></param>
        public Plane(Vector point, Vector normal)
        {
            Point = point;
            Normal = normal;
        }

        /// <summary>
        /// Defines a Plane by Hesse normal form. Parameter are a normal
        /// vector and the minimal distance to origin. The support Point will
        /// be assumend as the Point with the minimal distance to origin.
        /// </summary>
        /// <param name="normal">Plane's normal Vector</param>
        /// <param name="distToOrigin">Distance to origin</param>
        public Plane(Vector normal, double distToOrigin)
        {
            Normal = normal;
            Point = Vector.MultiplySkalar(Normal, DistanceToOrigin);
        }

        /// <summary>
        ///Returnes a new plane, which is coplanar to the given triangle.
        /// </summary>
        /// <param name="d">A triangle</param>
        /// <returns>coplanar plane</returns>
        public static Plane NewFromTriangle(Triangle d)
        {
            return NewFromThreePoints(d.P1, d.P2, d.P3);
        }

        /// <summary>
        /// Creates a new plane from three Points. 
        /// The support point of the plane will be the
        /// fist given point/argument.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns>plane that touches all three points</returns>
        public static Plane NewFromThreePoints(Point p1, Point p2, Point p3)
        {
            Vector normal = Vector.CrossProduct(new Vector(p1, p2), new Vector(p1, p3));
            return new Plane(p1.ToVector(), normal);
        }

        /// <summary>
        /// calculates a new linear regression plane from a noisy point set
        /// </summary>
        /// <param name="pts">point set</param>
        /// <returns>regression plane</returns>
        public static Plane NewFromRegression(Point[] pts)
        {
            Vector center = Geometry.Point.Center(pts).ToVector();

            // Calculate regression plane by calculations three points
            // lying on the plane and then calculating the normal vector
            // by cross product.
            //
            // Formular:
            // =========
            //
            //      x_i, y_i, z_i ....... x, y, z coordinates of the points in the set
            //      n ................... total amount of points in the set
            //      p1, p2, p3, v1, v2 .. auxiliary Vectors
            //      normal .............. normal vector      
            // 
            //        / sum_i(x_i^2)   sum_i(x_i*y_i)   sum_i(x_i*z_i) \
            //  p1 = |  ------------ ; -------------- ; --------------  |
            //        \  sum_i(x_i)      sum_i(x_i)       sum_i(x_i)   / 
            //
            //
            //        / sum_i(x_i*y_i)   sum_i(y_i^2)   sum_i(y_i*z_i) \
            //  p2 = |  -------------- ; ------------ ; --------------  |
            //        \   sum_i(y_i)      sum_i(y_i)      sum_i(y_i)   /
            //
            //
            //        / sum_i(x_i)   sum_i(y_i)   sum_i(y_i) \
            //  p3 = |  ---------- ; ---------- ; ----------  |
            //        \      n            n            n     /
            //
            //  v1 = p2 - p1
            //  v2 = p3 - p1
            //  normal = (v1 x v2)   

            double SumX = 0,
                SumY = 0,
                SumZ = 0,
                SumXX = 0,
                SumXY = 0,
                SumXZ = 0,
                SumYY = 0,
                SumYZ = 0,
                count = pts.Length;


            foreach (Point p in pts)
            {
                SumX += p.X;
                SumY += p.Y;
                SumZ += p.Z;
                SumXX += p.X * p.X;
                SumYY += p.Y * p.Y;
                SumXY += p.X * p.Y;
                SumXZ += p.X * p.Z;
                SumYZ += p.Y * p.Z;
            }

            Vector p1 = new Vector(SumXX / SumX, SumXY / SumX, SumXZ / SumX);
            Vector p2 = new Vector(SumXY / SumY, SumYY / SumY, SumYZ / SumY);
            Vector p3 = new Vector(SumX / count, SumY / count, SumZ / count);

            Vector v1 = new Vector(p1, p2);
            Vector v2 = new Vector(p1, p3);

            Vector normal = Vector.CrossProduct(v1, v2);

            return new Plane(center, normal);


        }

        /// <summary>
        /// Computes a plane that fits best into a given point set by
        /// principal component analysis.
        /// </summary>
        /// <param name="pts">unorganised point set</param>
        /// <param name="accuracy">
        /// Amount of correctly computed decimals when computing the
        /// eigenvalues of the correlation matrix
        /// </param>
        /// <param name="eigenvalueScalingFactor">
        /// Matrix scaling factor. Used by the numeric eigenvalue
        /// computation algorithm. The computation might be faster,
        /// but the algorithm might be unstable if this value is
        /// not 1.
        /// </param>
        /// <returns>best fitting Plane</returns>
        public static Plane NewFromPrincipalComponentAnalysis(Point[] pts, int accuracy, int eigenvalueScalingFactor)
        {
            Cuboid cub = Cuboid.BoundingBoxFromPCA(pts, accuracy, eigenvalueScalingFactor);
            return cub.MainPlanes()[2];
        }
        /// <summary>
        /// Recalculates the distance between the Plane and origin. This
        /// method is used when the Plane itself is changed.
        /// </summary>
        protected void UpdateDistToOrigin()
        {
            _distanceToOrigin = Vector.ScalarProduct(Normal, Point);
        }

        /// <summary>
        /// Returnes the minimal distance between this plane and a given point.
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>minimal distance to the plane</returns>
        public double DistToPoint(Point p)
        {
            return p.DistanceToPlane(this);
        }

        /// <summary>
        /// Calculates two orthogonal vectors, which both are orthogonal
        /// to the normal vector of the plane. Both vectors thus are parallel
        /// to the plane. Im combination with the plane's normal vector, the
        /// new vectors form a new linear system.
        /// </summary>
        /// <returns>{v1, v2}</returns>
        public Vector[] OrthogonalParallelVectors()
        {
            Vector n = Normal;
            Vector v1, v2;

            if (n.X == 0)
            {
                v1 = new Vector(1, 0, 0);
            }
            else if (n.Y == 0)
            {
                v1 = new Vector(0, 1, 0);
            }
            else if (n.Z == 0)
            {
                v1 = new Vector(0, 0, 1);
            }
            else
            {
                double z = - (n.X + n.Y) / n.Z;
                v1 = new Vector(1, 1, z);
            }
            v2 = Vector.CrossProduct(n, v1);
            return new Vector[] { v1, v2 };
        }

        /// <summary>
        /// Checks, if this plane is parallel to a given second plane.
        /// </summary>
        /// <param name="p">second plane</param>
        /// <returns>true, if both planes are parallel</returns>
        public bool Parallel(Plane p)
        {
            if (Vector.CrossProduct(this.Normal, p.Normal).Length() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks, if this plane is parallel to a given line.
        /// </summary>
        /// <param name="l">line</param>
        /// <returns>true, if parallel</returns>
        public bool Parallel(Line l)
        {
            if (Vector.ScalarProduct(this.Normal, l.Direction) == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the intersection line of this plane and a given second one.
        /// </summary>
        /// <param name="p">second plane</param>
        /// <returns>Intersection line</returns>
        public Line Intersection(Plane p)
        {
            if (this.Parallel(p))
            {
                throw new Exception("There is no intersection! The planes are parallel!");
            }
            throw new NotImplementedException();
            //UNDONE Schnittgerade Ebenen
        }


        /// <summary>
        /// Checks, if this plane is identical to another given plane.
        /// </summary>
        /// <param name="p">other plane</param>
        /// <returns>True, if identical</returns>
        public bool Identical(Plane p)
        {
            if (!this.Parallel(p))
            {
                return false;
            }

            if (this.DistanceToOrigin == p.DistanceToOrigin)
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// Returns a string describing the plane by it's point and it's normal.
        /// </summary>
        /// <returns>string describing the plane</returns>
        public override string ToString()
        {
            return ("Plane {Point: " + Point.ToString() +
                ", Normal: " + Normal.ToString() + "}");
        }
    }
}
