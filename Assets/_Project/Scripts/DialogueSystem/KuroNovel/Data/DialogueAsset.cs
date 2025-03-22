using System.Collections.Generic;
using UnityEngine;

namespace KuroNovel.Data
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "KuroNovel/DialogueAsset")]
    public class DialogueAsset : ScriptableObject
    {
        public List<DialogueEntry> dialogueEntries;
    }

    [System.Serializable]
    public class DialogueEntry
    {
        public string speakerID; // Character ID for localization
        public string textID;    // Dialogue line ID for localization
        public string emotion;   // Expression or sprite to use
        public string voicePath; // Addressable path for voice line
        public List<Choice> choices; // Player choices (optional)
        public List<DialogueFlag> flags; // Flags to check or modify
    }

    [System.Serializable]
    public class Choice
    {
        public string choiceTextID; // Choice text ID for localization
        public List<DialogueFlag> flagChanges; // Flags updated by this choice
    }

    [System.Serializable]
    public class DialogueFlag
    {
        public string flagName; // Name of the flag
        public int value;       // Value to check or modify
        public FlagOperation operation; // What to do with the flag
    }

    public enum FlagOperation
    {
        None,       // No operation
        Set,        // Set to a value
        Add,        // Increment by a value
        Subtract,   // Decrement by a value
        CheckEqual, // Check if flag equals a value
        CheckGreater, // Check if flag is greater than a value
        CheckLess // Check if flag is less than a value
    }
}
