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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuryUtilCs.Mathmatics.Geometry
{
    //DOKU
    public class Cuboid
    {
        //HOT UNIT TESTS

        #region fields
        //DOKU
        private Point _center;

        //DOKU
        private Vector[] _dimensions = new Vector[3];
        #endregion

        #region properties manually
        //DOKU
        public Point Center
        {
            get { return _center; }
        }

        //DOKU
        public Vector[] Dimensions
        {
            get { return _dimensions; }
        }

        public double Volume
        {
            get
            {
                return _dimensions[0].Length() *
                  _dimensions[1].Length() *
                  _dimensions[2].Length();
            }
        }
        #endregion

        #region properties automatically
        #endregion

        //DOKU
        public static Cuboid BoundingBoxFromPCA(Point[] pts,
            int accuracy, int eigenvalueScalingFactor)
        {
            //HOT DRY: most of the stuff below was just copied from Plane.cs
            // redundancy, see note in Plane.cs
            #region information, description
            // Calculation of planes like this according to Hoppe et al. 1992
            // Information about the algorithm gathered in
            //      
            //      J. Hartung, "Multivariante Statistik"
            //      Oldenburg-Verlag München 2007, 7. Auflage
            //      Seite 505 ff.
            //
            //  Description of the calculation:
            //  ===============================
            //
            //  Points are characteristics matrix Y
            //
            //      Y = y_ij        i ... point
            //                      j ... component(x,y,z)
            //  
            //  Defining normalized characteristics matrix Y'
            //  
            //      Y' = y'_ij
            //  with
            //      y'_ij = ( y_ij - y*_j ) / s_j
            //                      
            //          y*_j = mean of this component of all points
            //          s_j  = standard deviation of this component
            //
            //  REMARK 03.08.2016: I'll leave the division by s_j out of this...
            //                     So it's y'_ij = y_ij - y*_j for now
            //
            //
            //  After that, the empirical correlation matrix is formed
            // 
            //      R = Y'^ *  Y'       T'^ ... transposed of T'
            //
            //  The eigenvector to the smallest eigenvalue of this matrix
            //  is the normal of the plane
            #endregion


            Matrix normY = new Matrix(pts.Length, 3);
            Matrix correlationMatrix;
            Point center = Geometry.Point.Center(pts);
            Point current;
            for (int i = 0; i < pts.Length; i++)
            {
                current = pts[i];
                normY[i, 0] = current.X - center.X;
                normY[i, 1] = current.Y - center.Y;
                normY[i, 2] = current.Z - center.Z;
            }
            correlationMatrix = Matrix.Multiply(normY.Transposed(), normY);
            Matrix[] eigenvectors = correlationMatrix.Eigenvectors(
                accuracy, eigenvalueScalingFactor, true, true);
            Vector[] normals = new Vector[3];
            for (int i = 0; i < 3; i++)
            {
                // eigenvectors are only only half the dimensions; 
                // hence have to multiply by 2
                normals[i] = eigenvectors[i].ToVector().MultiplySkalar(2);
            }

            return new Cuboid(center, normals);
        }

        #region constructors
        //DOKU
        public Cuboid(Point center, Vector[] dimensions)
        {
            this._center = center;
            this._dimensions = dimensions;
        }

        //DOKU
        public Cuboid(Point a, Point b, Point c)
        {
            // TODO is this even definite??
            // UNDONE Cuboid by three points
            throw new NotImplementedException();
        }
        #endregion

        //HOT HACK
        /// <summary>
        /// Returns the "main" planes of the box.
        /// The planes are sorted by their area:
        /// Plane[0] is the smallest plane; Plane[2]
        /// has the largest area.
        /// </summary>
        public Plane[] MainPlanes()
        {
            Plane[] mainPlanes = new Plane[3];
            List<Vector> dimensionsForSort = _dimensions.ToList();
            dimensionsForSort.Sort(delegate (Vector v1, Vector v2)
            {
                return (v1.Length().CompareTo(v2.Length()));
            });
            mainPlanes[0] = new Plane(Center.ToVector(), dimensionsForSort[2]);
            mainPlanes[1] = new Plane(Center.ToVector(), dimensionsForSort[1]);
            mainPlanes[2] = new Plane(Center.ToVector(), dimensionsForSort[0]);
            return mainPlanes;
        }
    }
}
