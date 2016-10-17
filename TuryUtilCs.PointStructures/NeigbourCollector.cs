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
    /// Internal class that is used to store the result of the closest-neighbour-search.
    /// </summary>
    internal class NeigbourCollector
    {

        /// <summary>
        /// Query point: The center around that the neighbours
        /// will be searched.
        /// </summary>
        private Point _queryPoint;

        /// <summary>
        /// Best found neighbours so far
        /// </summary>
        private List<Point> _bestNeighbours;

        /// <summary>
        /// Maximal amount of neighbours to find.
        /// </summary>
        private int _maxAmount;

        /// <summary>
        /// Distance between the farest point to query
        /// and query itself.
        /// So: the maximal distance between any point in 
        /// _bestNeighbours and query.
        /// </summary>
        private double _largestSqDistance;

        /// <summary>
        /// List of closest found neighbours
        /// </summary>
        public List<Point> BestNeighbours
        {
            get { return _bestNeighbours; }
        }

        /// <summary>
        /// Distance between the farest point to query
        /// and query itself.
        /// So: the maximal distance between any point in 
        /// _bestNeighbours and query.
        /// </summary>
        public double LargestSquareDistance
        {
            get { return _largestSqDistance; }
        }

        /// <summary>
        /// Constructor of KDTreeNeighbours-objects
        /// </summary>
        /// <param name="query">point that's neighbours' shall be found.</param>
        /// <param name="amount">maximal amount of neighbours to return</param>
        public NeigbourCollector(Point query, int amount)
        {
            _maxAmount = amount;
            _queryPoint = query;
            _largestSqDistance = 0;
            _bestNeighbours = new List<Point>();
        }

        /// <summary>
        /// Checks if a potential neighbour shall be added to the current neighbours
        /// list. If the neighbours list is not full yet or if the potential neighbour
        /// is closer to the query point than at least one of the already added neighbours, 
        /// it will be added to the list.
        /// If the maximal amount of list entries was alread reached before that, the last
        /// (worst) entry of the list will be removed.
        /// </summary>
        /// <param name="p">potential neighbour of query</param>
        public void AddNeighbour(Point p)
        {
            double currentSquareDist = p.SquareDistanceTo(_queryPoint);

            // If list is full and distance to query is worse than 
            // the worst accepted, this point will be ignored.
            if (currentSquareDist >= _largestSqDistance &&
                _maxAmount == _bestNeighbours.Count)
            {
                return;
            }

            // Check, if p is closer to any of the points in the list.
            for (int i = 0; i < _bestNeighbours.Count; i++)
            {
                if (currentSquareDist < _bestNeighbours[i].SquareDistanceTo(_queryPoint))
                {
                    _bestNeighbours.Insert(i, p);

                    //remove last element, if list is full
                    if (_bestNeighbours.Count > _maxAmount)
                    {
                        _bestNeighbours.Remove(_bestNeighbours.Last());
                    }

                    // Update largest square distance to the new last
                    // point in the list.
                    _largestSqDistance = _bestNeighbours.Last().
                        SquareDistanceTo(_queryPoint);
                    return;
                }
            }

            // if the point was not inserted, it will be appended
            //OPTIMIERBAR: this could easily be checked BEFORE the
            // iteration above...
            _bestNeighbours.Add(p);
            _largestSqDistance = currentSquareDist;
        }
    }
}
