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
           EventBus.Publish(EventCollector.HideQuestEvent);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            EventBus.Unsubscribe(EventCollector.ShowQuestEvent);
            EventBus.Unsubscribe(EventCollector.HideQuestEvent);
        }

        protected override void OnInitialize()
        {
            _database = Resources.Load<QuestDatabase>("QuestDatabase");

            EventBus.Subscribe(EventCollector.ShowQuestEvent, ShowQuest);
            EventBus.Subscribe(EventCollector.HideQuestEvent, HideQuest);
        }

        public void StartQuest(string questID)
        {
            if (_currentQuest != null)
            {
                Debug.LogWarning("A quest is already in progress. Complete it before starting a new one.");
                return;
            }

            _currentQuest = _database.Quests.FirstOrDefault(q => q.QuestID == questID);
            if (_currentQuest == null)
            {
                Debug.LogError($"Quest with ID {questID} not found.");
                return;
            }

            _questName.text = _currentQuest.QuestElement.QuestName;
            _questDescription.text = _currentQuest.QuestElement.Description;

            EventBus.Publish(EventCollector.ShowQuestEvent);
        }

        public void CompleteQuest(Quest quest)
        {
            if (_currentQuest == null || _currentQuest != quest)
            {
                Debug.LogWarning("No matching quest is currently in progress.");
                return;
            }

            _questName.text = "";
            _questDescription.text = "";

            _currentQuest = null;
            EventBus.Publish(EventCollector.HideQuestEvent);
        }
    }
}
