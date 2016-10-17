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


namespace TuryUtilCs.PointStructures
{
    /// <summary>
    /// one node of a kd-tree
    /// </summary>
    public class KDTreeNode
    {
        //UNDONE Treenodecount resetten für jeden neuen tree
        /// <summary>
        /// actual number of nodes in the tree. 
        /// each node has one id-number, which
        /// is calculated with this counter
        /// </summary>
        private static int TreeNodeCount= 0;

        /// <summary>
        /// id of the node in the tree. for identification
        /// purposes
        /// </summary>
        public int Id{get; set;}

        /// <summary>
        /// the point described by the node
        /// </summary>
        public TuryUtilCs.Mathmatics.Geometry.Point Point { get; set; }

        /// <summary>
        /// the left child node of this node
        /// </summary>
        public KDTreeNode Left { get; set; }
        
        /// <summary>
        /// the right child node of this node
        /// </summary>
        public KDTreeNode Right { get; set; }

        /// <summary>
        /// creates a new kd-tree node
        /// </summary>
        /// <param name="point">point that shall be described</param>
        /// <param name="left">left child node</param>
        /// <param name="right">right child node</param>
        public KDTreeNode(TuryUtilCs.Mathmatics.Geometry.Point point, KDTreeNode left, KDTreeNode right)
        {
            this.Point = point;
            this.Left = left;
            this.Right = right;

            this.Id = TreeNodeCount;
            TreeNodeCount++;
        }

        /// <summary>
        /// checks, if this node is a leaf
        /// </summary>
        /// <returns>true, if the node has no children => node is a leaf then</returns>
        public Boolean IsLeaf()
        {
            return (Left == null && Right == null);
        }
    }
}
