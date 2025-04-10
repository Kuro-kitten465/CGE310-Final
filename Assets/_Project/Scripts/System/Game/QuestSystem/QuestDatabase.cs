using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Kuro.GameSystem
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "Kuro/QuestDatabase")]
    public class QuestDatabase : ScriptableObject
    {
        public List<Quest> Quests = new();

        public Quest GetQuest(string questID)
        {
            return Quests.FirstOrDefault(q => q.QuestID == questID);
        }
    }
}
