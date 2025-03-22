using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using KuroNovel.Data;

namespace KuroNovel.Core
{
    public class UISystem
    {
        private NovelSettings _settings;
        private GameObject _uiRoot;
        
        // UI Elements
        private DialoguePanel _dialoguePanel;
        private CharacterDisplay _characterDisplay;
        private BackgroundDisplay _backgroundDisplay;
        private ChoicePanel _choicePanel;

        public UISystem(NovelSettings settings)
        {
            _settings = settings;
        }

        public void Initialize()
        {
            // Create UI hierarchy
            if (_uiRoot == null)
            {
                _uiRoot = GameObject.Find("KuroNovelUI");
                
                if (_uiRoot == null)
                {
                    _uiRoot = new GameObject("KuroNovelUI");
                    var canvas = _uiRoot.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    _uiRoot.AddComponent<UnityEngine.UI.CanvasScaler>();
                    _uiRoot.AddComponent<UnityEngine.UI.GraphicRaycaster>();
                }
                
                // Initialize UI components
                InitializeUI();
            }
        }

        private void InitializeUI()
        {
            // Background
            var bgGO = new GameObject("Background");
            bgGO.transform.SetParent(_uiRoot.transform, false);
            _backgroundDisplay = bgGO.AddComponent<BackgroundDisplay>();
            
            // Character
            var charGO = new GameObject("Characters");
            charGO.transform.SetParent(_uiRoot.transform, false);
            _characterDisplay = charGO.AddComponent<CharacterDisplay>();
            
            // Dialogue
            var dialogueGO = new GameObject("Dialogue");
            dialogueGO.transform.SetParent(_uiRoot.transform, false);
            _dialoguePanel = dialogueGO.AddComponent<DialoguePanel>();
            _dialoguePanel.Initialize(_settings);
            
            // Choice
            var choiceGO = new GameObject("Choices");
            choiceGO.transform.SetParent(_uiRoot.transform, false);
            _choicePanel = choiceGO.AddComponent<ChoicePanel>();
        }

        public void ShowDialogue(string characterName, string text, Color nameColor)
        {
            _dialoguePanel.ShowDialogue(characterName, text, nameColor);
        }

        public void HideDialogue()
        {
            _dialoguePanel.HideDialogue();
        }

        public async Task<string> ShowChoices(List<string> choiceTexts, List<string> nodeIds)
        {
            return await _choicePanel.ShowChoices(choiceTexts, nodeIds);
        }

        public async Task ShowCharacter(Sprite sprite, string position, float duration = 0.5f)
        {
            await _characterDisplay.ShowCharacter(sprite, position, duration);
        }

        public async Task HideCharacter(string position, float duration = 0.5f)
        {
            await _characterDisplay.HideCharacter(position, duration);
        }

        public async Task ShowBackground(Sprite sprite, float duration = 1.0f)
        {
            await _backgroundDisplay.ShowBackground(sprite, duration);
        }
    }
}
