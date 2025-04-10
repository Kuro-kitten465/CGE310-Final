using UnityEngine;
using System.Collections.Generic;
using System;
using Kuro.GameSystem;

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
            public bool isAutoAdvance = false; // Whether to auto-advance after the display duration
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
        public Quest questToUpdate; // Quest to update when this dialogue is triggered
        public bool TriggerCutsceneOnEnded = false; // Whether to trigger a cutscene when this dialogue is ended
    }
}
