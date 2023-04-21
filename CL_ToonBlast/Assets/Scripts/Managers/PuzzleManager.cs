using BalDUtilities.CreateUtils;
using BalDUtilities.MouseUtils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleManager : Singleton<PuzzleManager>, IPointerDownHandler
{
    [field: SerializeField] public Grid<GridBlock> PuzzleGrid { get; private set; }

    [Header("Puzzle Grid Proprieties")]

    public const int GridWidth = 9;
    public const int GridHeight = 9;
    [SerializeField] private float cellSize;

    [SerializeField] private Transform cellsContainer;

    [field: SerializeField] public SO_BlocksArray BlocksData { get; private set; }

    [SerializeField] private SO_PuzzleData puzzle;

    [SerializeField] private GameObject block_PF;

    private struct S_NeighboursWithXY
    {
        public GridBlock value;
        public int x;
        public int y;

        public S_NeighboursWithXY(GridBlock _value, int _x, int _y)
        {
            value = _value;
            x = _x;
            y = _y;
        }
    }

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField] private bool drawGridDebug;

    [InspectorButton(nameof(EDITOR_CreateGrid), ButtonWidth = 150)]
    [SerializeField] private bool ed_createGrid;

    private void EDITOR_CreateGrid() => CreateGrid();

    [InspectorButton(nameof(EDITOR_LogGrid), ButtonWidth = 150)]
    [SerializeField] private bool logGrid;

    private void EDITOR_LogGrid() => Debug.Log(PuzzleGrid?.ToString());

    [InspectorButton(nameof(MakeBlocksFall), ButtonWidth = 150)]
    [SerializeField] private bool makeblocksfall;

#endif

    private void Reset()
    {
        cellsContainer = this.transform;
    }

    protected virtual void CreateGrid(bool drawDebug = false)
    {
        PuzzleGrid = new Grid<GridBlock>(GridWidth, GridHeight, cellSize, cellsContainer.transform.position, (x, y) => null);

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                SO_BasePuzzleBlockData block = puzzle.BlocksData[x, y];

                if (block != null)
                {
                    GameObject blockObj = block_PF.Create(cellsContainer);
                    blockObj.transform.localScale = new Vector3(cellSize, cellSize);

                    SetPositionOfBlock(x, y, blockObj.transform);
                    blockObj.GetComponent<GridBlock>().SetData(block);

                    GridBlock gridBlock = blockObj.GetComponent<GridBlock>();
                    gridBlock.SetData(block);

                    PuzzleGrid.SetValue(x, y, gridBlock);
                }
            }
        }
    }

    private void Start()
    {
        CreateGrid();
        //MakeBlocksFall();
    }

    protected virtual void Update()
    {
#if UNITY_EDITOR
        if (drawGridDebug) PuzzleGrid?.DrawDebug();
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Get the position of the mouse click
        PuzzleGrid.GetXY(MousePosition.GetMouseWorldPosition(), out int x, out int y);
        Debug.Log(x + " " + y);

        if (PuzzleGrid.IsOutOfBounds(x, y)) return;

        // Get the block the player clicked on
        GridBlock clickedOnBlock = PuzzleGrid.GetValue(x, y);
        if (clickedOnBlock == null) return;

        // retrieve the data of the block
        SO_BasePuzzleBlockData blockData = clickedOnBlock.BlockData;
        if (blockData == null) return;

        int x2 = x;
        while (x2 > 0)
        {
            if (PuzzleGrid.GetValue(x2 - 1, y) == null) x2--;
            else break;
        }

        PuzzleGrid.SetValue(x2, y, clickedOnBlock);
        Debug.Log(PuzzleGrid.GetValue(x2, y));
        PuzzleGrid.SetValue(x, y, null);

        clickedOnBlock.transform.position = PuzzleGrid.GetWorldPosition(x2, y) + new Vector3(cellSize * .5f, cellSize * .5f);

        return;

        // check if the block is a clickable
        SO_ClickableBlockData clickableBlockData = blockData as SO_ClickableBlockData;
        if (clickableBlockData != null)
        {
            List<S_NeighboursWithXY> neighbours = new List<S_NeighboursWithXY>();
            CheckNeighboursOfIndex(x, y, ref neighbours, clickableBlockData);

            // if there is enough neighbours, destroy the blocks
            if (neighbours.Count > clickableBlockData.MinRequiredNeighborsToBreak)
            {
                foreach (var item in neighbours)
                {
                    PuzzleGrid.SetValue(item.x, item.y, null);
                    item.value.Damage();
                }

                MakeBlocksFall();
            }
        }
    }

    private void CheckNeighboursOfIndex(int x, int y, ref List<S_NeighboursWithXY> neighbours, SO_ClickableBlockData originData)
    {
        GridBlock current = PuzzleGrid.GetValue(x, y);

        if (current == null) return;
        if (current.wasChecked) return;
        if (current.BlockData != originData) return;

        S_NeighboursWithXY currentData = new S_NeighboursWithXY(current, x, y);
        neighbours.Add(currentData);

        current.wasChecked = true;

        CheckNeighboursOfIndex(x + 1, y, ref neighbours, originData); // Right
        CheckNeighboursOfIndex(x - 1, y, ref neighbours, originData); // Left
        CheckNeighboursOfIndex(x, y + 1, ref neighbours, originData); // Up
        CheckNeighboursOfIndex(x, y - 1, ref neighbours, originData); // Down
    }

    private void MakeBlocksFall()
    {
        return; 
        for (int x = 1; x < GridWidth; x++)
        {
            // no need to check the bottom line
            for (int y = 0; y < GridHeight; y++)
            {
                GridBlock block = PuzzleGrid.GetValue(x, y);
                if (block == null) continue;
                if (block.BlockData.IsStatic) continue;

                Debug.Log(block.BlockData.BlockSprite.texture.ToString() + " " + x + " "+ y);

                // check for every cells beneath the current
                int xBeneath = x - 1;
                while (xBeneath > 0 && PuzzleGrid.GetValue(xBeneath, y) == null)
                {
                    xBeneath--;
                }

                PuzzleGrid.SetValue(xBeneath, y, block);
                PuzzleGrid.SetValue(x, y, null);

                SetPositionOfBlock(xBeneath, y, block.transform);
            }
        }
    }

    private void SetPositionOfBlock(int x, int y, Transform blockTransform)
    {
        int blockPosX = GridHeight - (x % GridHeight) - 1;
        blockTransform.position = PuzzleGrid.GetWorldPosition(blockPosX, y) + new Vector3(cellSize * .5f, cellSize * .5f);
    }
}