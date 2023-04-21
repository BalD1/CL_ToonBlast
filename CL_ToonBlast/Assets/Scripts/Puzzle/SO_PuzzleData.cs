using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Scriptable/Puzzle/PuzzleData")]
public class SO_PuzzleData : ScriptableObject, ISerializationCallbackReceiver
{
    [field: SerializeField] public SO_BasePuzzleBlockData[,] BlocksData { get; private set; }

    [SerializeField] private List<Package<SO_BasePuzzleBlockData>> serializable;
    [System.Serializable]
    struct Package<TElement>
    {
        public int Index0;
        public int Index1;
        public TElement Element;
        public Package(int idx0, int idx1, TElement element)
        {
            Index0 = idx0;
            Index1 = idx1;
            Element = element;
        }
    }
    public void OnBeforeSerialize()
    {
        // Convert our unserializable array into a serializable list
        serializable = new List<Package<SO_BasePuzzleBlockData>>();
        if (BlocksData == null) BlocksData = new SO_BasePuzzleBlockData[PuzzleManager.GridWidth, PuzzleManager.GridHeight];
        for (int i = 0; i < BlocksData.GetLength(0); i++)
        {
            for (int j = 0; j < BlocksData.GetLength(1); j++)
            {
                serializable.Add(new Package<SO_BasePuzzleBlockData>(i, j, BlocksData[i, j]));
            }
        }
    }
    public void OnAfterDeserialize()
    {
        // Convert the serializable list into our unserializable array
        BlocksData = new SO_BasePuzzleBlockData[PuzzleManager.GridWidth, PuzzleManager.GridHeight];
        foreach (var package in serializable)
        {
            BlocksData[package.Index0, package.Index1] = package.Element;
        }
    }

    public void SetBlockDataAt(int x, int y, SO_BasePuzzleBlockData newData)
    {
        BlocksData[x, y] = newData;
    }
}