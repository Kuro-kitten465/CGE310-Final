using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Events;
using Kuro.GameSystem;

namespace Kuro.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager _instance;
        public static DialogueManager Instance
        {
            get { return _instance; }
        }

        [Header("UI Elements")]
        public GameObject dialoguePanel;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public Image characterImage;

        [Header("Choice UI")]
        public GameObject choicePanel;
        public Button choiceButtonPrefab;

        [Header("Dialogue Settings")]
        public float textSpeed = 0.05f;

        [Header("Events")]
        public UnityEvent OnDialogueStart;
        public UnityEvent OnDialogueEnd;

        private DialogueData currentDialogue;
        private int currentLineIndex;
        private bool isDialogueActive = false;
        private Coroutine typingCoroutine;
        private Coroutine autoAdvanceCoroutine;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // Hide dialogue UI on start
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(false);
        }

        // Start a dialogue sequence from a DialogueData scriptable object
        public void StartDialogue(DialogueData dialogue)
        {
            currentDialogue = dialogue;
            currentLineIndex = 0;
            isDialogueActive = true;

            // Save this dialogue as the current checkpoint
            GameState.Instance.lastDialogueID = dialogue.dialogueID;
            GameState.Instance.lastDialogueLineIndex = 0;

            dialoguePanel.SetActive(true);
            OnDialogueStart?.Invoke();

            DisplayNextLine();
        }

        // Resume dialogue from a saved point
        public void ResumeDialogueFromSave(DialogueData dialogue, int lineIndex)
        {
            currentDialogue = dialogue;
            currentLineIndex = lineIndex;
            isDialogueActive = true;

            dialoguePanel.SetActive(true);
            OnDialogueStart?.Invoke();

            DisplayNextLine();
        }

        public void DisplayNextLine()
        {
            // Stop any existing coroutines
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            if (autoAdvanceCoroutine != null)
                StopCoroutine(autoAdvanceCoroutine);

            // Check if we've reached the end of the dialogue
            if (currentLineIndex >= currentDialogue.lines.Count)
            {
                // Show choices if any exist, otherwise end dialogue
                if (currentDialogue.choices != null && currentDialogue.choices.Length > 0)
                {
                    ShowChoices();
                }
                else
                {
                    EndDialogue();
                }
                return;
            }

            // Get the current line
            var line = currentDialogue.lines[currentLineIndex];

            // Update UI
            nameText.text = line.characterName;

            if (line.characterSprite != null)
            {
                characterImage.sprite = line.characterSprite;
                characterImage.enabled = true;
            }
            else
            {
                characterImage.enabled = false;
            }

            // Save this position as checkpoint
            GameState.Instance.lastDialogueLineIndex = currentLineIndex;

            // Set flag if needed
            if (!string.IsNullOrEmpty(line.flagToSet))
            {
                GameState.Instance.SetFlag(line.flagToSet);
            }

            // Start typing animation
            typingCoroutine = StartCoroutine(TypeSentence(line.text));

            // Auto advance after set duration
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceAfterDelay(line.displayDuration));

            // Advance to next line for next time
            currentLineIndex++;
        }

        private IEnumerator TypeSentence(string sentence)
        {
            dialogueText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(textSpeed);
            }
        }

        private IEnumerator AutoAdvanceAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            DisplayNextLine();
        }

        private void ShowChoices()
        {
            // Clear existing choices
            foreach (Transform child in choicePanel.transform)
            {
                Destroy(child.gameObject);
            }

            // Create buttons for each choice
            foreach (var choice in currentDialogue.choices)
            {
                Button choiceButton = Instantiate(choiceButtonPrefab, choicePanel.transform);
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;

                // Setup button click with the choice's flag
                string flagToSet = choice.flagToSet;
                choiceButton.onClick.AddListener(() =>
                {
                    GameState.Instance.SetFlag(flagToSet);
                    EndDialogue();
                });
            }

            choicePanel.SetActive(true);
        }

        private void EndDialogue()
        {
            isDialogueActive = false;
            dialoguePanel.SetActive(false);
            choicePanel.SetActive(false);

            // Check if this dialogue triggers a quest update
            if (currentDialogue.triggersQuestUpdate && !string.IsNullOrEmpty(currentDialogue.questIDToUpdate))
            {
                GameState.Instance.AdvanceQuest(currentDialogue.questIDToUpdate);
            }

            OnDialogueEnd?.Invoke();
        }

        // Public method to check if dialogue is currently active
        public bool IsDialogueActive()
        {
            return isDialogueActive;
        }
    }
}