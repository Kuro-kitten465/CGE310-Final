using UnityEngine;
using Kuro.GameSystem;
using System.Collections.Generic;

namespace Kuro.Dialogue
{
    [System.Serializable]
    public class GameStateCondition
    {
        public string requiredFlag = "";
        public string requiredQuestID = "";
        public int requiredQuestProgress = 0;
    }

    [System.Serializable]
    public class GameStateTransition
    {
        public string stateID;
        public GameStateCondition[] conditions;
        public DialogueData startDialogue; // Dialogue to play when entering this state
        public UnityEngine.Events.UnityEvent onStateEnter;
    }

    public class GameDirector : MonoBehaviour
    {
        public DialogueDatabase dialogueDatabase;

        [Header("Game States")]
        public List<GameStateTransition> gameStates = new();
        public string currentStateID = "start";

        private void Start()
        {
            // Load saved game if available
            GameState.Instance.LoadGameState();

            // Resume from checkpoint if available
            if (!string.IsNullOrEmpty(GameState.Instance.lastDialogueID))
            {
                DialogueData savedDialogue = dialogueDatabase.GetDialogueById(GameState.Instance.lastDialogueID);
                if (savedDialogue != null)
                {
                    DialogueManager.Instance.ResumeDialogueFromSave(savedDialogue, GameState.Instance.lastDialogueLineIndex);
                }
            }

            // Check if we need to transition to a different state based on loaded data
            CheckStateTransitions();
        }

        public void CheckStateTransitions()
        {
            foreach (var state in gameStates)
            {
                if (AreConditionsMet(state.conditions))
                {
                    TransitionToState(state);
                    break;
                }
            }
        }

        private bool AreConditionsMet(GameStateCondition[] conditions)
        {
            if (conditions == null || conditions.Length == 0)
                return true;

            foreach (var condition in conditions)
            {
                // Check flag condition
                if (!string.IsNullOrEmpty(condition.requiredFlag) &&
                    !GameState.Instance.GetFlag(condition.requiredFlag))
                {
                    return false;
                }

                // Check quest progress condition
                if (!string.IsNullOrEmpty(condition.requiredQuestID) &&
                    GameState.Instance.GetQuestProgress(condition.requiredQuestID) < condition.requiredQuestProgress)
                {
                    return false;
                }
            }

            return true;
        }

        public void TransitionToState(GameStateTransition state)
        {
            currentStateID = state.stateID;
            Debug.Log($"Transitioning to game state: {currentStateID}");

            // Start dialogue if specified
            if (state.startDialogue != null && !DialogueManager.Instance.IsDialogueActive())
            {
                DialogueManager.Instance.StartDialogue(state.startDialogue);
            }

            // Trigger state enter events
            state.onStateEnter?.Invoke();
        }

        // Call this when quest-relevant events happen in the game
        public void OnQuestEvent(string questID)
        {
            GameState.Instance.AdvanceQuest(questID);
            CheckStateTransitions();
        }

        // Save game state (call this when reaching checkpoint)
        public void SaveCheckpoint()
        {
            GameState.Instance.SaveGameState();
        }

        // Load from last checkpoint
        public void LoadCheckpoint()
        {
            GameState.Instance.LoadGameState();

            // Resume dialogue if available
            if (!string.IsNullOrEmpty(GameState.Instance.lastDialogueID))
            {
                DialogueData savedDialogue = dialogueDatabase.GetDialogueById(GameState.Instance.lastDialogueID);
                if (savedDialogue != null)
                {
                    DialogueManager.Instance.ResumeDialogueFromSave(savedDialogue, GameState.Instance.lastDialogueLineIndex);
                }
            }

            CheckStateTransitions();
        }

        // Reset game to start
        public void ResetGame()
        {
            GameState.Instance.ResetGame();
            currentStateID = "start";

            // Find and trigger the start state
            foreach (var state in gameStates)
            {
                if (state.stateID == "start")
                {
                    TransitionToState(state);
                    break;
                }
            }
        }
    }
}
