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

namespace TuryUtilCs.PointStructures
{
    /// <summary>
    /// One box of FrixGrid structures
    /// </summary>
    public class FixGridBox:List<Point>
    {
        #region properties

        //TODO set check correctness!
        // => one does not simply change the index of the grid box!
        // only possible case: gridbox will be extended or something.
        // but this still hast to be done.
        /// <summary>
        /// The position of the FixGridBox within the FixGrid structure
        /// along the x-axis.
        /// </summary>
        public int PosInGridX { get; set; }

        /// <summary>
        /// The position of the FixGridBox within the FixGrid structure
        /// along the y-axis.
        /// </summary>
        public int PosInGridY { get; set; }

        /// <summary>
        /// The position of the FixGridBox within the FixGrid structure
        /// along the z-axis.
        /// </summary>
       public int PosInGridZ { get; set; }
        
        /// <summary>
        /// The minimal x position within the FixGridBox
        /// </summary>
        public double Xmin { get; set; }
         
        /// <summary>
        /// The minimal y position within the FixGridBox
        /// </summary>
        public double Ymin { get; set; }
          
        /// <summary>
        /// The minimal z position within the FixGridBox
        /// </summary>
       public double Zmin { get; set; }

        /// <summary>
        /// The maximal x position within the FixGridBox
        /// </summary>
        public double Xmax { get; set; }
 
        /// <summary>
        /// The maximal y position within the FixGridBox
        /// </summary>
        public double Ymax { get; set; }
 
        /// <summary>
        /// The maximal z position within the FixGridBox
        /// </summary>
        public double Zmax { get; set; }

        /// <summary>
        /// The width of the FixGridBox along the x axis.
        /// </summary>
        public double XWidth
        {
            get { return Xmax - Xmin; }
        }

        /// <summary>
        /// The width of the FixGridBox along the y axis.
        /// </summary>
        public double YWidth
        {
            get { return Ymax - Ymin; }
        }

        /// <summary>
        /// The width of the FixGridBox along the z axis.
        /// </summary>
        public double ZWidth
        {
            get { return Zmax - Zmin; }
        }

        /// <summary>
        /// A freely chosen object that can be stored with the
        /// FixGridBox to add further information about it or it's
        /// content.
        /// </summary>
        public Object Load { get; set; }

        #endregion properties

        #region constructors
        /// <summary>
        /// Creates a FixGridBox filled with points but with unset boundaries.
        /// </summary>
        /// <param name="pts">points to store in the FixGridBox</param>
        [Obsolete]
        public FixGridBox(Point[] pts)
        {
            foreach (Point p in pts)
            {
                this.Add(p);
            }
        }



        /// <summary>
        /// Creates a new and empty FixGridBox.
        /// </summary>
        /// <param name="ix">
        /// Index of the FixGridBox within the FixGrid structure along the x axis
        /// </param>
        /// <param name="iy">
        /// Index of the FixGridBox within the FixGrid structure along the y axis
        /// </param>
        /// <param name="iz">
        /// Index of the FixGridBox within the FixGrid structure along the z axis
        /// </param>
        /// <param name="xmin">Minimal x value of the FixGridBox</param>
        /// <param name="ymin">Minimal y value of the FixGridBox</param>
        /// <param name="zmin">Minimal z value of the FixGridBox</param>
        /// <param name="xmax">Maximal x value of the FixGridBox</param>
        /// <param name="ymax">Maximal y value of the FixGridBox</param>
        /// <param name="zmax">Maximal z value of the FixGridBox</param>
        /// <param name="load">
        /// Additional and freely chosen object that will be stored
        /// along with the FixGridBox.
        /// </param>
        public FixGridBox(int ix, int iy, int iz,
            double xmin, double ymin, double zmin,
            double xmax, double ymax, double zmax, Object load)
        {
            PosInGridX = ix;
            PosInGridY = iy;
            PosInGridZ = iz;

            Xmin = xmin;
            Ymin = ymin;
            Zmin = zmin;
            Xmax = xmax;
            Ymax = ymax;
            Zmax = zmax;

            Load = load;
        }
        #endregion

    }
}
