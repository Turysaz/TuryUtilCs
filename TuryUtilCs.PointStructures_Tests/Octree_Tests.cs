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
using TuryUtilCs.PointStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TuryUtilCs.Mathmatics.Geometry;

namespace TuryUtilCs.PointStructures.Tests
{
    [TestClass()]
    public class Octree_Tests
    {
        //DOKU
        private Point[] testPointSetRandom;
        private Point[] testPointSetMinimal;

        private Point[] CreateRandomPointSet(int amount)
        {
            testPointSetRandom = new Point[amount];
            Random rand = new Random();
            for (int i = 0; i < amount; i++)
            {
                double x = rand.NextDouble();
                double y = rand.NextDouble();
                double z = rand.NextDouble();
                testPointSetRandom[i] = new Point(x, y, z);
            }

            return testPointSetRandom;
        }

        private Point[] CreateMinimalPointSet()
        {
            testPointSetMinimal = new Point[4];
            testPointSetMinimal[0] = new Point(0, 0, 0);
            testPointSetMinimal[1] = new Point(0, 1, 0);
            testPointSetMinimal[2] = new Point(0, 0, 1);
            testPointSetMinimal[3] = new Point(1, 1, 1);
            return testPointSetMinimal;
        }

        public Octree_Tests()
        {
            CreateMinimalPointSet();
            CreateRandomPointSet(100);
        }

        [TestMethod()]
        public void Octree_Test_PointsOnly()
        {
            Point[] pts = CreateRandomPointSet(100);
            Octree oct = new Octree(pts);
            Assert.IsTrue(oct.RootNode.IsLeaf, "Root node should be a leaf.");
            Assert.AreEqual(100, oct.RootNode.Points.Count);
        }

        [TestMethod()]
        public void Octree_Test_PointsAndNonLeafLayers()
        {
            Point[] pts = CreateRandomPointSet(1000);
            Octree oct = new Octree(pts, 1);
            Assert.IsFalse(oct.RootNode.IsLeaf, "Root node should not be a leaf.");
            Assert.AreEqual(null, oct.RootNode.Points);
            Assert.AreEqual(125, oct.RootNode.SubBoxes[0].Points.Count, 15,
                "Should be around 125. If not, try again!");
        }

        [TestMethod()]
        public void Octree_Test_PtsAndNLLsAndMaxLeafSize()
        {
            Point[] pts = CreateRandomPointSet(1000);
            Octree oct = new Octree(pts, 1, 10);
            List<OctreeBox> leafs = oct.GetLeafs(oct.RootNode);

            int sum = 0;
            foreach (OctreeBox box in leafs)
            {
                sum += box.Points.Count;
                Assert.IsFalse(box.Points.Count > 10,
                    "There should not be more than 10 points in a box!");
            }
            Assert.AreEqual(1000, sum);
        }

        [TestMethod()]
        public void GetLeafs_Test()
        {
            Point[] pts = CreateRandomPointSet(100);
            Octree oct = new Octree(pts);
            oct.RootNode.Subdivide();

            List<OctreeBox> leafs = oct.GetLeafs(oct.RootNode);
            Assert.AreEqual(8, leafs.Count);
            foreach (OctreeBox box in leafs)
            {
                Assert.IsTrue(box.IsLeaf);
            }
        }

        [TestMethod()]
        public void ClosestNeighbours_Test()
        {
            TestCloud2D set = new TestCloud2D();
            Octree oct;
            for (int i = 0; i < 5; i++)
            {
                oct = new Octree(set.Pts, i);

                Point[] closest3 = oct.ClosestNeighbours(set.Q3, 3).ToArray();
                Assert.AreEqual(3, closest3.Length, 0,
                    string.Format(
                         "Not the correct Amount: Exp: 3, Found: {0} (i = {1})",
                         closest3.Length, i)
                    );

                foreach (Point expected in set.ResultPoints_Q3_Closest3())
                {
                    if (!closest3.Contains(expected))
                    {
                        Assert.Fail(String.Format(
                            "{0} is one of the closest 3 around Q1, but was not found!",
                            expected));
                    }
                }
            }
        }

        [TestMethod()]
        public void ClosestNeigbour_Test()
        {
            TestCloud2D set = new TestCloud2D();
            Octree oct;
            for (int i = 0; i < 5; i++)
            {
                oct = new Octree(set.Pts, i);

                Point closest = oct.ClosestNeighbour(set.Q4);
                Assert.AreEqual(set.ResultPoints_Q4_Closest(), closest);
            }
        }

        [TestMethod()]
        public void PointsInRange_Test()
        {
            TestCloud2D set = new TestCloud2D();
            Octree oct = new Octree(set.Pts);
            Point[] inRange;

            // range 3 around Q1
            // =================

            inRange = oct.PointsInRange(set.Q1, 3).ToArray();

            // check for correct length
            Assert.AreEqual(set.ResultPoints_Q1_Range3().Length,
                inRange.Length);

            foreach (Point expected in set.ResultPoints_Q1_Range3())
            {
                if (!inRange.Contains(expected))
                {
                    Assert.Fail(
                        "{0} is in range of 3 around Q1, but it has not been found.",
                        expected);
                }
            }

            // range 5 around Q2
            // =================

            inRange = oct.PointsInRange(set.Q2, 5).ToArray();

            // check for correct length
            Assert.AreEqual(set.ResultPoints_Q2_Range5().Length,
                inRange.Length);

            foreach (Point expected in set.ResultPoints_Q2_Range5())
            {
                if (!inRange.Contains(expected))
                {
                    Assert.Fail(
                        "{0} is in range of 5 around Q2, but it has not been found.",
                        expected);
                }
            }
        }

        [TestMethod()]
        public void ClosestBoxes_Test()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void BoxesInRange_Test()
        {
            throw new NotImplementedException();
        }
    }
}