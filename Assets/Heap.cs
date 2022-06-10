using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T: IHeapItem<T>
{
    T[] _items;
    int _currentItemCount;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = _currentItemCount;
        _items[_currentItemCount] = item;
        SortUp(item);
        _currentItemCount++;
    }

    public T RemoveFirst()
    {
        T _firstItem = _items[0];
        _currentItemCount--;
        _items[0] = _items[_currentItemCount];
        _items[0].HeapIndex = 0;

        SortDown(_items[0]);
        return _firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count 
    {
        get 
        {
            return _currentItemCount;
        }
    }

    public void Clear()
    {
        _currentItemCount = 0;
    }

    public bool Contains(T item)
    {
        if(item.HeapIndex < _currentItemCount)
        {
            return Equals(_items[item.HeapIndex], item);
        }
        else
        {
            return false;
        }
    }

    void SortDown(T item)
    {
        while(true)
        {
            int _childIndexLeft = item.HeapIndex * 2 + 1;
            int _childIndexRight = item.HeapIndex * 2 + 2;
            int _swapIndex = 0;

            if(_childIndexLeft < _currentItemCount)
            {
                _swapIndex = _childIndexLeft;

                if(_childIndexRight < _currentItemCount)
                {
                    if(_items[_childIndexLeft].CompareTo(_items[_childIndexRight]) < 0 )
                    {
                        _swapIndex = _childIndexRight;
                    }
                }

                if(item.CompareTo(_items[_swapIndex]) < 0)
                {
                    Swap(item, _items[_swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item)
    {
        int _parentIndex = (item.HeapIndex - 1)/2;

        while(true)
        {
            T _parentItem = _items[_parentIndex];
            if(item.CompareTo(_parentItem) > 0)
            {
                Swap(item, _parentItem);
            }
            else
            {
                break;
            }

            _parentIndex = (item.HeapIndex-1)/2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        _items[itemA.HeapIndex] = itemB;
        _items[itemB.HeapIndex] = itemA;

        int _itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = _itemAIndex;
    }
}


public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex 
    {
        get;
        set;
    }

}