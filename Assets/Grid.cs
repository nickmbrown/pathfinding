using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float _nodeDiameter;
    int _gridSizeX, _gridSizeY;

    private void Start() 
    {
        _nodeDiameter = nodeRadius*2;
        _gridSizeX = Mathf.RoundToInt(gridWorldSize.x/_nodeDiameter);
        _gridSizeY = Mathf.RoundToInt(gridWorldSize.y/_nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[_gridSizeX, _gridSizeY];
        Vector3 _worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

        for(int x = 0; x < _gridSizeX; x++)
        {
            for(int y = 0; y < _gridSizeY; y++)
            {
                Vector3 _worldPos = _worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.forward * (y * _nodeDiameter + nodeRadius); 
                bool _isWalkable = !(Physics.CheckSphere(_worldPos, nodeRadius, unwalkableMask));
                grid[x,y] = new Node(_isWalkable,_worldPos, x, y);
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        float _percentX = Mathf.Clamp01((worldPosition.x / gridWorldSize.x) + 0.5f);
        float _percentY = Mathf.Clamp01((worldPosition.z / gridWorldSize.y) + 0.5f);

        int x  = Mathf.RoundToInt((_gridSizeX - 1) * _percentX);
        int y  = Mathf.RoundToInt((_gridSizeY - 1) * _percentY);

        return grid[x,y];
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> _neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkX < _gridSizeX && checkY >= 0 && checkY < _gridSizeY)
                {
                    _neighbors.Add(grid[checkX,checkY]);
                }
            }
        }

        return _neighbors;
    }

    public List<Node> path;

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid != null)
        {
            foreach (var node in grid)
            {
                Gizmos.color = node.isWalkable ? Color.white : Color.red;

                if(path != null) 
                {
                    Debug.Log(path.Count);
                    if(path.Contains(node))
                    {
                        Gizmos.color = Color.blue;
                    }
                }

                Gizmos.DrawCube(node.worldPosition, Vector3.one * (_nodeDiameter -0.1f));
            }
        }
    }
}
