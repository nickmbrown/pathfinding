using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;
    Heap<Node> _openSet;
    HashSet<Node> _closedSet;

    private void Awake() 
    {
        grid = GetComponent<Grid>();

    }
    
    private void Start() 
    {
        _openSet = new Heap<Node>(grid.MaxSize);
        _closedSet = new HashSet<Node>();
    }

    private void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

            FindPath(seeker.position, target.position);
        }

        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        Node _startNode = grid.GetNodeFromWorldPosition(startPos);
        Node _targetNode = grid.GetNodeFromWorldPosition(targetPos);

        _openSet.Clear();
       _closedSet.Clear();

        _openSet.Add(_startNode);

        while(_openSet.Count > 0)
        {
            Node _node = _openSet.RemoveFirst();
            _closedSet.Add(_node);

            if(_node == _targetNode)
            {
                sw.Stop();
                print("Path found: " + sw.ElapsedMilliseconds + "ms");
                RetracePath(_startNode, _targetNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbors(_node))
            {
                if(!neighbor.isWalkable || _closedSet.Contains(neighbor)) 
                    continue;

                int _newCostToNeighbor = _node.gCost + GetDistance(_node, neighbor);
                if(_newCostToNeighbor < neighbor.gCost || !_openSet.Contains(neighbor))
                {
                    neighbor.gCost = _newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, _targetNode);
                    neighbor.parent = _node;

                    if(!_openSet.Contains(neighbor))
                    {
                        _openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> _path = new List<Node>();
        Node _currentNode = endNode;

        while(_currentNode != startNode)
        {
            _path.Add(_currentNode);
            _currentNode = _currentNode.parent;
        }

        _path.Reverse();

        grid.path = _path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int _distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int _distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(_distanceX > _distanceY)
        {
            return 14 * _distanceY  + 10 * (_distanceX - _distanceY);
        }

        return 14 * _distanceX  + 10 * (_distanceY - _distanceX);
        
    }
}
