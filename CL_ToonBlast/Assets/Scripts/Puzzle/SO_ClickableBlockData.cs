using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClickableBlockData", menuName = "Scriptable/Puzzle/ClickableBlockData")]
public class SO_ClickableBlockData : SO_BasePuzzleBlockData
{
    [field: SerializeField] public int MinRequiredNeighborsToBreak { get; private set; }
}