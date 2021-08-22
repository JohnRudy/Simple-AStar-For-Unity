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

//Object class for nodes

using Quicksort;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : QSItem<AStarNode> {
    //Used for cellcosts
    public int x;
    public int y;

    //Used for world positioning
    public Vector3 worldPos;
    public bool occupied;

    //For recursion
    public AStarNode parent;

    //Values of A Star
    public int gCost;
    public int hCost;

    //Node grid sizing
    public Vector3 size;

    public List<AStarNode> neighbours;

    public AStarNode ( int _x, int _y, bool _occupied, Vector3 _worldPos, Vector3 _size ) {
        x = _x;
        y = _y;

        worldPos = _worldPos;
        occupied = _occupied;

        size = _size;
    }

    int fCost;
    public int FCost {
        get {
            return gCost + hCost;
        }
        set {
            if( !fCost.Equals(value) ) {
                fCost = value;
            }
        }
    }

    public int CompareTo ( AStarNode itemToCompare ) {
        int compare = fCost.CompareTo(itemToCompare.fCost);
        return compare;
    }
}