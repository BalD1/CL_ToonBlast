using UnityEngine;

public class Grid<TGridObject>
{
    public delegate void D_OnGridValueChanged(int x, int y);
    public D_OnGridValueChanged D_onGridValueChanged;

    private float cellSize;

    private int width;
    private int height;
    private TGridObject[,] gridArray;

    private Vector3 originPosition;

    public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition, System.Func<int, int, TGridObject> createGridObject, bool drawDebug = false)
    {
        this.width = _width;
        this.height = _height;
        this.cellSize = _cellSize;
        this.originPosition = _originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = createGridObject(x, y);
            }
        }

        if (drawDebug) DrawDebug(Color.white, 10);
    }

    /// <summary>
    /// Get the world position of the cell on <paramref name="x"/>, <paramref name="y"/>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>World Position in <see cref="Vector3"/></returns>
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize + originPosition;
    }

    /// <summary>
    /// <para> Gets the cell located on <paramref name="worldPosition"/> </para>
    /// <para> Outs the position <paramref name="x"/>, <paramref name="y"/> of the cell in grid</para>
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    /// <summary>
    /// <para> Sets the <paramref name="value"/> of the cell in <paramref name="x"/>, <paramref name="y"/> position of the grid </para>
    /// <para> Triggeres "<see cref="D_onGridValueChanged"/>" event</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, TGridObject value)
    {
        if (IsOutOfBounds(x, y)) return;
 
        gridArray[x, y] = value;
        
        D_onGridValueChanged?.Invoke(x, y);
    }
    /// <summary>
    /// <para> Sets the <paramref name="value"/> of the cell <paramref name="worldPosition"/> position </para>
    /// <para> Triggeres "<see cref="D_onGridValueChanged"/>" event</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(Vector3 worldPosition, TGridObject value)
    {
        GetXY(worldPosition, out int x, out int y);
        SetValue(x, y, value);
    }

    /// <summary>
    /// Gets the value of the cell in <paramref name="x"/>, <paramref name="y"/> position in the grid
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public TGridObject GetValue(int x, int y)
    {
        if (IsOutOfBounds(x, y)) return default;

        return gridArray[x, y];
    }
    /// <summary>
    /// Get the value of the cell in <paramref name="worldPosition"/> position
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public TGridObject GetValue(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        return GetValue(x, y);
    }

    /// <summary>
    /// Checks if there is a cell in <paramref name="x"/>, <paramref name="y"/> position in the grid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsOutOfBounds(int x, int y)
    {
        if (x < 0 || y < 0) return true;
        if (x >= width || y >= height) return true;

        return false;
        
    }

    /// <summary>
    /// Checks if there is a cell in <paramref name="pos"/> world position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsOutOfBounds(Vector2 pos)
    {
        GetXY(pos, out int x, out int y);
        return IsOutOfBounds(x,y);
    }

    public void DrawDebug() => DrawDebug(Color.white, Time.deltaTime);
    public void DrawDebug(Color color, float time)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), color, time);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), color, time);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), color, time);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), color, time);
    }
}
