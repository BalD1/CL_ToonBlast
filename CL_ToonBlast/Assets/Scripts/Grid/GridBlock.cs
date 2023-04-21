using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBlock : MonoBehaviour
{
    [field: SerializeField] public SO_BasePuzzleBlockData BlockData { get; private set; }

    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    [field: SerializeField, ReadOnly] public int CurrentHP { get; private set; }

    public bool wasChecked;

    private bool isTweening;

    public delegate void D_OnDeath();
    public D_OnDeath D_onDeath;

    private void Reset()
    {
        SpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        CurrentHP = BlockData.MaxHP;
    }

    public void SetData(SO_BasePuzzleBlockData _blockData)
    {
        SO_RandomBlock rand = _blockData as SO_RandomBlock;
        if (rand != null) BlockData = rand.BlocksRange.RandomElement();
        else BlockData = _blockData;

        SpriteRenderer.sprite = BlockData.BlockSprite;
    }

    public void OnCantClickFeedback()
    {
        if (isTweening) return;

        isTweening = true;
        this.SpriteRenderer.sortingOrder++;
        this.gameObject.LeanScale(this.transform.localScale * 1.25f, .15f).setLoopPingPong(1).setEaseInOutCirc();
        LeanTween.rotateZ(this.gameObject, 45, .15f).setFrom(0).setLoopPingPong(1).setEaseInOutCirc().setOnComplete(() =>
        {
            this.SpriteRenderer.sortingOrder--;
            isTweening = false;
        });
    }

    public void Damage(int damage = 1)
    {
        CurrentHP -= damage;
        if (CurrentHP <= 0) OnDeath();
    }

    private void OnDeath()
    {
        LeanTween.scale(this.gameObject, Vector3.zero, .1f).setOnComplete( () => Destroy(this.gameObject));
        D_onDeath?.Invoke();
        
    }

    public void SetName(int x, int y)
    {
#if UNITY_EDITOR
        this.gameObject.name = BlockData.Name + $" [{x}, {y}]";
#endif
    }
}
