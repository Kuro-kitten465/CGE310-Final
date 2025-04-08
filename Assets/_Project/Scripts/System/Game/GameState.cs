using UnityEngine;
using System.Collections.Generic;

namespace Kuro.GameSystem
{
    public class GameState : MonoBehaviour
    {
        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<GameState>();
                    if (_instance == null)
                    {
                        GameObject obj = new("GameState");
                        _instance = obj.AddComponent<GameState>();
                        DontDestroyOnLoad(obj);
                    }
                }
                return _instance;
            }
        }

        // Game flags dictionary
        private Dictionary<string, bool> flags = new Dictionary<string, bool>();

        // Current quest state
        private Dictionary<string, int> questProgress = new Dictionary<string, int>();

        // Last shown dialogue ID (for checkpoint feature)
        public string lastDialogueID = "";
        public int lastDialogueLineIndex = 0;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Methods to set and get flags
        public void SetFlag(string flagName, bool value = true)
        {
            flags[flagName] = value;
        }

        public bool GetFlag(string flagName)
        {
            if (flags.TryGetValue(flagName, out bool value))
            {
                return value;
            }
            return false;
        }

        // Methods for quest progress
        public void AdvanceQuest(string questID)
        {
            if (questProgress.ContainsKey(questID))
            {
                questProgress[questID]++;
            }
            else
            {
                questProgress[questID] = 1;
            }

            Debug.Log($"Quest {questID} advanced to stage {questProgress[questID]}");
        }

        public int GetQuestProgress(string questID)
        {
            if (questProgress.TryGetValue(questID, out int value))
            {
                return value;
            }
            return 0;
        }

        // Save current game state
        public void SaveGameState()
        {
            // Convert flags to JSON
            string flagsJson = JsonUtility.ToJson(flags);
            PlayerPrefs.SetString("GameFlags", flagsJson);

            // Convert quest progress to JSON
            string questJson = JsonUtility.ToJson(questProgress);
            PlayerPrefs.SetString("QuestProgress", questJson);

            // Save last dialogue information
            PlayerPrefs.SetString("LastDialogueID", lastDialogueID);
            PlayerPrefs.SetInt("LastDialogueLineIndex", lastDialogueLineIndex);

            PlayerPrefs.Save();
            Debug.Log("Game state saved");
        }

        // Load game state
        public void LoadGameState()
        {
            if (PlayerPrefs.HasKey("GameFlags"))
            {
                string flagsJson = PlayerPrefs.GetString("GameFlags");
                flags = JsonUtility.FromJson<Dictionary<string, bool>>(flagsJson);
            }

            if (PlayerPrefs.HasKey("QuestProgress"))
            {
                string questJson = PlayerPrefs.GetString("QuestProgress");
                questProgress = JsonUtility.FromJson<Dictionary<string, int>>(questJson);
            }

            if (PlayerPrefs.HasKey("LastDialogueID"))
            {
                lastDialogueID = PlayerPrefs.GetString("LastDialogueID");
                lastDialogueLineIndex = PlayerPrefs.GetInt("LastDialogueLineIndex");
            }

            Debug.Log("Game state loaded");
        }

        // Reset game to beginning
        public void ResetGame()
        {
            flags.Clear();
            questProgress.Clear();
            lastDialogueID = "";
            lastDialogueLineIndex = 0;
            PlayerPrefs.DeleteAll();
            Debug.Log("Game state reset");
        }
    }
}
