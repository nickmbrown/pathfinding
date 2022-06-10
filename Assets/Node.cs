using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool isWalkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    public Node parent;
    int _heapIndex;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public string GetNodeInfo()
    {
        return $"{gridX}, {gridY}";
    }

    public int CompareTo(Node other)
    {
        int _compare = fCost.CompareTo(other.fCost);
        if(_compare == 0)
        {
            _compare = hCost.CompareTo(other.hCost);
        }
        return -_compare;
    }

    public int fCost 
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int HeapIndex
    {
        get
        {
            return _heapIndex;
        }
        set
        {
            _heapIndex = value;
        }
    }
}
