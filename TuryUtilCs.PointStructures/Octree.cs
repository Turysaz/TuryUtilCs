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
    public class Octree
    {
        //DOKU
        private OctreeBox _rootNode;

        //DOKU
        public OctreeBox RootNode
        {
            get { return _rootNode; }
        }

        //DOKU
        public Octree(Point[] pts) : this(pts, 0, -1) { }

        //DOKU
        public Octree(Point[] pts, int nonLeafLayers)
            : this(pts, nonLeafLayers, -1) { }

        //DOKU
        public Octree(Point[] pts, int nonLeafLayers, int maxPointsPerBox)
        {
            if (maxPointsPerBox == 0)
            {
                throw new ArgumentException(
                    "An octree with the maximul leaf size 0 is impossible!");
            }

            _rootNode = new OctreeBox(pts.ToList(), nonLeafLayers);

            // If there is no point cap, the tree is done here.
            if (maxPointsPerBox < 0) { return; }

            // Get all the leafs to decide if a subdivision is
            // necessary or not
            List<OctreeBox> leafs = GetLeafs(_rootNode);

            // Divide all leafs into smaller boxes.
            // If a box has too many points, add it to
            //  the list of boxes that shall be divided.
            int i = 0;
            while (i < leafs.Count)
            {
                OctreeBox box = leafs[i];
                if (box.Points.Count <= maxPointsPerBox)
                {
                    i++;
                    continue;
                }

                leafs.RemoveAt(i);
                leafs.AddRange(box.Subdivide());
            }
        }

        //DOKU
        public List<OctreeBox> GetLeafs()
        {
            return GetLeafs(this.RootNode);
        }

        //DOKU
        public List<OctreeBox> GetLeafs(OctreeBox parent)
        {
            List<OctreeBox> leafs = new List<OctreeBox>();
            if (parent.IsLeaf)
            {
                leafs.Add(parent);
                return leafs;
            }
            foreach (OctreeBox child in parent.SubBoxes)
            {
                leafs.AddRange(GetLeafs(child));
            }
            return leafs;
        }

        //DOKU
        public List<Point> ClosestNeighbours(Point query, int amount)
        {
            NeigbourCollector neighbours = new NeigbourCollector(query, amount);
            TraverseSearch(query, RootNode, neighbours);
            return neighbours.BestNeighbours;
        }

        //DOKU
        public Point ClosestNeighbour(Point p)
        {
            return ClosestNeighbours(p, 1)[0];
        }

        //DOKU
        private void TraverseSearch(Point query,
            OctreeBox node, NeigbourCollector neighbours)
        {
            if (node.IsLeaf)
            {
                foreach (Point p in node.Points)
                {
                    neighbours.AddNeighbour(p);
                }
                return;
            }

            // not leaf
            foreach (OctreeBox subNode in node.SubBoxes)
            {
                double curMinSDist = subNode.MinimumSquareDistanceTo(query);
                //TODO that shit about the Neighbourlist count
                // is really, really crappy!
                if (neighbours.BestNeighbours.Count > 0
                    && curMinSDist > neighbours.LargestSquareDistance)
                {
                    continue;
                }

                TraverseSearch(query, subNode, neighbours);
            }
            return;
        }

        //DOKU
        public List<Point> PointsInRange(Point query, double range)
        {
            double squareRange = range * range;

            List<OctreeBox> bir = BoxesInRange(query, range);
            List<Point> pir = new List<Point>();

            foreach (OctreeBox currentBox in bir)
            {
                foreach (Point currentPoint in currentBox.Points)
                {
                    if (currentPoint.SquareDistanceTo(query) <= squareRange)
                    {
                        pir.Add(currentPoint);
                    }
                }
            }

            return pir;
        }

        //DOKU
        public List<OctreeBox> ClosestBoxes(Point query, int amount)
        {
            // HACK Octree.ClosestBoxes() -> whole method...
            // HOT TEST!!!! 

            List<OctreeBox> allLeafs = this.GetLeafs();
            Dictionary<OctreeBox, double> allDists =
                new Dictionary<OctreeBox, double>();

            foreach (OctreeBox box in allLeafs)
            {
                allDists.Add(box, box.MinimumDistanceTo(query));
            }

            allLeafs.Sort(delegate (OctreeBox b1, OctreeBox b2)
            {
                return (allDists[b1].CompareTo(allDists[b2]));
            });

            return allLeafs.GetRange(0, amount);
        }

        //DOKU
        public List<OctreeBox> BoxesInRange(Point query, double range)
        {
            return BoxesInRange(query, range * range, RootNode);
        }

        //DOKU
        private List<OctreeBox> BoxesInRange(Point query, double squareRange,
            OctreeBox node)
        {
            List<OctreeBox> fittingSubBoxes = new List<OctreeBox>();

            if (node.IsLeaf)
            {
                fittingSubBoxes.Add(node);
            }
            else
            {
                foreach (OctreeBox box in node.SubBoxes)
                {
                    // skip nodes that are too far away
                    if (box.MinimumSquareDistanceTo(query) > squareRange)
                    {
                        continue;
                    }

                    if (box.IsLeaf)
                    {
                        fittingSubBoxes.Add(box);
                    }
                    else
                    {
                        fittingSubBoxes.AddRange(
                            BoxesInRange(query, squareRange, box));
                    }


                }
            }

            return fittingSubBoxes;
        }
    }
}
