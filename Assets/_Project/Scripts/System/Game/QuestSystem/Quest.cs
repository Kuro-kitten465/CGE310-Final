using UnityEngine;

namespace Kuro.GameSystem
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Kuro/Quest")]
    public class Quest : ScriptableObject
    {
        public string QuestID;
        public QuestElement QuestElement;
        public bool GoToNextQuestImmediately;
    }

    [System.Serializable]
    public class QuestElement
    {
        public string QuestName;
        public string Description;
    }
}
