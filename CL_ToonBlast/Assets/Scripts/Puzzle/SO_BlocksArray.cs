using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BlocksArray", menuName = "Scriptable/Puzzle/BlocksArray")]
public class SO_BlocksArray : ScriptableObject
{
    [field: SerializeField] public SO_BasePuzzleBlockData[] BlocksData { get; private set; }

#if UNITY_EDITOR
    [InspectorButton(nameof(PopulateBlocksArray), ButtonWidth = 150)]
    [SerializeField] private bool populateBlocksArray;
#endif

    private void PopulateBlocksArray()
    {
        BlocksData = Resources.LoadAll<SO_BasePuzzleBlockData>("Puzzle/Blocks/");

        for (int i = 0; i < BlocksData.Length; i++)
        {
            BlocksData[i].SetID(i);
        }

#if UNITY_EDITOR
        EditorUtility.SetDirty(this); 
#endif
    }
}