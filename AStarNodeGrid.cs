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
/// 

// Setting up the node grid for the stage.


using System.Collections.Generic;
using UnityEngine;

public class AStarNodeGrid : MonoBehaviour {
    //List of all nodes created
    protected List<AStarNode> nodeGrid;

    //Used for centering the plane from attached transform
    Transform gridPlaneParent;

    //Center of grid
    Vector3 gridCenter;
    [Header("Node grid settings")]
    [Tooltip("Size of the grid plane")] public Vector2 gridSize;
    [Tooltip("Resolution of grid")] public int resolution;

    //A single boxcollider for raycasting the size of the grid
    BoxCollider bCollider;

    [Tooltip("Which layers are considered obstacles")] public LayerMask obstacleMask;
    [Tooltip("Diagonal movement")] public bool dianogal;


    [SerializeField] bool showGrid;

    //Sanity check and in editor visualisation
    private void OnValidate () {
        if( resolution <= 0 ) { resolution = 1; }
        if( gridSize.x <= 0 ) { gridSize.x = 1; }
        if( gridSize.y <= 0 ) { gridSize.y = 1; }

        SetNodeGrid();
    }

    //Degub
    private void OnDrawGizmos () {
        if( Application.isPlaying && showGrid ) {
            foreach( AStarNode node in nodeGrid ) {
                if( !node.occupied ) {
                    Gizmos.DrawWireCube(node.worldPos, node.size);
                }
            }
        }
    }

    private void Awake () {
        bCollider = transform.gameObject.AddComponent<BoxCollider>();
        SetNodeGrid();
    }

    void SetNodeGrid () {
        nodeGrid = new List<AStarNode>();

        //Setting transforms and positions
        if( gridPlaneParent == null ) {
            gridPlaneParent = transform;
        }

        gridCenter = gridPlaneParent.position;

        float nodeSizeX = gridSize.x / resolution;
        float nodeSizeY = gridSize.y / resolution;

        //Creating nodes with occupancy validation
        for( int i = 0 ; i < resolution ; i++ ) {
            for( int j = 0 ; j < resolution ; j++ ) {

                //Taking center of grid into account 
                float center_X = gridCenter.x + ( nodeSizeX * i ) - ( gridSize.x / 2 ) + ( nodeSizeX / 2 );
                float center_Z = gridCenter.z + ( nodeSizeY * j ) - ( gridSize.y / 2 ) + ( nodeSizeY / 2 );
                float center_Y = gridCenter.y;

                Vector3 _worldPos = new Vector3(center_X, center_Y, center_Z);

                //Checking if a node position is valid with a checkbox
                if( !Physics.CheckBox(_worldPos + Vector3.up, new Vector3(nodeSizeX / 2, 1, nodeSizeY / 2), Quaternion.identity, obstacleMask) ) {
                    nodeGrid.Add(new AStarNode(i, j, false, _worldPos, new Vector3(nodeSizeX, 0.2f, nodeSizeY)));
                }
                else {
                    nodeGrid.Add(new AStarNode(i, j, true, _worldPos, new Vector3(nodeSizeX, 0.2f, nodeSizeY)));
                }
            }
        }

        if( bCollider != null ) {
            bCollider = GetComponent<BoxCollider>();
            bCollider.center = gridCenter;
            bCollider.size = new Vector3(gridSize.x, 0.05f, gridSize.y);
        }
    }
}
