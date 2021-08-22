///<summary>
///Copyright 2021 Jussi Mattila a.k.a John Rudy Permission is hereby granted, free of charge, to any person 
///obtaining a copy of this software and associated documentation files (the "Software"), to deal in the 
///Software without restriction, including without limitation the rights to use, copy, modify, merge, 
///publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the 
///Software is furnished to do so, subject to the following conditions: The above copyright notice and this 
///permission notice shall be included in all copies or substantial portions of the Software. 
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
/// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
/// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
/// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE 
/// USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>

/// Slightly inefficient
/// Results are consistent. 
/// Fine for smaller projects. 


using Quicksort;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AStarPathfinding : AStarNodeGrid {
    QSList<AStarNode> open;
    HashSet<AStarNode> closed;
    List<AStarNode> path;
    AStarNode c_Node;
    AStarNode s_Node;
    AStarNode e_Node;
    Stopwatch sw;

    //Calling this method results in a path if a path can be found. 
    public List<AStarNode> Path ( Vector3 _start, Vector3 _end ) {
        //Performance testing
        sw = new Stopwatch();
        sw.Start();

        path = new List<AStarNode>();
        open = new QSList<AStarNode>();
        closed = new HashSet<AStarNode>();

        //Assinging start and end nodes. 
        StartAndEndNode(_start, _end);

        //Astar loop
        AStar();

        if( c_Node == e_Node ) { Recursion(); }
        else { print("No path"); return null; }

        sw.Stop();
        print("Path found in" + sw.ElapsedMilliseconds + " ms");
        return path;
    }

    void AStar () {
        while( open.Count > 0 ) {
            if( c_Node == e_Node ) { break; }
            else {
                //Lowest fCost node is the first. 
                c_Node = open.RemoveFirst();
                closed.Add(c_Node);

                List<AStarNode> neighbours = Neighbours();

                foreach( AStarNode node in neighbours ) {
                    if( !node.occupied && !closed.Contains(node) ) {
                        int newMoveCost = c_Node.gCost + CellCost(c_Node, node);
                        if( newMoveCost < node.gCost || !open.Contains(node) ) {
                            node.gCost = newMoveCost;
                            node.hCost = CellCost(node, e_Node);
                            node.parent = c_Node;
                            if( !open.Contains(node) ) {
                                open.Add(node);
                            }
                        }
                    }
                }
                //Sorts nodes lowest fCost on top. 
                open.Sort();
            }
        }
    }

    void Recursion () {
        while( c_Node != s_Node ) {
            path.Insert(0, c_Node);
            c_Node = path [ 0 ].parent;
        }
    }

    //Needs a better handling method. Too many else if for my taste
    List<AStarNode> Neighbours () {
        List<AStarNode> neighbours = new List<AStarNode>();
        AStarNode additionNode = null;

        foreach( AStarNode node in nodeGrid ) {
            if( node.x == c_Node.x + 1 && node.y == c_Node.y ) {
                additionNode = node;
            }
            else if( node.x == c_Node.x - 1 && node.y == c_Node.y ) {
                additionNode = node;
            }
            else if( node.x == c_Node.x && node.y == c_Node.y - 1 ) {
                additionNode = node;
            }
            else if( node.x == c_Node.x && node.y == c_Node.y + 1 ) {
                additionNode = node;
            }

            //8 directions
            if( dianogal ) {
                if( node.x == c_Node.x + 1 && node.y == c_Node.y + 1 ) {
                    additionNode = node;
                }
                else if( node.x == c_Node.x + 1 && node.y == c_Node.y - 1 ) {
                    additionNode = node;
                }
                else if( node.x == c_Node.x - 1 && node.y == c_Node.y - 1 ) {
                    additionNode = node;
                }
                else if( node.x == c_Node.x - 1 && node.y == c_Node.y + 1 ) {
                    additionNode = node;
                }
            }

            if( additionNode != null && !neighbours.Contains(additionNode) && !additionNode.occupied ) {
                neighbours.Add(additionNode);
            }
        }

        return neighbours;
    }

    //A slightly inefficient way of doing this 
    void StartAndEndNode ( Vector3 _start, Vector3 _end ) {
        //To start of with, assinging start and end to be first node. 
        s_Node = nodeGrid [ 0 ];
        e_Node = nodeGrid [ 0 ];

        for( int i = 0 ; i < nodeGrid.Count ; i++ ) {
            //Setting start node 
            float dist_a1 = Vector3.Distance(s_Node.worldPos, _start);
            float dist_b1 = Vector3.Distance(nodeGrid [ i ].worldPos, _start);

            if( dist_a1 > dist_b1 && !nodeGrid [ i ].occupied ) {
                s_Node = nodeGrid [ i ];
            }

            //Setting end node
            float dist_a2 = Vector3.Distance(e_Node.worldPos, _end);
            float dist_b2 = Vector3.Distance(nodeGrid [ i ].worldPos, _end);

            if( dist_a2 > dist_b2 && !nodeGrid [ i ].occupied ) {
                e_Node = nodeGrid [ i ];
            }
        }
        open.Add(s_Node);
    }


    //TODO: LOOK INTO OTHER HEURISTIC METHODS
    //Currently the gCost explodes in size. 
    //Taxicab / Manhattan heuristic?
    public int CellCost ( AStarNode nodeA, AStarNode nodeB ) {
        int distXg = Mathf.Abs(nodeA.x - nodeB.x);
        int distYg = Mathf.Abs(nodeA.y - nodeB.y);

        int cost;

        if( distXg > distYg ) {
            cost = 14 * distYg + 10 * ( distXg - distYg ) + nodeA.gCost;
        }
        else {
            cost = 14 * distXg + 10 * ( distYg - distXg ) + nodeA.gCost;
        }

        return cost;
    }
}