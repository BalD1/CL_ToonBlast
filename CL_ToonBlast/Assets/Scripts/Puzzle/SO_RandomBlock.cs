using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomBlock", menuName = "Scriptable/Puzzle/RandomBlock")]
public class SO_RandomBlock : SO_BasePuzzleBlockData
{
    [field: SerializeField] public SO_BasePuzzleBlockData[] BlocksRange { get; private set; } 
}