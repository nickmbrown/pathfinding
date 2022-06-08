using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    private void Awake() 
    {
        grid = GetComponent<Grid>();
    }

    private void Update() 
    {
        FindPath(seeker.position, target.position);
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node _startNode = grid.GetNodeFromWorldPosition(startPos);
        Node _targetNode = grid.GetNodeFromWorldPosition(targetPos);

        List<Node> _openSet = new List<Node>();
        HashSet<Node> _closedSet = new HashSet<Node>();

        _openSet.Add(_startNode);

        while(_openSet.Count > 0)
        {
            Node _node = _openSet[0];
            for (int i = 1; i < _openSet.Count; i++)
            {
                if(_openSet[i].fCost < _node.fCost || _openSet[i].fCost == _node.fCost)
                {
                    if( _openSet[i].hCost < _node.hCost)
                        _node = _openSet[i];
                }
            }

            _openSet.Remove(_node);
            _closedSet.Add(_node);

            if(_node == _targetNode)
            {
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
        Debug.Log("retrace");
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
