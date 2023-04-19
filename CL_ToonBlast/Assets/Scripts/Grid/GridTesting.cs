using UnityEngine;
using BalDUtilities.CreateUtils;
using BalDUtilities.MouseUtils;

public class GridTesting : MonoBehaviour
{
    private Grid<GameObject> grid;

    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellsSize;

    [SerializeField] private GameObject[,] gridCells;
    [SerializeField] private GameObject[,] cellsToDestroy;


    [SerializeField] private GameObject cell_PF;

#if UNITY_EDITOR
    [SerializeField] private bool updateInEditor;
#endif

    [InspectorButton(nameof(Cleanup), ButtonWidth = 150)]
    [SerializeField] private bool cleanup;

    private void OnValidate()
    {
        if (!updateInEditor) return;

        if (gridCells == null) gridCells = new GameObject[gridWidth, gridHeight];

        UnityEditor.EditorApplication.delayCall += () =>
        {
            foreach (var item in cellsToDestroy)
            {
                DestroyImmediate(item);
            }
        };
        cellsToDestroy = gridCells;

        gridCells = new GameObject[gridWidth, gridHeight];

        CreateGrid(true);
    }

    private void Cleanup()
    {
        foreach (Transform child in this.transform)
        {
            DestroyImmediate(child.gameObject);
        }
        gridCells = new GameObject[gridWidth, gridHeight];
    }

    private void Start()
    {
        Cleanup();
        CreateGrid();
    }

    private void CreateGrid(bool drawDebug = false)
    {
        grid = new Grid<GameObject>(gridWidth, gridHeight, cellsSize, this.transform.position - new Vector3(.5f, .5f) * cellsSize, 
        (x, y) =>
        {
            GameObject cell = cell_PF.Create(new Vector3(x, y) + this.transform.position);
            cell.transform.SetParent(this.transform, false);
            cell.transform.localScale = new Vector3(cellsSize, cellsSize);
            cell.transform.position *= cellsSize;
            return cell;
        }, drawDebug);
    }

    private void Update()
    {
        grid.DrawDebug();
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetValue(MousePosition.GetMouseWorldPosition())?.GetComponent<GridCellTest>().SetRandomValue();
        }
    }
}
