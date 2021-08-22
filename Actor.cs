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


//Example actor class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actor : MonoBehaviour {
    AStarPathfinding pathfind;
    bool ismoving = false;

    public LayerMask pathfinding;

    //debug
    List<Vector3> path;

    private void OnDrawGizmos () {
        if( Application.isPlaying && path != null ) {
            foreach( Vector3 pos in path ) {
                Gizmos.DrawWireSphere(pos, 0.2f);
            }
        }
    }

    private void OnEnable () {
        pathfind = FindObjectOfType<AStarPathfinding>();
    }

    private void Update () {

        if( Input.GetMouseButton(0) ) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if( !ismoving ) {
                Physics.Raycast(ray, out hit, Mathf.Infinity,pathfinding);
                print(hit.point);
                StartCoroutine(MoveActor(pathfind.Path(transform.position, hit.point)));
            }
        }

    }

    //For example recursively calling this method to move to the next point in the list
    IEnumerator MoveActor ( List<AStarNode> move ) {
        ismoving = true;

        path = new List<Vector3>();

        if( move != null ) {
            
            //debugging
            for( int i = 0 ; i < move.Count ; i++ ) {
                path.Insert(i, move [ i ].worldPos);
            }

            yield return new WaitForSeconds(0.2f);

            transform.position = move [ 0 ].worldPos;
            move.RemoveAt(0);
            if( move.Count > 0 ) {
                StartCoroutine(MoveActor(move));
            }
            else {
                ismoving = false;
            }
        }
        else {
            ismoving = false;
        }
    }
}