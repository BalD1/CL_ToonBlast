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

#if UNITY_EDITOR
    [SerializeField] private bool drawDebug; 
#endif

    private void Start()
    {
        CreateGrid();
    }

    protected virtual void CreateGrid(bool drawDebug = false)
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

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (drawDebug) grid.DrawDebug();
#endif
    }
}
