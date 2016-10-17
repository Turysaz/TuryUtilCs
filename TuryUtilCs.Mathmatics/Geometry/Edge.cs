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
    /// A line between two Points. The edge contains a color.
    /// The color does not need to be set, default is white.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// First Point
        /// </summary>
        public Point P1 { get; set; }

        /// <summary>
        /// Second Point
        /// </summary>
        public Point P2 { get; set; }

        /// <summary>
        /// Color of the Edge.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Construktor: Creates an Edge by it's endpoints.
        /// The Edges's color is set to white (default).
        /// </summary>
        /// <param name="p1">first point</param>
        /// <param name="p2">second point</param>
        public Edge(Point p1, Point p2)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.Color = Color.White;
        }

        /// <summary>
        /// Creates a new Edge between two points with a
        /// specified color.
        /// </summary>
        /// <param name="p1">fist endpoint</param>
        /// <param name="p2">second endpoint</param>
        /// <param name="c">Color of the Edge</param>
        public Edge(Point p1, Point p2, Color c)
        {
            this.P1 = p1;
            this.P2 = p2;
            this.Color = c;
        }


        /// <summary>
        /// Returnes the middle point of the edge.
        /// </summary>
        /// <returns>middle point</returns>
        public Point Center()
        {
            double x = (P1.X + P2.X) / 2;
            double y = (P1.Y + P2.Y) / 2;
            double z = (P1.Z + P2.Z) / 2;

            return new Point(x, y, z);
        }


        /// <summary>
        /// Returns a Line, which is colinear to the Edge.
        /// </summary>
        /// <returns>line</returns>
        public Line ToLine()
        {
            return new Line(this.P1.ToVector(), new Vector(this.P1, this.P2));
        }

        public Vector ToVector()
        {
            return new Vector(P1.X - P2.X, P1.Y - P2.Y, P1.Z - P2.Z);
        }

        /// <summary>
        /// Returns the distance between the two points of the edge,
        /// respective it's length
        /// </summary>
        /// <returns>distance between the points</returns>
        public double Length()
        {
            return P1.DistanceTo(P2);
        }


        /// <summary>
        /// Returns the minimal distance between the edge and a point.
        /// </summary>
        /// <param name="p">point, to which the distance shall be
        /// calculated</param>
        /// <returns>The distance between the edge and the point</returns>
        public double DistanceTo(Point p)
        {
            Point projectionOfP = p.ProjectionToLine(this.ToLine());

            double length = this.Length();
            double distProjectionToP1 = this.P1.DistanceTo(projectionOfP);
            double distProjectionToP2 = this.P2.DistanceTo(projectionOfP);

            if (distProjectionToP1 < length && distProjectionToP2 < length)
            {
                return p.DistanceToLine(this.ToLine());
            }
            else
            {
                return System.Math.Min(p.DistanceTo(this.P1), p.DistanceTo(this.P2));
            }
        }
        /// <summary>
        /// Returns a string describing the edge by it's points.
        /// </summary>
        /// <returns>string describing the edge</returns>
        public override string ToString()
        {
            return ("Edge {P1: " + P1.ToString() +
                " <---> P2: " + P2.ToString() + "}");
        }
    }
}
