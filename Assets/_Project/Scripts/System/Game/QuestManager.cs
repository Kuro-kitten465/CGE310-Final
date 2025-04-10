using System.Collections.Generic;
using UnityEngine;
using Kuro.Utilities.DesignPattern;
using System.Linq;
using TMPro;

namespace Kuro.GameSystem
{
    public class QuestManager : MonoSingleton<QuestManager>
    {
        [SerializeField] private Canvas _questCanvas;
        [SerializeField] private TMP_Text _questName;
        [SerializeField] private TMP_Text _questDescription;

        private QuestDatabase _database;
        private Quest _currentQuest;
        public Quest CurrentQuest => _currentQuest;
        public Quest NextQuest
        {
            get
            {
                var index = _database.Quests.IndexOf(_currentQuest);
                return _database.Quests[index + 1];
            }
        }
        public Quest PreviousQuest
        {
            get
            {
                var index = _database.Quests.IndexOf(_currentQuest);
                return _database.Quests[index - 1];
            }
        }

        private void ShowQuest() => _questCanvas.gameObject.SetActive(true);
        private void HideQuest() => _questCanvas.gameObject.SetActive(false);

        private void OnEnable()
        {
            EventBus.Subscribe(EventCollector.ShowQuestEvent, ShowQuest);
            EventBus.Subscribe(EventCollector.HideQuestEvent, HideQuest);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(EventCollector.ShowQuestEvent);
            EventBus.Unsubscribe(EventCollector.HideQuestEvent);
        }

        protected override void OnInitialize()
        {
            _database = Resources.Load<QuestDatabase>("QuestDatabase");
        }

        public void OnQuestCompleted()
        {
            if (_currentQuest.GoToNextQuestImmediately == false) return;
            
            GoToNextQuest(NextQuest.QuestID);
        }

        public void GoToNextQuest(string questID)
        {
            Quest quest = _database.GetQuest(questID);

            if (quest == null)
            {
                Debug.LogError($"Quest with ID {questID} not found in the database.");
                return;
            }

            _currentQuest = quest;

            _questName.text = quest.QuestElement.QuestName;
            _questDescription.text = quest.QuestElement.Description;
        }
    }
}
