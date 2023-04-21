using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SO_BasePuzzleBlockData : ScriptableObject
{
    [field: SerializeField] [field: ReadOnly] public int ID { get; private set; }
    [field: SerializeField] public Sprite BlockSprite { get; private set; }
    [field: SerializeField] public bool IsStatic { get; private set; }
    [field: SerializeField] public int MaxHP { get; private set; }

    private void Reset()
    {
        MaxHP = 1;
    }

    public void SetID(int id)
    {
        ID = id;
        EditorUtility.SetDirty(this);
    }
}