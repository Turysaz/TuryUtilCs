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

//HOT UNIT TEST
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TuryUtilCs.Mathmatics.Geometry;

namespace TuryUtilCs.PointStructures
{
    //DOKU
    public class OctreeBox
    {
        #region fields

        //DOKU
        protected double[] _dimensionsMin = new double[3];

        //DOKU
        protected double[] _dimensionsMax = new double[3];

        #endregion

        #region properties - manually
        //DOKU
        public double[] DimMin
        {
            get { return _dimensionsMin; }
        }

        //DOKU
        public double[] DimMax
        {
            get { return _dimensionsMax; }
        }

        //DOKU
        public double XWidth
        {
            get { return Xmax - Xmin; }
        }

        //DOKU
        public double YWidth
        {
            get { return Ymax - Ymin; }
        }

        //DOKU
        public double ZWidth
        {
            get { return Zmax - Zmin; }
        }

        //DOKU
        public double Xmin
        {
            get { return _dimensionsMin[0]; }
        }

        //DOKU
        public double Ymin
        {
            get { return _dimensionsMin[1]; }
        }

        //DOKU
        public double Zmin
        {
            get { return _dimensionsMin[2]; }
        }

        //DOKU
        public double Xmax
        {
            get { return _dimensionsMax[0]; }
        }

        //DOKU
        public double Ymax
        {
            get { return _dimensionsMax[1]; }
        }

        //DOKU
        public double Zmax
        {
            get { return _dimensionsMax[2]; }
        }

        //DOKU
        public double Xmean
        {
            get { return (_dimensionsMax[0] - _dimensionsMin[0]) / 2 + _dimensionsMin[0]; }
        }

        //DOKU
        public double Ymean
        {
            get { return (_dimensionsMax[1] - _dimensionsMin[1]) / 2 + _dimensionsMin[1]; }
        }

        //DOKU
        public double Zmean
        {
            get { return (_dimensionsMax[2] - _dimensionsMin[2]) / 2 + _dimensionsMin[2]; }
        }

        #region Getter for individual octree boxes

        //DOKU
        public OctreeBox BoxUpBackLeft
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[0]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxUpBackRight
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[1]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxUpFrontLeft
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[2]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxUpFrontRight
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[3]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxBottomBackLeft
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[4]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxBottomBackRight
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[5]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxBottomFrontLeft
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[6]; }
                return null;
            }
        }

        //DOKU
        public OctreeBox BoxBottomFrontRight
        {
            get
            {
                if (SubBoxes != null) { return SubBoxes[7]; }
                return null;
            }
        }

        #endregion

        #endregion

        #region properties - automatically

        //DOKU
        public List<Point> Points { get; set; }

        //DOKU
        public bool IsLeaf { get; private set; }

        /// <summary>
        /// Sub nodes of the node within the octree.
        /// 2 layer of 4 boxes each. Top layer: max Z,
        /// bottom layer: min Z.
        /// Each layer, viewed from top, is numbered
        /// columns first, rows last. So, in the bottom
        /// layer, the "upper left" sub box sill be 
        /// SubBoxes[0], the one on it's right will be
        /// SubBoxes[1], the one below the first one 
        /// will be SubBoxes[2] and the box in
        /// the "lower right" corner will be SubBoxes[3].
        /// Same procedure about the top layer.
        /// </summary>
        public OctreeBox[] SubBoxes { get; set; }

        //DOKU
        public Object Load { get; set; }

        #endregion

        #region constructors

        //HOT public class interface! 

        //DOKU
        public OctreeBox(List<Point> pts, int minNotLeafLayers)
        {
            // find dimensions

            double xmin, xmax, ymin, ymax, zmin, zmax;
            xmin = pts[0].X;
            ymin = pts[0].Y;
            zmin = pts[0].Z;
            xmax = xmin;
            ymax = ymin;
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
            this._dimensionsMin = new double[] { xmin, ymin, zmin };
            this._dimensionsMax = new double[] { xmax, ymax, zmax };

            if (minNotLeafLayers == 0)
            {
                this.IsLeaf = true;
                this.SubBoxes = null;
                this.Points = pts;
            }
            else
            {
                // All points in pts MUST lie within this box!
                Subdivide(pts, minNotLeafLayers - 1);
            }
        }


        private OctreeBox(List<Point> pts, OctreeBox parent,
            int remainingNotLeafLayers, int positionInParent)
        {
            // I'm even more proud about this boundary finder :)
            for (int i = 2; i >= 0; i--)
            {
                int n = (int)System.Math.Pow(2, i);
                if (positionInParent >= n)
                {
                    _dimensionsMax[i] = parent._dimensionsMax[i];
                    _dimensionsMin[i] = parent.Mean(i);
                    positionInParent -= n;
                }
                else
                {
                    _dimensionsMax[i] = parent.Mean(i);
                    _dimensionsMin[i] = parent._dimensionsMin[i];
                }
            }
            if (remainingNotLeafLayers == 0)
            {
                this.IsLeaf = true;
                this.SubBoxes = null;
                this.Points = pts;
            }
            else
            {
                // All points in pts MUST lie within this box!
                Subdivide(pts, remainingNotLeafLayers - 1);
            }

        }



        #endregion

        //DOKU
        public double Mean(int i)
        {
            switch (i)
            {
                case (0):
                    return Xmean;
                case (1):
                    return Ymean;
                case (2):
                    return Zmean;
            }
            throw new ArgumentException("'i' must be element of {0,1,2}!");
        }

        //DOKU
        public OctreeBox[] Subdivide()
        {
            if (!IsLeaf)
            {
                throw new Exception(
                    "Cannot subdivide a box that has already been subdivided!");
            }

            return Subdivide(this.Points, 0);
        }

        //DOKU
        private OctreeBox[] Subdivide(List<Point> pts, int remainingNoLeafLayers)
        {
            this.SubBoxes = new OctreeBox[8];

            List<Point>[] subdividedPointCloud = new List<Point>[8];

            // initialize array
            for (int i = 0; i < 8; i++)
            {
                subdividedPointCloud[i] = new List<Point>();
            }

            foreach (Point p in pts)
            {
                // I'm proud of this simple method of assigning the point :-)
                int boxNumber = 0;
                if (p.Z > Zmean) { boxNumber += 4; }
                if (p.Y > Ymean) { boxNumber += 2; }
                if (p.X > Xmean) { boxNumber += 1; }
                subdividedPointCloud[boxNumber].Add(p);
            }

            // write divided points to subboxes
            for (int i = 0; i < 8; i++)
            {
                SubBoxes[i] =
                    new OctreeBox(subdividedPointCloud[i], this, remainingNoLeafLayers, i);
            }

            // reset Points to null, for all points are now Points of the SubBoxes
            this.Points = null;
            this.IsLeaf = false;

            return this.SubBoxes;
        }

        //DOKU
        public bool PointWithin(Point p)
        {
            return WithinXLayer(p)
                && WithinYLayer(p)
                && WithinZLayer(p);
        }

        //DOKU
        private bool WithinXLayer(Point p)
        {
            //TODO function inline
            return WithinLayer(p, 0);
        }

        //DOKU
        private bool WithinYLayer(Point p)
        {
            //TODO inline
            return WithinLayer(p, 1);
        }

        //DOKU
        private bool WithinZLayer(Point p)
        {
            //TODO inline
            return WithinLayer(p, 2);
        }

        private bool WithinLayer(Point p, int n)
        {
            //TODO inline
            return !(p[n] < DimMin[n] || p[n] > DimMax[n]);
        }

        //DOKU
        public double MinimumSquareDistanceTo(Point p)
        {
            double[] nDist = new double[3];

            for (int i = 0; i < 3; i++)
            {
                if (WithinLayer(p, i))
                {
                    nDist[i] = 0;
                }
                else
                {
                    nDist[i] = Math.Min(p[i] - DimMin[i],
                        DimMax[i] - p[i]);
                }
            }
            return (nDist[0] * nDist[0] +
                nDist[1] * nDist[1] +
                nDist[2] * nDist[2]);
        }

        //DOKU
        public double MinimumDistanceTo(Point p)
        {
            return Math.Sqrt(MinimumSquareDistanceTo(p));
        }
    }
}
