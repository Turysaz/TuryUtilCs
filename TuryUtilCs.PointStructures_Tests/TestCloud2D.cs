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

﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TuryUtilCs.Mathmatics.Geometry;

namespace TuryUtilCs.PointStructures.Tests
{
    [TestClass()]
    public class TestCloud2D
    {
        double[] xVals = new double[]
        {   0,
            1,4,7,9,-1,
            2,5,7,10,9,
            11,-2,0,-4,4,
            2,6,7,9,12,
            11,9,7,5,3,
            1,-2,-4,-6,-5,
            -3,-3,-1,0,1,
            2,4,3
        };

        double[] yVals = new double[]
        {
            0,
            8,8,9,9,7,
            6,6,6,7,5,
            5,5,5,4,4,
            3,3,2,3,3,
            1,1,0,1,1,
            1,2,1,0,-1,
            -1,-3,-2,-5,-4,
            -2,-2,-5
        };

        public Point[] Pts;

        public Point Q1 = new Point(4, 3, 0);
        public Point Q2 = new Point(-2, 0, 0);
        public Point Q3 = new Point(10, 4, 0);
        public Point Q4 = new Point(4, -4, 0);

        // length = 5
        public int[] ResQ1_Range3 = new int[] 
        { 15, 16, 17, 24, 25 };

        // length = 14
        public int[] ResQ2_Range5 = new int[] 
        {
            0, 12, 14, 16, 26,
            27, 28, 29, 30, 31,
            32, 33, 35, 36
        };

        // length = 3
        public int[] ResQ3_closest3 = new int[] { 10, 11, 19 };

        public int ResQ4_closest = 38;

        public TestCloud2D()
        {
            Pts = new Point[39];
            for (int i = 0; i < 39; i++)
            {
                Pts[i] = new Point(xVals[i], yVals[i], 0);
            }
        }

        public Point[] ResultPoints_Q1_Range3()
        {
            List<Point> ret = new List<Point>();
            foreach (int i in ResQ1_Range3)
            {
                ret.Add(Pts[i]);
            }
            return ret.ToArray();
        }

        public Point[] ResultPoints_Q2_Range5()
        {
            List<Point> ret = new List<Point>();
            foreach (int i in ResQ2_Range5)
            {
                ret.Add(Pts[i]);
            }
            return ret.ToArray();
        }

        public Point[] ResultPoints_Q3_Closest3()
        {
            List<Point> ret = new List<Point>();
            foreach (int i in ResQ3_closest3)
            {
                ret.Add(Pts[i]);
            }
            return ret.ToArray();
        }

        public Point ResultPoints_Q4_Closest()
        {
            return Pts[ResQ4_closest];
        }
        
        public Point[] ReturnExcluded(Point p)
        {
            return ReturnExcluded(new Point[] { p });
        }
        
        public Point[] ReturnExcluded(Point[] included)
        {
            List<Point> all = Pts.ToList();
            foreach (Point p in included)
            {
                all.Remove(p);
            }
            return all.ToArray();
        }

        // SELFDIAGNOSTICS

        [TestMethod()]
        public void TestCloud2D_Selftest()
        {
            Assert.AreEqual(xVals.Length, yVals.Length);
            #region Q1
            foreach (Point p in ResultPoints_Q1_Range3())
            {
                if (p.DistanceTo(Q1) > 3)
                {
                    Assert.Fail("Q1 This Point does not belong here!");
                }
            }
            foreach (Point p in ReturnExcluded(ResultPoints_Q1_Range3()))
            {
                if (p.DistanceTo(Q1) <= 3)
                {
                    Assert.Fail("Q1 This Point should not be excluded!");
                }
            }
            #endregion

            #region Q2
            foreach (Point p in ResultPoints_Q2_Range5())
            {
                if (p.DistanceTo(Q2) > 5)
                {
                    Assert.Fail(String.Format(
                        "Q2 This Point does not belong here! ({0})", p.ToString()));
                }
            }
            foreach (Point p in ReturnExcluded(ResultPoints_Q2_Range5()))
            {
                if (p.DistanceTo(Q2) <= 5)
                {
                    Assert.Fail("Q2 This Point should not be excluded!");
                }
            }
            #endregion

            #region Q3
            Assert.AreEqual(3, ResultPoints_Q3_Closest3().Length);

            double maxdist = 0;

            foreach (Point p in ResultPoints_Q3_Closest3())
            {
                if (p.DistanceTo(Q3) > maxdist)
                {
                    maxdist = p.DistanceTo(Q3);
                }
            }

            foreach (Point p in ReturnExcluded(ResultPoints_Q3_Closest3()))
            {
                if (p.DistanceTo(Q3) < maxdist)
                {
                    Assert.Fail("Q3 This Point should not be excluded!");
                }
            }
            #endregion

            #region Q4

            maxdist = ResultPoints_Q4_Closest().DistanceTo(Q4);

            foreach (Point p in ReturnExcluded(ResultPoints_Q4_Closest()))
            {
                if (p.DistanceTo(Q4) < maxdist)
                {
                    Assert.Fail("Q4 This Point should not be excluded!");
                }
            }
            #endregion
        }

    }
}
