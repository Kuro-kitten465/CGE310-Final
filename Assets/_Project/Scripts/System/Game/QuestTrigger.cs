using UnityEngine;

namespace Kuro.GameSystem
{
    public class QuestTrigger : MonoBehaviour
    {
        public enum TriggerType
        {
            StartQuest,
            CompleteQuest,
            UpdateQuest
        }

        [Header("Quest Trigger")]
        [SerializeField] private TriggerType triggerType;
        [Header("References")]
        [SerializeField] private Quest quest;

        public void TriggerQuest()
        {
            switch (triggerType)
            {
                case TriggerType.StartQuest:
                    QuestManager.Instance.StartQuest(quest.QuestID);
                    break;
                case TriggerType.CompleteQuest:
                    QuestManager.Instance.CompleteQuest(quest);
                    break;
                case TriggerType.UpdateQuest:
                    //QuestManager.Instance.UpdateQuest(quest.QuestID);
                    break;
            }
        }
    }
}
