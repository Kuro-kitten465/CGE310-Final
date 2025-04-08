using UnityEngine;
using System.Collections.Generic;
using System;

namespace Kuro.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/Dialogue Data")]
    public class DialogueData : ScriptableObject
    {
        [Serializable]
        public class DialogueLine
        {
            public string characterName;
            public Sprite characterSprite;
            public string text;
            public float displayDuration = 3f; // How long to display before auto-advancing
            public string flagToSet = ""; // Optional flag to set when this line is shown
        }

        [Serializable]
        public class DialogueChoice
        {
            public string choiceText;
            public string flagToSet; // Flag that gets set when this choice is selected
        }

        public string dialogueID; // Unique identifier for this dialogue sequence
        public List<DialogueLine> lines = new();
        public DialogueChoice[] choices; // Choices at the end of the dialogue (optional)
        public bool triggersQuestUpdate = false;
        public string questIDToUpdate = "";
    }
}
