// NumberPuzzleManager.cs - Main script to handle the number puzzle minigame
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class NumberPuzzleManager : MonoBehaviour
{
    // Singleton instance
    private static NumberPuzzleManager _instance;
    public static NumberPuzzleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<NumberPuzzleManager>();
            }
            return _instance;
        }
    }

    // Event that fires when the game is completed
    public static event Action OnPuzzleCompleted;

    [Header("Puzzle Settings")]
    [SerializeField] private Transform puzzleContainer;
    [SerializeField] private GameObject[] numberTilePrefab;
    [SerializeField] private int gridSize = 3;
    [SerializeField] private float tileSize = 100f;
    [SerializeField] private float tileSpacing = 10f;

    [Header("UI Elements")]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI movesCountText;
    [SerializeField] private GameObject completionPanel;

    // Internal state
    private List<NumberTile> tiles = new List<NumberTile>();
    private Vector2Int emptyTilePos;
    private int moveCount = 0;
    private bool puzzleCompleted = false;

    // Callback when the game is complete
    //private Action puzzleCompletedCallback;

    private void Awake()
    {
        // Set up singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        // Set up UI listeners
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseGame);
        }

        // Initialize the puzzle
        InitializePuzzle();
    }

    private void InitializePuzzle()
    {
        // Clear existing tiles if any
        foreach (Transform child in puzzleContainer)
        {
            Destroy(child.gameObject);
        }
        tiles.Clear();

        // Create and shuffle the tiles
        CreateTiles();
        ShuffleTiles();
        UpdateMovesText();

        // Hide completion panel
        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        puzzleCompleted = false;
        moveCount = 0;
    }

    private void CreateTiles()
    {
        int tileCount = 0;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                tileCount++;
                
                // Skip the last tile (empty space)
                if (tileCount == gridSize * gridSize)
                {
                    emptyTilePos = new Vector2Int(x, y);
                    continue;
                }

                // Create tile
                GameObject tileObj = Instantiate(numberTilePrefab[tileCount - 1], puzzleContainer);
                NumberTile tile = tileObj.GetComponent<NumberTile>();
                
                // Set tile properties
                tile.SetNumber(tileCount);
                tile.SetPosition(new Vector2Int(x, y));
                
                // Position the tile in grid
                float posX = x * (tileSize + tileSpacing);
                float posY = -y * (tileSize + tileSpacing);
                tileObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);

                // Add to list
                tiles.Add(tile);
            }
        }
    }

    private void ShuffleTiles()
    {
        // Perform random valid moves to shuffle the puzzle
        for (int i = 0; i < 100; i++) // Perform 100 random moves
        {
            List<NumberTile> movableTiles = GetMovableTiles();
            if (movableTiles.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, movableTiles.Count);
                SwapWithEmpty(movableTiles[randomIndex]);
            }
        }
        
        // Reset move count after shuffling
        moveCount = 0;
    }

    private List<NumberTile> GetMovableTiles()
    {
        List<NumberTile> movableTiles = new List<NumberTile>();
        
        foreach (NumberTile tile in tiles)
        {
            if (IsAdjacentToEmpty(tile.CurrentPosition))
            {
                movableTiles.Add(tile);
            }
        }
        
        return movableTiles;
    }

    private bool IsAdjacentToEmpty(Vector2Int position)
    {
        // Check if the position is adjacent to the empty tile
        return (position.x == emptyTilePos.x && Mathf.Abs(position.y - emptyTilePos.y) == 1) ||
               (position.y == emptyTilePos.y && Mathf.Abs(position.x - emptyTilePos.x) == 1);
    }

    public void TryMoveTile(NumberTile tile)
    {
        if (puzzleCompleted)
            return;
            
        if (IsAdjacentToEmpty(tile.CurrentPosition))
        {
            SwapWithEmpty(tile);
            moveCount++;
            UpdateMovesText();
            
            // Check if puzzle is solved
            if (IsPuzzleSolved())
            {
                CompletePuzzle();
            }
        }
    }

    private void SwapWithEmpty(NumberTile tile)
    {
        // Update positions
        Vector2Int tilePos = tile.CurrentPosition;
        tile.SetPosition(emptyTilePos);
        emptyTilePos = tilePos;
        
        // Update visual position
        float posX = tile.CurrentPosition.x * (tileSize + tileSpacing);
        float posY = -tile.CurrentPosition.y * (tileSize + tileSpacing);
        tile.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, posY);
    }

    private bool IsPuzzleSolved()
    {
        // Check if all tiles are in correct position
        foreach (NumberTile tile in tiles)
        {
            int expectedNumber = tile.CurrentPosition.y * gridSize + tile.CurrentPosition.x + 1;
            if (tile.Number != expectedNumber)
            {
                return false;
            }
        }
        
        // Also check if empty space is in bottom right
        return emptyTilePos.x == gridSize - 1 && emptyTilePos.y == gridSize - 1;
    }

    private void CompletePuzzle()
    {
        puzzleCompleted = true;
        GameManager.Instance.UpdateFlag("WinPuzzle", puzzleCompleted);
        
        // Show completion panel if available
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
        }
        
        // Invoke event
        OnPuzzleCompleted?.Invoke();

        CloseGame(); // Close the game after completion
        
        // Call completion callback if set
        //puzzleCompletedCallback?.Invoke();
    }

    private void UpdateMovesText()
    {
        if (movesCountText != null)
        {
            movesCountText.text = $"Moves: {moveCount}";
        }
    }

    private void CloseGame()
    {
        // Unload the scene
        OnPuzzleCompleted?.Invoke(); // Invoke the event to notify listeners
        StartCoroutine(UnloadScene());
    }

    private IEnumerator UnloadScene()
    {
        // Unload the puzzle scene
        OnPuzzleCompleted = null; // Clear the event to prevent memory leaks
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync("PuzzleScene");
        yield return unloadOperation;
    }

    // Public method to reset the puzzle
    public void ResetPuzzle()
    {
        InitializePuzzle();
    }

    // Static methods to launch the puzzle from anywhere in the game

    // Static method to start the puzzle with a callback
    public static void StartPuzzle(Action onComplete = null)
    {
        OnPuzzleCompleted += onComplete;
        SceneManager.LoadSceneAsync("PuzzleScene", LoadSceneMode.Additive);
        
        // Wait for the scene to load and set the callback
        //Instance.StartCoroutine(Instance.WaitForSceneLoad(onComplete));
    }

    private IEnumerator WaitForSceneLoad(Action onComplete)
    {
        // Wait a frame to ensure scene is loaded
        yield return null;
        
        // Set the callback
        //puzzleCompletedCallback = onComplete;
    }

    private void OnGUI()
    {
        GUILayout.Label("For Debug Only", GUILayout.Width(200), GUILayout.Height(30));
        if (GUILayout.Button("Reset Puzzle", GUILayout.Width(200), GUILayout.Height(30)))
        {
            ResetPuzzle();
        }
        if (GUILayout.Button("Always Win", GUILayout.Width(200), GUILayout.Height(30)))
        {
            GameManager.Instance.UpdateFlag("WinPuzzle", true);
            CompletePuzzle();
        }
        if (GUILayout.Button("Always Lose", GUILayout.Width(200), GUILayout.Height(30)))
        {
            GameManager.Instance.UpdateFlag("WinPuzzle", false);
            CompletePuzzle();
        }
    }
}