using UnityEngine;
using System.Collections.Generic;

namespace Kuro.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueDatabase", menuName = "Dialogue System/Dialogue Database")]
    public class DialogueDatabase : ScriptableObject
    {
        public List<DialogueData> allDialogues = new();

        // Get dialogue by ID
        public DialogueData GetDialogueById(string id)
        {
            return allDialogues.Find(d => d.dialogueID == id);
        }
    }
}
