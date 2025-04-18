// NumberPuzzleLoader.cs - Static class to make it easy to call from anywhere
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class NumberPuzzleLoader
{
    // Load the puzzle with a callback when completed
    public static void LoadPuzzle(Action onPuzzleCompleted = null)
    {
        NumberPuzzleManager.StartPuzzle(onPuzzleCompleted);
    }
}
