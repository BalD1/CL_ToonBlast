using BalDUtilities.CreateUtils;
using BalDUtilities.MouseUtils;
using System;
using System.Collections.Generic;
using TMPro;
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
    [field: SerializeField] public SO_RandomBlock AllClickableBlocks { get; private set; }

    [SerializeField] private SO_PuzzleData puzzle;

    [SerializeField] private GameObject block_PF;

    [SerializeField] private TextMeshProUGUI scoreDisplay;

    [SerializeField] private int basePointsForBlocksDestroy = 50;
    private int currentScore;

    private int currentMultiplier;

    // just in case
    [SerializeField] private float click_COOLDOWN = .5f;
    private float click_TIMER;

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
                SO_BasePuzzleBlockData blockData = puzzle.BlocksData[x, y];

                if (blockData != null) CreateBlock(x, y, blockData, Vector2.zero);
            }
        }
    }

    private void Start()
    {
        CreateGrid();
    }

    protected virtual void Update()
    {
        if (click_TIMER > 0) click_TIMER -= Time.deltaTime;
#if UNITY_EDITOR
        if (drawGridDebug) PuzzleGrid?.DrawDebug();
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (click_TIMER > 0) return;
        click_TIMER = click_COOLDOWN;

        currentMultiplier = 0;

        // Get the position of the mouse click
        PuzzleGrid.GetXY(MousePosition.GetMouseWorldPosition(), out int x, out int y);

        if (PuzzleGrid.IsOutOfBounds(x, y)) return;

        // Get the block the player clicked on
        GridBlock clickedOnBlock = PuzzleGrid.GetValue(x, y);
        if (clickedOnBlock == null) return;

        // retrieve the data of the block
        SO_BasePuzzleBlockData blockData = clickedOnBlock.BlockData;
        if (blockData == null) return;

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
                SpawnNewBlocks();
            }
            else clickedOnBlock.OnCantClickFeedback();
        }
    }

    public void OnDestroyedBlock()
    {
        currentMultiplier++;
        currentScore += basePointsForBlocksDestroy * currentMultiplier;

        scoreDisplay.text = currentScore.ToString();
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
        for (int x = 0; x < GridWidth; x++)
        {
            CheckRow(x);
        }
    }

    private void CheckRow(int x)
    {
        // no need to check the bottom line
        for (int y = 1; y < GridHeight; y++)
        {
            GridBlock block = PuzzleGrid.GetValue(x, y);
            if (block == null) continue;
            if (block.BlockData.IsStatic) continue;

            // check for every cells beneath the current
            int yBeneath = y;
            while (yBeneath > 0)
            {
                if (PuzzleGrid.GetValue(x, yBeneath - 1) == null) yBeneath--;
                else break;
            }

            if (y == yBeneath) continue;

            PuzzleGrid.SetValue(x, yBeneath, block);
            PuzzleGrid.SetValue(x, y, null);

            SetPositionOfBlock(x, yBeneath, block.transform);

#if UNITY_EDITOR
            block.GetComponent<GridBlock>().SetName(x, yBeneath);
#endif
        }
    }

    private void SpawnNewBlocks()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            int y = GridHeight - 1;

            while(PuzzleGrid.GetValue(x, y) == null)
            {
                // spawn it outside of grid
                Vector2 spawnPos = PuzzleGrid.GetWorldPosition(x, y + (GridHeight - 1));

                CreateBlock(x, y, AllClickableBlocks.GetBlock(), spawnPos);

                y--;
            }
        }
    }

    private void CreateBlock(int x, int y, SO_BasePuzzleBlockData blockData, Vector2 spawnPos)
    {
        GameObject blockObj = block_PF.Create(cellsContainer);
        blockObj.transform.position = spawnPos;
        blockObj.transform.localScale = new Vector3(cellSize, cellSize);

        SetPositionOfBlock(x, y, blockObj.transform);

        GridBlock gridBlock = blockObj.GetComponent<GridBlock>();
        gridBlock.SetData(blockData);

        gridBlock.SetName(x, y);

        gridBlock.D_onDeath += OnDestroyedBlock;

        PuzzleGrid.SetValue(x, y, gridBlock);
    }

    private void SetPositionOfBlock(int x, int y, Transform blockTransform)
    {
        Vector2 targetPosition = PuzzleGrid.GetWorldPosition(x, y) + new Vector3(cellSize * .5f, cellSize * .5f);
        blockTransform.LeanMove(targetPosition, .25f);
    }
}