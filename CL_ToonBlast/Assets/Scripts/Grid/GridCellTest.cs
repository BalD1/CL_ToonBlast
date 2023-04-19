using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellTest : MonoBehaviour
{
    [SerializeField] private TextMesh textMesh;

    private void Reset()
    {
        textMesh = this.GetComponent<TextMesh>();
    }

    private void Start()
    {
        SetRandomValue();
    }

    public void SetRandomValue()
    {
        textMesh.text = Random.Range(0, 100).ToString();

    }
}
