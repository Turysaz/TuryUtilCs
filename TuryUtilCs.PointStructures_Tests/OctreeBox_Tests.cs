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
    public class OctreeBox_Tests
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

        public OctreeBox_Tests()
        {
            CreateRandomPointSet(100);
        }

        [TestMethod()]
        public void OctreeBox_Test()
        {
            Point[] pts = CreateRandomPointSet(100);
            OctreeBox testBox;

            testBox = new OctreeBox(pts.ToList(), 0);
            Assert.IsTrue(testBox.IsLeaf, "Box should be a leaf!");

            testBox = new OctreeBox(pts.ToList(), 3);

            // Leafs
            Assert.IsFalse(testBox.IsLeaf, "Box should not be a leaf!");
            Assert.IsFalse(testBox.SubBoxes[0].IsLeaf, "Subbox should not be a leaf!");
            Assert.IsFalse(testBox.SubBoxes[0].SubBoxes[0].IsLeaf,
                "Sub-Subbox should not be a leaf!");
            Assert.IsTrue(testBox.SubBoxes[0].SubBoxes[0].
                SubBoxes[0].IsLeaf, "Sub-Sub-Subbox should be a leaf!");

            Assert.AreEqual(8, testBox.SubBoxes.Length);
        }

        [TestMethod()]
        public void Subdivide_Test()
        {
            Point[] pts = CreateRandomPointSet(100);
            OctreeBox testBox;

            // 

            testBox = new OctreeBox(pts.ToList(),0);
            Assert.AreEqual(100, testBox.Points.Count());
            testBox.Subdivide();
            int sum = 0;
            foreach(OctreeBox box in testBox.SubBoxes)
            {
                foreach (Point p in box.Points)
                {
                    sum++;
                    if (!box.PointWithin(p))
                    {
                        Assert.Fail(String.Format("The Point {0} does not lie within box {1}!",
                            p.ToString(), box.ToString()));
                    }
                }
            }
            Assert.AreEqual(100, sum, 0, "Obviously not all points have been put to one box!");

            // boundaries
            testBox = new OctreeBox(pts.ToList(), 5);

            OctreeBox current = testBox;
            while (!current.IsLeaf)
            {
                double widthX = current.Xmax - current.Xmin;
                double widthY = current.Ymax - current.Ymin;
                double widthZ = current.Zmax - current.Zmin;

                foreach (OctreeBox box in current.SubBoxes)
                {
                    Assert.IsTrue(box.Xmax > box.Xmin, "Max should be greater than min!");
                    Assert.IsTrue(box.Ymax > box.Ymin, "Max should be greater than min!");
                    Assert.IsTrue(box.Zmax > box.Zmin, "Max should be greater than min!");

                    Assert.AreEqual((box.Xmax - box.Xmin) / 2 + box.Xmin, box.Xmean);
                    Assert.AreEqual((box.Ymax - box.Ymin) / 2 + box.Ymin, box.Ymean);
                    Assert.AreEqual((box.Zmax - box.Zmin) / 2 + box.Zmin, box.Zmean);

                    Assert.AreEqual(widthX / 2, box.Xmax - box.Xmin, 0.0000000000001);
                    Assert.AreEqual(widthY / 2, box.Ymax - box.Ymin, 0.0000000000001);
                    Assert.AreEqual(widthZ / 2, box.Zmax - box.Zmin, 0.0000000000001);
                }

                current = current.SubBoxes[0];
            }
        }

        [TestMethod()]
        public void PointWithin_Test()
        {
            Point[] testPoints = CreateMinimalPointSet();

            Point a = new Point(0.5, 0.5, 0.5);
            Point b = new Point(-2, 1, 0.5);

            OctreeBox box = new OctreeBox(testPoints.ToList(), 0);
            Assert.IsTrue(box.PointWithin(a));
            Assert.IsFalse(box.PointWithin(b));
        }

        [TestMethod()]
        public void MinimumSquareDistanceTo_Test()
        {
            Point[] testPoints = CreateMinimalPointSet();

            Point a = new Point(0.5, 0.5, 0.5);
            Point b = new Point(-2, 1, 0.5);
            Point c = new Point(3, 1, 0);
            Point d = new Point(1, 17, 0);

            OctreeBox box = new OctreeBox(testPoints.ToList(), 0);

            Assert.AreEqual(0, box.MinimumDistanceTo(a));
            Assert.AreEqual(2, box.MinimumDistanceTo(b));
            Assert.AreEqual(2, box.MinimumDistanceTo(c));
            Assert.AreEqual(16, box.MinimumDistanceTo(d));
        }
    }
}