// NumberTile.cs - Script for individual tiles in the puzzle
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberTile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Button button;
    
    public int Number { get; private set; }
    public Vector2Int CurrentPosition { get; private set; }
    
    private void Awake()
    {
        button.onClick.AddListener(OnTileClicked);
    }
    
    public void SetNumber(int number)
    {
        Number = number;
        numberText.text = number.ToString();
    }
    
    public void SetPosition(Vector2Int position)
    {
        CurrentPosition = position;
    }
    
    private void OnTileClicked()
    {
        NumberPuzzleManager.Instance.TryMoveTile(this);
    }
}
