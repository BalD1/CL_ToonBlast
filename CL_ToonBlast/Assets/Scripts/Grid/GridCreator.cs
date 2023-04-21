using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridCreator : MonoBehaviour
{
    [Header("Grid Proprieties")]

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;

    [SerializeField] private Transform cellsContainer;

    private Grid<int> grid;

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField] private bool drawDebug;
#endif

    private void Reset()
    {
        cellsContainer = this.transform;
    }

    private void OnValidate()
    {
        CreateGrid();
    }

    protected virtual void CreateGrid(bool drawDebug = false)
    {
        grid = new Grid<int>(gridWidth, gridHeight, cellSize, cellsContainer.transform.position, (x, y) => -1);
    }

    private void Start()
    {
        CreateGrid();
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        grid.DrawDebug();
#endif
    }
}
