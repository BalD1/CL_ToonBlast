using BalDUtilities.EditorUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class WINDOW_PuzzleMaker : EditorWindow
{
    private Vector2 windowScroll = Vector2.zero;

    private int scenesScrollView = 0;

    private const int buttonWidth = 40;
    private const int buttonHeight = 40;

    [SerializeField] private SO_PuzzleData currentPuzzle;

    [SerializeField] private Texture emptyTexture;

    [SerializeField] private SO_BlocksArray blocksArray;

    [MenuItem("Window/PuzzleMaker")]
    public static void ShowWindow()
    {
        GetWindow<WINDOW_PuzzleMaker>("Puzzle Maker Window");
    }

    private void OnGUI()
    {
        ReadOnlyDraws.EditorScriptDraw(typeof(WINDOW_PuzzleMaker), this);

        windowScroll = EditorGUILayout.BeginScrollView(windowScroll);

        if (blocksArray == null) blocksArray = Resources.Load<SO_BlocksArray>("Puzzle/Blocks/BlocksArray");
        blocksArray = EditorGUILayout.ObjectField(blocksArray, typeof(SO_BlocksArray), false) as SO_BlocksArray;

        emptyTexture = EditorGUILayout.ObjectField(emptyTexture, typeof(Texture), false) as Texture;

        DrawPuzzleDataArray();

        EditorGUILayout.EndScrollView();

        if (GUI.changed)
        {
            if (emptyTexture != null) EditorUtility.SetDirty(emptyTexture);
        }
    }

    private void DrawPuzzleDataArray()
    {
        currentPuzzle = EditorGUILayout.ObjectField(currentPuzzle, typeof(SO_PuzzleData), false) as SO_PuzzleData;
        if (currentPuzzle == null) return;

        Undo.RecordObject(currentPuzzle, "Puzzle");

        for (int x = 0; x < PuzzleManager.GridWidth; x++)
        {
            EditorGUILayout.BeginHorizontal();

            for (int y = 0; y < PuzzleManager.GridHeight; y++)
            {
                EditorGUILayout.BeginVertical("GroupBox");

                Texture buttonTexture;

                SO_BasePuzzleBlockData currentBlockData = currentPuzzle.BlocksData[x, y];
                if (currentBlockData == null || currentBlockData.BlockSprite == null) buttonTexture = emptyTexture;
                else buttonTexture = currentBlockData.BlockSprite.texture;

                SO_BasePuzzleBlockData newBlock = EditorGUILayout.ObjectField(currentBlockData, typeof(SO_BasePuzzleBlockData), false, GUILayout.MaxWidth(20)) as SO_BasePuzzleBlockData;
                if (newBlock != null) currentPuzzle.SetBlockDataAt(x, y, newBlock);

                if (GUILayout.Button(buttonTexture, GUILayout.MaxWidth(buttonWidth), GUILayout.MaxHeight(buttonHeight)))
                {
                    int nextID = currentBlockData == null ? 0 : currentBlockData.ID + 1;

                    if (nextID >= blocksArray.BlocksData.Length)
                    {
                        nextID = -1;
                        newBlock = null;
                    }
                    else newBlock = blocksArray.BlocksData[nextID];

                    currentPuzzle.SetBlockDataAt(x, y, newBlock);
                }


                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentPuzzle);
        }
    }
}
