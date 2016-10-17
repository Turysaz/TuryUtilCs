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
    /// fix grid structure to store huge amounts of points efficiently
    /// </summary>
    public class FixGrid
    {
        #region debug fields
        //DOKU
        private static double _additionalPercentage = 10;

        #endregion

        #region fields
        /// <summary>
        /// The minimal x, y and z coordinates of the grid
        /// </summary>
        private double[] _dimensions_min = new double[3];
        /// <summary>
        /// The maximal x, y and z coordinates of the grid
        /// </summary>
        private double[] _dimensions_max = new double[3];
        /// <summary>
        /// The amount of subdivisions of the grid in x, y and z direction
        /// </summary>
        private int[] _subdivision = new int[3];
        #endregion

        #region properties

        #region grid size
        /// <summary>
        /// Minimal x value
        /// </summary>
        public double Xmin
        {
            get { return _dimensions_min[0]; }
            set { _dimensions_min[0] = value; }
        }
        /// <summary>
        /// Minimal y value
        /// </summary>
        public double Ymin
        {
            get { return _dimensions_min[1]; }
            set { _dimensions_min[1] = value; }
        }
        /// <summary>
        /// Minimal z value
        /// </summary>
        public double Zmin
        {
            get { return _dimensions_min[2]; }
            set { _dimensions_min[2] = value; }
        }

        /// <summary>
        /// Maximal x value
        /// </summary>
        public double Xmax
        {
            get { return _dimensions_max[0]; }
            set { _dimensions_max[0] = value; }
        }

        /// <summary>
        /// Maximal y value
        /// </summary>
        public double Ymax
        {
            get { return _dimensions_max[1]; }
            set { _dimensions_max[1] = value; }
        }

        /// <summary>
        /// Maximal z Value
        /// </summary>
        public double Zmax
        {
            get { return _dimensions_max[2]; }
            set { _dimensions_max[2] = value; }
        }
        #endregion

        /// <summary>
        /// Subdivision of the grid in x direction
        /// </summary>
        public int Xdiv
        {
            get { return _subdivision[0]; }
            set { _subdivision[0] = value; }
        }

        /// <summary>
        /// Subdivision of the grid in y direction
        /// </summary>
        public int Ydiv
        {
            get { return _subdivision[1]; }
            set { _subdivision[1] = value; }
        }

        /// <summary>
        /// Subdivision of the grid in z direction
        /// </summary>
        public int Zdiv
        {
            get { return _subdivision[2]; }
            set { _subdivision[2] = value; }
        }


        /// <summary>
        /// The width of each FixGridBox in x direction
        /// </summary>
        public double XBoxWidth
        {
            get { return (Xmax - Xmin) / Xdiv; }
        }

        /// <summary>
        /// The length of each FixGridBox in y direction
        /// </summary>
        public double YBoxWidth
        {
            get { return (Ymax - Ymin) / Ydiv; }
        }

        /// <summary>
        /// The heigth of each FixGridBox in z direction
        /// </summary>
        public double ZBoxWidth
        {
            get { return (Zmax - Zmin) / Zdiv; }
        }

        /// <summary>
        /// The Grid's Boxes
        /// </summary>
        public FixGridBox[,,] Boxes { get; set; }

        #endregion properties


        #region constructors

        //DOKU
        public FixGrid(Point[] pts, int subdivision)
            : this(pts, subdivision, subdivision, subdivision)
        {
        }

        /// <summary>
        /// Creates a new FixedGrid from an existing point set.
        /// The dimensions will be set to the minimal surrounding box.
        /// </summary>
        /// <param name="pts">Point set</param>
        /// <param name="xdiv">division of x</param>
        /// <param name="ydiv">division of y</param>
        /// <param name="zdiv">division of z</param>
        public FixGrid(Point[] pts, int xdiv, int ydiv, int zdiv)
        {
            //division is argument set -> fix!
            Xdiv = xdiv;
            Ydiv = ydiv;
            Zdiv = zdiv;

            double[] boundaries = GetPointSetBoundaries(pts);
            Xmin = boundaries[0];
            Xmax = boundaries[1];
            Ymin = boundaries[2];
            Ymax = boundaries[3];
            Zmin = boundaries[4];
            Zmax = boundaries[5];

            CreateBoxes();
            SortPointsToBoxes(pts);
        }

        /// <summary>
        /// Creates an empty FixGrid structure with given
        /// parameters.
        /// </summary>
        /// <param name="xmin">minimal x value</param>
        /// <param name="ymin">minimal y value</param>
        /// <param name="zmin">minimal z value</param>
        /// <param name="xmax">maximal x value</param>
        /// <param name="ymax">maximal y value</param>
        /// <param name="zmax">maximal z value</param>
        /// <param name="xdiv">subdivisions along x</param>
        /// <param name="ydiv">subdivisions along y</param>
        /// <param name="zdiv">subdivisions along z</param>
        public FixGrid(double xmin, double ymin, double zmin,
            double xmax, double ymax, double zmax,
            int xdiv, int ydiv, int zdiv)
        {
            Xmin = xmin;
            Xmax = xmax;
            Ymin = ymin;
            Ymax = ymax;
            Zmin = zmin;
            Zmax = zmax;
            Xdiv = xdiv;
            Ydiv = ydiv;
            Zdiv = zdiv;

            CreateBoxes();
        }

        #endregion constructors

        //DOKU
        public static FixGrid MinimalSubdivision(Point[] pts, int minSubdivision)
        {

            double[] boundaries = GetPointSetBoundaries(pts, _additionalPercentage);

            double xmin = boundaries[0];
            double xmax = boundaries[1];
            double ymin = boundaries[2];
            double ymax = boundaries[3];
            double zmin = boundaries[4];
            double zmax = boundaries[5];

            double
                xwidth = xmax - xmin,
                ywidth = ymax - ymin,
                zwidth = zmax - zmin;

            double smallestWidth = xwidth;
            if (xwidth > ywidth) { smallestWidth = ywidth; }
            if (smallestWidth > zwidth) { smallestWidth = zwidth; }

            int xSubdivision = (int)(minSubdivision * xwidth / smallestWidth);
            int ySubdivision = (int)(minSubdivision * ywidth / smallestWidth);
            int zSubdivision = (int)(minSubdivision * zwidth / smallestWidth);

            return new FixGrid(pts, xSubdivision, ySubdivision, zSubdivision);

        }

        //DOKU
        public static FixGrid MaximalSubdivision(Point[] pts, int minSubdivision)
        {

            double[] boundaries = GetPointSetBoundaries(pts, _additionalPercentage);

            double xmin = boundaries[0];
            double xmax = boundaries[1];
            double ymin = boundaries[2];
            double ymax = boundaries[3];
            double zmin = boundaries[4];
            double zmax = boundaries[5];

            double
                xwidth = xmax - xmin,
                ywidth = ymax - ymin,
                zwidth = zmax - zmin;

            double largestWidth = xwidth;
            if (xwidth < ywidth) { largestWidth = ywidth; }
            if (largestWidth < zwidth) { largestWidth = zwidth; }

            int xSubdivision = (int)(minSubdivision * xwidth / largestWidth);
            int ySubdivision = (int)(minSubdivision * ywidth / largestWidth);
            int zSubdivision = (int)(minSubdivision * zwidth / largestWidth);

            return new FixGrid(pts, xSubdivision, ySubdivision, zSubdivision);
        }


        //DOKU
        private static double[] GetPointSetBoundaries(Point[] pts)
        {
            double
                xmin = pts[0].X,
                xmax = xmin,
                ymin = pts[0].Y,
                ymax = ymin,
                zmin = pts[0].Z,
                zmax = zmin;

            foreach (Point p in pts)
            {
                if (p.X > xmax) { xmax = p.X; }
                if (p.Y > ymax) { ymax = p.Y; }
                if (p.Z > zmax) { zmax = p.Z; }
                if (p.X < xmin) { xmin = p.X; }
                if (p.Y < ymin) { ymin = p.Y; }
                if (p.Z < zmin) { zmin = p.Z; }
            }
            return new double[] { xmin, xmax, ymin, ymax, zmin, zmax };
        }

        //DOKU
        private static double[] GetPointSetBoundaries(Point[] pts, double additionalPercent)
        {
            double ap = (additionalPercent / 100) + 1;
            double[] basic = GetPointSetBoundaries(pts);
            for (int i = 0; i < 6; i++)
            {
                basic[i] *= ap;
            }
            return basic;
        }

        /// <summary>
        /// Create empty GridBoxes of a FixGrid structure.
        /// </summary>
        protected void CreateBoxes()
        {
            Boxes = new FixGridBox[Xdiv, Ydiv, Zdiv];

            for (int x = 0; x < Xdiv; x++)
            {
                for (int y = 0; y < Ydiv; y++)
                {
                    for (int z = 0; z < Zdiv; z++)
                    {
                        Boxes[x, y, z] = new FixGridBox(x, y, z,
                            Xmin + x * XBoxWidth,
                            Ymin + y * YBoxWidth,
                            Zmin + z * ZBoxWidth,
                            Xmin + (x + 1) * XBoxWidth,
                            Ymin + (y + 1) * YBoxWidth,
                            Zmin + (z + 1) * ZBoxWidth,
                            null);
                    }
                }
            }
        }

        /// <summary>
        /// Assign the points of an unorganised point set to
        /// the FixGrid's GridBoxes.
        /// </summary>
        /// <param name="pts">Unorganised point set</param>
        public void SortPointsToBoxes(Point[] pts)
        {
            foreach (Point p in pts)
            {
                int x = (int)((p.X - Xmin) / XBoxWidth);
                int y = (int)((p.Y - Ymin) / YBoxWidth);
                int z = (int)((p.Z - Zmin) / ZBoxWidth);

                if (p.X == Xmax) { x--; }
                if (p.Y == Ymax) { y--; }
                if (p.Z == Zmax) { z--; }

                Boxes[x, y, z].Add(p);
            }
        }

        /// <summary>
        /// Assign the points of an unorganised point set to
        /// the FixGrid's GridBoxes.
        /// </summary>
        /// <param name="pts">Unorganised point set</param>
        public void SortPointsToBoxes(List<Point> pts)
        {
            SortPointsToBoxes(pts.ToArray());
        }

        /// <summary>
        /// Get all surrounding GridBoxes around a given point
        /// (query point).
        /// The surrounding boxes are all boxes that do have any
        /// contact to the center box and the center box itself.
        /// So, in the regular case, this method will return 27 boxes.
        /// If however the query point lies within one of the outer
        /// boxes or even beyond the boundaries of the FixGrid, 
        /// this method will return less than 27 boxes.
        /// In this case, the SurroundingBoxes3x3 will assume the
        /// closest box to the query point to be the center and return
        /// the boxes surrounding this box.
        /// Hence, the minimum length of the returned FixGridBox-array
        /// will be 8 (closest box to query is one corner of the 
        /// FixGrid structure).
        /// </summary>
        /// <param name="p">query point</param>
        /// <returns>Array of surrounding GridBoxes</returns>
        public virtual FixGridBox[] SurroundingBoxes3x3(Point p)
        {
            // indices of the center box
            int x = (int)((p.X - Xmin) / XBoxWidth);
            int y = (int)((p.Y - Ymin) / YBoxWidth);
            int z = (int)((p.Z - Zmin) / ZBoxWidth);

            return SurroundingBoxes3x3(x, y, z);

        }

        //DOKU
        public FixGridBox[] SurroundingBoxes3x3(int x, int y, int z)
        {

            List<FixGridBox> retBox = new List<FixGridBox>();

            if (x < 1) { x = 1; }
            if (y < 1) { y = 1; }
            if (z < 1) { z = 1; }

            if (x > Xdiv - 2) { x = Xdiv - 2; }
            if (y > Ydiv - 2) { y = Ydiv - 2; }
            if (z > Zdiv - 2) { z = Zdiv - 2; }

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        retBox.Add(Boxes[x + i, y + j, z + k]);
                    }
                }
            }
            return retBox.ToArray();

        }

        //DOKU
        public FixGridBox[] SurroundingBoxes3x3(FixGridBox box)
        {
            int x = box.PosInGridX;
            int y = box.PosInGridY;
            int z = box.PosInGridZ;

            return SurroundingBoxes3x3(x, y, z);
        }

        //DOKU
        public FixGridBox[] NeighbourLayers(FixGridBox box, string layernames)
        {
            int x = box.PosInGridX;
            int y = box.PosInGridY;
            int z = box.PosInGridZ;
            return NeighbourLayers(x, y, z, layernames, true);
        }

        //DOKU
        public FixGridBox[] NeighbourLayers(int x, int y, int z, string layernames)
        {
            return NeighbourLayers(x, y, z, layernames, true);
        }

        //TEST_NECESSARY
        //DOKU
        public FixGridBox[] NeighbourLayers(int x, int y, int z, string layernames, bool addCenter)
        {
            // possible layers:
            //   d - down   min z
            //   t - top    max z
            //   l - left   min y
            //   r - right  max y
            //   b - back   min x
            //   f - front  max x
            if (x < 0 || y < 0 || z < 0 || x > Xdiv - 1 || y > Ydiv - 1 || z > Zdiv - 1)
            {
                string msg = string.Format("Invalid Index! Possible dimensions are:\n" +
                    "X : 0..{0}; Y: 0..{1}, Z: 0..{2}\n" +
                    "Your request: ({3}; {4}; {5})",
                    Xdiv - 1, Ydiv - 1, Zdiv - 1, x, y, z);
                throw new IndexOutOfRangeException(msg);
            }

            if (layernames == "")
            {
                Console.WriteLine(
                    "FIXGRID WARNING: No layers specified for neighbour search!");
                return null;
            }

            int xmin, ymin, zmin,
                xmax, ymax, zmax;

            // DOKU
            if (layernames.Contains('b')) { xmin = -1; } else { xmin = 0; }
            if (layernames.Contains('l')) { ymin = -1; } else { ymin = 0; }
            if (layernames.Contains('d')) { zmin = -1; } else { zmin = 0; }
            if (layernames.Contains('f')) { xmax = 1; } else { xmax = 0; }
            if (layernames.Contains('r')) { ymax = 1; } else { ymax = 0; }
            if (layernames.Contains('t')) { zmax = 1; } else { zmax = 0; }


            List<FixGridBox> ret = new List<FixGridBox>();

            if (x + xmin < 0) { xmin = 0; }
            if (y + ymin < 0) { ymin = 0; }
            if (z + zmin < 0) { zmin = 0; }

            if (x + xmax == Xdiv) { xmax = 0; }
            if (y + ymax == Ydiv) { ymax = 0; }
            if (z + zmax == Zdiv) { zmax = 0; }

            for (int i = xmin; i <= xmax; i++)
            {
                for (int j = ymin; j <= ymax; j++)
                {
                    for (int k = zmin; k <= zmax; k++)
                    {
                        // skip center
                        if (!addCenter && i == 0 && j == 0 && k == 0) { continue; }

                        ret.Add(Boxes[x + i, y + j, z + k]);
                    }
                }
            }

            return ret.ToArray();

        } // end method
    }
}
