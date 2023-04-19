using BalDUtilities.MouseUtils;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    private Grid<GameObject> grid;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellsSize;

    [SerializeField] private GameObject[,] gridCells;

    [SerializeField] private GameObject cell_PF;

    [SerializeField] private Transform cellsContainer;

#if UNITY_EDITOR
    [SerializeField] private bool drawDebug;
#endif

    private void Reset()
    {
        cellsContainer = this.transform;
    }

    protected virtual void Start()
    {
        CreateGrid();
    }

    protected virtual void CreateGrid(bool drawDebug = false)
    {
        grid = new Grid<GameObject>(gridWidth, gridHeight, cellsSize, cellsContainer.position - new Vector3(.5f, .5f) * cellsSize,
        (x, y) =>
        {
            GameObject cell = cell_PF.Create(new Vector3(x, y) + cellsContainer.position);
            Transform cellTransform = cell.transform;

            cellTransform.SetParent(cellsContainer, false);
            cellTransform.localScale = new Vector3(cellsSize, cellsSize);
            cellTransform.position *= cellsSize;

            GridCell cellScript = cell.GetComponent<GridCell>();
            cellScript.Setup(x, y);

            if (GameSettings.UnityEditor) cell.name = string.Format($"Cell [ {x}, {y} ]");

            return cell;
        }, drawDebug);
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (drawDebug) grid.DrawDebug();
#endif
    }
}
