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
using System.Drawing;

//using System.Windows.Media;

namespace TuryUtilCs.Mathmatics.Geometry
{
    /// <summary>
    /// A triagle of three points in space.
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// First point
        /// </summary>
        public Point P1 { get; set; }

        /// <summary>
        /// Second point
        /// </summary>
        public Point P2 { get; set; }

        /// <summary>
        /// Third point
        /// </summary>
        public Point P3 { get; set; }

        /// <summary>
        /// Color of the Triangle.
        /// </summary>
        public Color Color { get; set; }

        public Triangle(Point p1, Point p2, Point p3)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
            this.Color = Color.White;
        }

        public Triangle(Point p1, Point p2, Point p3, Color c)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.P3 = p3;
            this.Color = c;
        }

        /// <summary>
        /// Returns the triangle's area.
        /// </summary>
        /// <returns>area</returns>
        public double Area()
        {
            return System.Math.Sin(this.Angle(1)) * P1.DistanceTo(P2) * P1.DistanceTo(P3) / 2;
        }

        /// <summary>
        /// Returns the triangle's centroid.
        /// </summary>
        /// <returns>centroid (Point) </returns>
        public Point Centroid()
        {
            double nx = (P1.X + P2.X + P3.X) / 3;
            double ny = (P1.Y + P2.Y + P3.Y) / 3;
            double nz = (P1.Z + P2.Z + P3.Z) / 3;

            return new Point(nx, ny, nz);

        }

        /// <summary>
        /// Returns an angle of the triangle in RAD.
        /// </summary>
        /// <param name="i">Index of the angle. <b>Angle 0 is the angle at Point P1.</b></param>
        /// <returns>angle [RAD]</returns>
        public double Angle(int i)
        {
            // cosine rule

            double fractionTop = 0;
            double fractionBot = 2;
            for (int j = 0; j < 3; j++)
            {
                //OPTIMIERBAR triangle: calculate angle
                //Ineffizient! Man sollte hier eher mit den quadrierten Distanzen arbeiten,
                //da ggf sowieso quadriert werden muss
                double l = this.Edge(j).Length();
                if (j == i)
                {
                    fractionTop -= l * l;
                }
                else
                {
                    fractionTop += l * l;
                    fractionBot *= l;
                }
            }

            return System.Math.Acos(fractionTop / fractionBot);
        }

        public double[] Angles()
        {
            double[] a = new double[3];
            for(int i = 0; i<3; i++)
            {
                a[i] = Angle(i);
            }
            return a;
        }

        /// <summary>
        /// Returnes one edge of the triangle
        /// </summary>
        /// <param name="i">number of the edge given</param>
        /// <returns>one edge of the triangle</returns>
        public Edge Edge(int i)
        {
            if(i<0 || i>2){
                throw new Exception("A triangle has only 3 edges. Choose a number between 0 and 2!");
            }
            switch (i)
            {
                case (0):
                    return new Edge(P2, P3);
                case (1):
                    return new Edge(P1, P3);
                default:
                    return new Edge(P1, P2);
            }
        }

        /// <summary>
        /// Returns the third point of a triangle, if two points
        /// are known already.
        /// </summary>
        /// <param name="p1">first known point</param>
        /// <param name="p2">second known point</param>
        /// <returns>remaining unknown point</returns>
        public Point ThirdPoint(Point p1, Point p2)
        {
            List<Point> pts = new List<Point>();
            pts.Add(P1);
            pts.Add(P2);
            pts.Add(P3);
            pts.Remove(p1);
            pts.Remove(p2);
            return pts[0];
        }

        /// <summary>
        /// Returns a plane, which is coplanar to the triangle.
        /// </summary>
        /// <returns>coplanar plane</returns>
        public Plane ToPlane()
        {
            return Plane.NewFromTriangle(this);
        }

        /// <summary>
        /// Flips the triangle's normal.
        /// </summary>
        public void FlipNormal()
        {
            Point tmp = P1;
            P1 = P2;
            P2 = tmp;
        }

        /// <summary>
        /// Returns a string describing the triangle by it's points.
        /// </summary>
        /// <returns>string describing the triangle</returns>
        public override string ToString()
        {
            return ("Triangle: {" + P1.ToString() +"\n\t"+
                P2.ToString() + "\n\t" + P3.ToString() + "}");
        }


    }
}
