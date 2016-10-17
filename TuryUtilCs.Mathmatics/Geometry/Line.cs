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
    /// Raumgerade
    /// </summary>
    public class Line
    {
        /// <summary>
        /// Point on the line. Defines the position in space.
        /// </summary>
        public Vector Point { get; set; }

        /// <summary>
        /// Defines the Orientation in space.
        /// </summary>
        public Vector Direction { get; set; }

        public Line(Vector point, Vector direction)
        {
            this.Point = point;
            this.Direction = direction;
        }

        /// <summary>
        /// Checks, if another line is colinear to this one.
        /// </summary>
        /// <param name="l">other line</param>
        /// <returns>true, if both lines are colinear</returns>
        public Boolean Identical(Line l)
        {
            if (!Parallel(l)) { return false; }

            double factor = (l.Point.X - this.Point.X) / this.Direction.X;

            if (this.Point.Y + factor * this.Direction.Y != l.Point.Y) { return false; }
            if (this.Point.Z + factor * this.Direction.Z != l.Point.Z) { return false; }

            return true;
        }

        /// <summary>
        /// Checks, if the line crosses another line or not.
        /// </summary>
        /// <param name="l">other line</param>
        /// <returns>true, if the lines are crossing</returns>
        public Boolean Crossing(Line l)
        {

            double factor = (l.Point.X - this.Point.X) / this.Direction.X;

            if (this.Point.Y + factor * this.Direction.Y != l.Point.Y) { return false; }
            if (this.Point.Z + factor * this.Direction.Z != l.Point.Z) { return false; }

            return true;
        }

        /// <summary>
        /// Checks, if a second line is parallel to this one.
        /// </summary>
        /// <param name="l">second line</param>
        /// <returns>true, if the lines are parallel</returns>
        public Boolean Parallel(Line l)
        {
            if (Vector.CrossProduct(this.Direction, l.Direction).Length() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returnes the minimal distance between the line and a point in space
        /// </summary>
        /// <param name="p">point in space</param>
        /// <returns>distance between the line and the point</returns>
        public double DistanceToPoint(Point p)
        {
            return p.DistanceToLine(this);
        }


        /// <summary>
        /// Returns a string describing the line by it's point and it's direction.
        /// </summary>
        /// <returns>string describing the line</returns>
        public override string ToString()
        {
            return ("Line {Point: " + Point.ToString() +
                ", Direction: " + Direction.ToString() + "}");
        }
    }
}
