using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    [field: SerializeField] public SO_BasePuzzleBlockData BlockData { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    [field: SerializeField, ReadOnly] public int CurrentHP { get; private set; }

    public bool wasChecked;

    private void Reset()
    {
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        CurrentHP = BlockData.MaxHP;
    }

    public void SetData(SO_BasePuzzleBlockData _blockData)
    {
        BlockData = _blockData;
        spriteRenderer.sprite = _blockData.BlockSprite;
    }

    public void Damage(int damage = 1)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0) OnDeath();
    }

    private void OnDeath()
    {
        Destroy(this.gameObject);
    }
}
