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
using System.Threading;

using TuryUtilCs.Mathmatics.Geometry;

namespace TuryUtilCs.PointStructures
{
    /// <summary>
    /// A kd-tree for organizing points in 3d space
    /// </summary>
    public class KDTree
    {
        /// <summary>
        /// Root node of the tree
        /// </summary>
        public KDTreeNode RootNode { get; set; }

        /// <summary>
        /// Protected constructor, create instances by using static methods!
        /// </summary>
        /// <param name="root">root node of the tree. => represents the tree</param>
        protected KDTree(KDTreeNode root)
        {
            RootNode = root;

            //turn decimal separator into points, which is needed for saving the tree
            System.Globalization.CultureInfo customCulture = (System.Globalization.
                CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        }

        /// <summary>
        /// Create a kd-tree from an array of points.
        /// </summary>
        /// <param name="pts">point set</param>
        /// <returns>kd-tree representing the point set</returns>
        public static KDTree CreateFromPointSet(Point[] pts)
        {
            List<Point> ptlist = pts.ToList();

            //// If the recursion depth is very, very, very deep (very unlikely)
            //KDTreeNode root = null;
            //int stacksize = 100000;
            //Thread t = new Thread(() => { root = BuildSubTreeFromCloud(ptlist, 0); }, stacksize);
            //t.Start();
            //t.Join();
            //return new KDTree(root);

            return new KDTree(BuildSubTreeFromCloud(ptlist, 0));
        }

        /// <summary>
        /// Read a kd-tree from a .kdt-file.
        /// </summary>
        /// <param name="file">file path</param>
        /// <returns>read tree</returns>
        public static KDTree LoadFromFile(string file)
        {
            //UNDONE Load KDTree from file
            throw new Exception("not implemented");
        }


        /// <summary>
        /// Writes a kd-tree to a .kdt-file.
        /// </summary>
        /// <param name="file">file path</param>
        public void SaveToFile(string file)
        {
            List<string> lines = new List<string>();
            lines.Add("node point_x point_y point_z left right");
            Traverse(ref lines, RootNode);

            System.IO.File.WriteAllLines(file, lines);

        }

        /// <summary>
        /// Traverses the tree and writes one line desribing each element
        /// to the list lines.
        /// This method is basically another ToString()-Method for writig
        /// the kd-tree to a file.
        /// </summary>
        /// <param name="lines">This one will be the output of the
        /// whole recursive traversation.</param>
        /// <param name="node"></param>
        private void Traverse(ref List<string> lines, KDTreeNode node)
        {
            string newline = node.Id + " " +
                node.Point.X + " " +
                node.Point.Y + " " +
                node.Point.Z + " ";

            if (node.Left != null)
            {
                newline += node.Left.Id + " ";
            }
            else
            {
                newline += "null ";
            }

            if (node.Right != null)
            {
                newline += node.Right.Id + " ";
            }
            else
            {
                newline += "null ";
            }

            lines.Add(newline);

            if (node.Left != null)
            {
                Traverse(ref lines, node.Left);
            }

            if (node.Right != null)
            {
                Traverse(ref lines, node.Right);
            }
        }

        /// <summary>
        /// Creates an kd-tree recursively from a point set.
        /// </summary>
        /// <param name="pts">point set</param>
        /// <param name="depth">current recursion depth</param>
        /// <returns>root node of the created tree</returns>
        private static KDTreeNode BuildSubTreeFromCloud(List<Point> pts, int depth)
        {
            // if empty array
            if (pts.Count == 0)
            {
                return null;
            }

            // locked to 3 dimensions
            int axis = depth % 3;

            // choose sort coordiante (greatest X, greatest Y, ...)
            pts.Sort(delegate (Point a, Point b)
            {
                return a[axis].CompareTo(b[axis]);
            });

            //  cut list into two halfes
            //  "uneven" = {0;1} => if pts.Count is uneven, e.g. pts = {a,b,c}
            //  median = 3/2 = 1 => left = range(0,1) = {a} and right = range{1,1+1}
            //      = range(1,2) = {b,c}
            int median = pts.Count / 2;
            int uneven = (pts.Count % 2) - 1;
            List<Point> lefthalf, righthalf;
            lefthalf = pts.GetRange(0, median);
            righthalf = pts.GetRange(median + 1, median + uneven);

            List<Point> leftList = lefthalf.ToList<Point>();
            List<Point> rightList = righthalf.ToList<Point>();

            // search recursively
            return new KDTreeNode(pts[median],
                BuildSubTreeFromCloud(leftList, depth + 1),
                BuildSubTreeFromCloud(rightList, depth + 1));
        }


        /// <summary>
        /// Find all points within a defined range around a point.
        /// </summary>
        /// <param name="query">point</param>
        /// <param name="radius">range</param>
        /// <param name="node">node currently in progress -> necessary for recursion.
        /// (Don't set this value, it will be set automatically during the recursion.)</param>
        /// <param name="depth">current recursion depth
        /// (Don't set this value, it will be set automatically during the recursion.)</param>
        /// <returns>List of all points within the radius</returns>
        public List<Point> FindWithinRadius(Point query, double radius,
            KDTreeNode node = null, int depth = -1)
        {
            List<Point> neighbours = new List<Point>();

            // depth == -1 means that this is the top level call of this method
            if (depth == -1)
            {
                depth = 0;
                node = RootNode;
            }

            // if node == null, the parent node is either a leaf or at least not
            // a fork
            if (node == null)
            {
                return neighbours;
            }

            // Locked to 3 axis
            int axis = depth % 3;

            // if this node is within the range, add it!
            if (query.SquareDistanceTo(node.Point) <= System.Math.Pow(radius, 2))
            {
                neighbours.Add(node.Point);
            }

            // in case of both-side-search:
            // rememder, which side has already been searched => alreadySearchedRight
            bool alreadySearchedRight = true;
            if (query[axis] < node.Point[axis])
            {
                neighbours = neighbours.Concat(
                    FindWithinRadius(query, radius, node.Left, depth + 1)).ToList();
                alreadySearchedRight = false;
            }
            else
            {
                neighbours = neighbours.Concat(
                    FindWithinRadius(query, radius, node.Right, depth + 1)).ToList();
            }

            // case mentioned above:
            // both subtrees have to be searched; using alreadySearchedRight for
            // chosing which side to search next.
            if (System.Math.Abs(query[axis] - node.Point[axis]) < radius)
            {
                if (alreadySearchedRight)
                {
                    neighbours = neighbours.Concat(
                        FindWithinRadius(query, radius, node.Left, depth + 1)).ToList();
                }
                else
                {
                    neighbours = neighbours.Concat(
                        FindWithinRadius(query, radius, node.Right, depth + 1)).ToList();
                }
            }
            return neighbours;


        }


        /// <summary>
        /// Recursive search for neighbours.
        /// </summary>
        /// <param name="query">center of the search</param>
        /// <param name="node">current checked node</param>
        /// <param name="depth">current recursion depth</param>
        /// <param name="neighbours">KDTreeNeighbours-object, collecting results of nm-search</param>
        private void NMSearch(Point query,
            KDTreeNode node, int depth, NeigbourCollector neighbours)
        {
            // The NM-search does only look for potential neighbours. The
            // final decision if this node is to be returned or not will
            // be done by the KDTreeNeighbours-object

            // If there is no node, return.
            if (node == null)
            {
                return;
            }

            // If the node is a leaf, it will always be added to the neighbours-list
            if (node.IsLeaf())
            {
                neighbours.AddNeighbour(node.Point);
                return;
            }

            // locked to 3 axis
            int axis = depth % 3;

            // One subtree usually lies closer to the query point along the currently
            // viewed axis. This one is the close-tree. The other one is the far-tree.
            KDTreeNode farSubTree, closeSubTree;
            if (query[axis] < node.Point[axis])
            {
                farSubTree = node.Right;
                closeSubTree = node.Left;
            }
            else
            {
                farSubTree = node.Left;
                closeSubTree = node.Right;
            }

            // Search the close-tree first
            NMSearch(query, closeSubTree, depth + 1, neighbours);

            // Since the current point was not sortet out by the
            // previous recursion level, it needs to be added to
            // the neighbour list.
            neighbours.AddNeighbour(node.Point);

            // If the current 'boundary' lies closer to the query
            // point than the farest approved neighbour so far, 
            // the other side of the boundary also needs to be
            // searched.
            if (System.Math.Pow(node.Point[axis] -
                query[axis], 2) < neighbours.LargestSquareDistance)
            {
                NMSearch(query, farSubTree, depth + 1, neighbours);
            }

            return;

        }

        /// <summary>
        /// Returns the n closest neighbours to a query point.
        /// </summary>
        /// <param name="query">query point</param>
        /// <param name="amount">n, the amount of desired neighbours</param>
        /// <returns>The n closest neighbours to query</returns>
        public List<Point> FindClosestNeighbours(Point query, int amount)
        {
            // This method is just a "wrapper". The real search will be
            // done by NMSearch().
            // This approach was necessary to define and return the
            // neighbours list.

            NeigbourCollector neighbours = new NeigbourCollector(query, amount);
            NMSearch(query, RootNode, 0, neighbours);
            return neighbours.BestNeighbours;
        }

        /// <summary>
        /// Finds the closest neighbour to query
        /// </summary>
        /// <param name="query">query point</param>
        /// <returns>The closest neighbour to query</returns>
        public Point FindClosestNeighbour(Point query)
        {
            return FindClosestNeighbours(query, 1)[0];
        }



    }
}

