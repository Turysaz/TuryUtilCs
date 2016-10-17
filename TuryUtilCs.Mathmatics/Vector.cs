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

using TuryUtilCs.Mathmatics.Geometry;


namespace TuryUtilCs.Mathmatics
{
    /// <summary>
    /// Vector for describing 3D-Positions and directions
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Components of the vector
        /// </summary>
        private double[] _components = new double[3];

        /// <summary>
        /// first component
        /// </summary>
        public double X
        {
            get { return _components[0]; }
            set { _components[0] = value; }
        }

        /// <summary>
        /// second component
        /// </summary>
        public double Y
        {
            get { return _components[1]; }
            set { _components[1] = value; }
        }

        /// <summary>
        /// third component
        /// </summary>
        public double Z
        {
            get { return _components[2]; }
            set { _components[2] = value; }
        }

        /// <summary>
        /// Get specific component.
        /// </summary>
        /// <param name="i">index of the component</param>
        /// <returns>component with index i</returns>
        public double this[int i]
        {
            get { return _components[i]; }
            set { _components[i] = value; }
        }

        /// <summary>
        /// creates a new vector
        /// </summary>
        /// <param name="x">first component</param>
        /// <param name="y">second component</param>
        /// <param name="z">third component</param>
        public Vector(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Creates a new vector from one point to another.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Vector(Vector from, Vector to)
        {
            this.X = to.X - from.X;
            this.Y = to.Y - from.Y;
            this.Z = to.Z - from.Z;
        }

        /// <summary>
        /// Creates a new vector from one point to another.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Vector(Point from, Point to)
        {
            this.X = to.X - from.X;
            this.Y = to.Y - from.Y;
            this.Z = to.Z - from.Z;
        }

        /// <summary>
        /// Returnes a new vector, which is colinear to the given vector, but
        /// multiplied with s
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="s">scalar to multiply the vector with</param>
        /// <returns></returns>
        public static Vector MultiplySkalar(Vector v, double s)
        {
            return new Vector(v.X * s,
                v.Y * s,
                v.Z * s);
        }

        /// <summary>
        /// Multiplies the vector by a scalar.
        /// <ul>ATTENTION:</ul> The original vector will be changed!
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vector MultiplySkalar(double s)
        {
            X *= s;
            Y *= s;
            Z *= s;

            return this;
        }

        /// <summary>
        /// calculates the cross product of two vectors
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>cross product</returns>
        public static Vector CrossProduct(Vector v1, Vector v2)
        {
            double nx = v1.Y * v2.Z - v1.Z * v2.Y;
            double ny = v1.Z * v2.X - v1.X * v2.Z;
            double nz = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector(nx, ny, nz);
        }

        /// <summary>
        /// calculates the scalar product of two vectors
        /// </summary>
        /// <param name="v1">first vector</param>
        /// <param name="v2">second vector</param>
        /// <returns>scalar product</returns>
        public static double ScalarProduct(Vector v1, Vector v2)
        {
            return (v1.X * v2.X +
                v1.Y * v2.Y +
                v1.Z * v2.Z);
        }

        /// <summary>
        /// Addition of two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>sum of the vectors</returns>
        public static Vector Add(Vector v1, Vector v2)
        {
            return new Vector(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector Subtract(Vector v1, Vector v2)
        {
            return Vector.Add(v1, Vector.MultiplySkalar(v2, -1));
        }

        /// <summary>
        /// calculates the cosinus of the angle between both vectors.
        /// Faster than calculating the angle itself.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>cos(angle)</returns>
        public static double CosAngleBetween(Vector v1, Vector v2)
        {
            double cos = Vector.ScalarProduct(v1, v2) /
                (v1.Length() * v2.Length());
            return cos;
        }

        /// <summary>
        /// calculates the angle between two vectors in RAD.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns>angle in rad</returns>
        public static double AngleBetween(Vector v1, Vector v2)
        {
            //ATTENTION! RADIAN MEASURE!

            // TODO: cos and acos 
            // There is a weird bug:
            // When the angle between two vectors is 180°, the cos
            // is - no surprise - (-1).
            // According to MSDN, Math.Acos returns NaN for values not
            // whithin [-1;1].
            // So, a cos of -1 should work. I still got NaN on each try!
            // I think this migth be a problem of accuracy, but i don't know
            // how to fix this best. still need to fix it, because this
            // this will certainly cause more problems!

            return System.Math.Acos(Vector.CosAngleBetween(v1, v2));
        }

        /// <summary>
        /// returns NEW vector with the length 1.
        /// </summary>
        /// <returns></returns>
        public Vector Normalize()
        {
            double abs = this.Length();
            return new Vector(this.X / abs, this.Y / abs, this.Z / abs);
        }

        /// <summary>
        /// Caculates the squared lenght of the vector
        /// </summary>
        /// <returns>length ^2</returns>
        public double SquareLenght()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>length</returns>
        public double Length()
        {
            return System.Math.Sqrt(this.SquareLenght());
        }



        /// <summary>
        /// returns the vector transformed into a point
        /// </summary>
        /// <returns></returns>
        public Point ToPoint()
        {
            return new Point(this.X, this.Y, this.Z);
        }

        /// <summary>
        /// Returns a string describing the vector by it's components.
        /// </summary>
        /// <returns>string describing the vector</returns>
        public override string ToString()
        {
            return ("(" + X + "; " + Y + "; " + Z + ")");
        }

        /// <summary>
        /// Rotates the vector around an specific axis.
        /// </summary>
        /// <param name="axis">The axis around which the vector shall be rotated</param>
        /// <param name="angle">The angle (rad) of the rotation</param>
        /// <returns>rotated vector</returns>
        public Vector Rotate(Vector axis, double angle)
        {
            // ATTENTION: RADIAN MEASURE
            
            //TODO rotieren prüfen
            Matrix rotationMatrix = new Matrix(3, 3);
            axis = axis.Normalize();

            double c = 1 - System.Math.Cos(angle);
            
            //Erste Zeile
            rotationMatrix[0, 0] = axis.X * axis.X * c + System.Math.Cos(angle);
            rotationMatrix[0, 1] = axis.X * axis.Y * c - axis.Z * System.Math.Sin(angle);
            rotationMatrix[0, 2] = axis.X * axis.Z * c + axis.Y * System.Math.Sin(angle);

            //Zweite Zeile
            rotationMatrix[1, 0] = axis.Y * axis.X * c + axis.Z * System.Math.Sin(angle);
            rotationMatrix[1, 1] = axis.Y * axis.Y * c + System.Math.Cos(angle);
            rotationMatrix[1, 2] = axis.Y * axis.Z * c - axis.X * System.Math.Sin(angle);

            //Dritte Zeile
            rotationMatrix[2, 0] = axis.Z * axis.X * c - axis.Y * System.Math.Sin(angle);
            rotationMatrix[2, 1] = axis.Z * axis.Y * c + axis.X * System.Math.Sin(angle);
            rotationMatrix[2, 2] = axis.Z * axis.Z * c + System.Math.Cos(angle);

            return Matrix.Multiply(rotationMatrix, this).ToVector();
        }


        /// <summary>
        /// Rotates the vector around an axis that is parallel to a given axis (that goes through
        /// (0,0,0) )and goes through a point.
        /// </summary>
        /// <param name="axis">unit vector of the given axis</param>
        /// <param name="angle">The angle of the rotation</param>
        /// <param name="point">the point around which the rotation shall be done.</param>
        /// <returns></returns>
        public Vector Rotate(Vector axis, double angle, Vector point)
        {
            return Add(point, Subtract(this, point).Rotate(axis, angle));
        }
    }
}
