using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [field: SerializeField] [field: ReadOnly] public int x { get; private set; }
    [field: SerializeField] [field: ReadOnly] public int y { get; private set; }

    public void Setup(int _x, int _y)
    {
        x = _x; 
        y = _y;
    }
}
