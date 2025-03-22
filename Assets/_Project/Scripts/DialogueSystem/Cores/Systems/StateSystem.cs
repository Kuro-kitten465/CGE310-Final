using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using KuroNovel.Data;

namespace KuroNovel.Core
{
    public class StateSystem
    {
        private enum NovelState { Idle, DialogueDisplay, WaitingForInput, ProcessingChoice, Paused }
        
        private NovelState _currentState = NovelState.Idle;
        private ScriptSystem _scriptSystem;
        private UISystem _uiSystem;
        private VariableSystem _variableSystem;
        private AssetSystem _assetSystem;
        private SoundSystem _soundSystem;
        private InputSystem _inputSystem;
        
        private bool _isExecuting;
        
        public StateSystem(
            ScriptSystem scriptSystem, 
            UISystem uiSystem, 
            VariableSystem variableSystem,
            AssetSystem assetSystem,
            SoundSystem soundSystem,
            InputSystem inputSystem)
        {
            _scriptSystem = scriptSystem;
            _uiSystem = uiSystem;
            _variableSystem = variableSystem;
            _assetSystem = assetSystem;
            _soundSystem = soundSystem;
            _inputSystem = inputSystem;
            
            // Subscribe to input events
            _inputSystem.OnAdvance += HandleAdvanceInput;
        }

        public void BeginExecution()
        {
            _isExecuting = true;
            ProcessCurrentNode();
        }

        public void Pause()
        {
            _currentState = NovelState.Paused;
        }

        public void Resume()
        {
            if (_currentState == NovelState.Paused)
            {
                _currentState = NovelState.WaitingForInput;
            }
        }

        public void Stop()
        {
            _isExecuting = false;
            _uiSystem.HideDialogue();
        }

        private void HandleAdvanceInput()
        {
            if (_currentState == NovelState.WaitingForInput)
            {
                ProcessNextNode();
            }
        }

        private async void ProcessCurrentNode()
        {
            if (!_isExecuting) return;
            
            var currentNode = _scriptSystem.GetCurrentNode();
            if (currentNode == null)
            {
                Debug.LogError("[KuroNovel] Current node is null!");
                return;
            }

            switch (currentNode.Type)
            {
                case NodeType.Dialogue:
                    await ProcessDialogueNode(currentNode);
                    break;
                case NodeType.Choice:
                    await ProcessChoiceNode(currentNode);
                    break;
                case NodeType.Condition:
                    ProcessConditionNode(currentNode);
                    break;
                case NodeType.Action:
                    await ProcessActionNode(currentNode);
                    break;
                default:
                    Debug.LogError($"[KuroNovel] Unknown node type: {currentNode.Type}");
                    break;
            }
        }

        private async Task ProcessDialogueNode(DialogueNode node)
        {
            // Process commands first
            foreach (var command in node.Commands)
            {
                await ExecuteCommand(command);
            }
            
            // Show dialogue
            string characterName = string.Empty;
            Color nameColor = Color.white;
            
            if (!string.IsNullOrEmpty(node.CharacterID))
            {
                // In a real implementation, we would look up the character from a database
                characterName = node.CharacterID;
                nameColor = Color.white; // Get from character data
            }
            
            // Get localized text
            string dialogueText = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(node.Content.TableReference, node.Content.TableEntryReference);
            
            _uiSystem.ShowDialogue(characterName, dialogueText, nameColor);
            
            // Set state to waiting for input
            _currentState = NovelState.WaitingForInput;
        }

        private async Task ProcessChoiceNode(DialogueNode node)
        {
            _currentState = NovelState.ProcessingChoice;
            
            List<string> choiceTexts = new List<string>();
            List<string> nodeIds = new List<string>();
            
            foreach (var choice in node.Choices)
            {
                // Check condition if exists
                if (!string.IsNullOrEmpty(choice.Condition))
                {
                    if (!_variableSystem.EvaluateCondition(choice.Condition))
                    {
                        continue; // Skip this choice
                    }
                }
                
                // Get localized text
                string choiceText = await LocalizationSettings.StringDatabase.GetLocalizedStringAsync(choice.Text.TableReference, choice.Text.TableEntryReference);
                choiceTexts.Add(choiceText);
                nodeIds.Add(choice.NextNodeID);
            }
            
            // Show choices and wait for selection
            string selectedNodeId = await _uiSystem.ShowChoices(choiceTexts, nodeIds);
            
            if (!string.IsNullOrEmpty(selectedNodeId))
            {
                _scriptSystem.AdvanceToNode(selectedNodeId);
            }
            else
            {
                // Default to first choice if something went wrong
                _scriptSystem.AdvanceToNode(node.Choices[0].NextNodeID);
            }
            
            // Continue processing
            ProcessCurrentNode();
        }

        private void ProcessConditionNode(DialogueNode node)
        {
            if (node.NodeCondition == null)
            {
                Debug.LogError("[KuroNovel] Condition node has no condition!");
                return;
            }
            
            bool result = _variableSystem.EvaluateCondition(node.NodeCondition.Expression);
            string nextNodeId = result ? node.NodeCondition.TrueNodeID : node.NodeCondition.FalseNodeID;
            
            _scriptSystem.AdvanceToNode(nextNodeId);
            ProcessCurrentNode();
        }

        private async Task ProcessActionNode(DialogueNode node)
        {
            foreach (var action in node.Actions)
            {
                await ExecuteAction(action);
            }
            
            // Move to next node
            _scriptSystem.AdvanceToNode(node.NextNodeID);
            ProcessCurrentNode();
        }

        private async Task ExecuteCommand(Command command)
        {
            // Implementation would handle each command type
            switch (command.Type)
            {
                case CommandType.Show:
                    // Example: Show character
                    break;
                case CommandType.Hide:
                    // Example: Hide character
                    break;
                case CommandType.SetBackground:
                    // Example: Change background
                    break;
                // Other command types...
            }
            
            await Task.CompletedTask; // Placeholder
        }

        private async Task ExecuteAction(KuroNovel.Data.Action action)
        {
            // Implementation would handle custom actions
            await Task.CompletedTask; // Placeholder
        }

        private void ProcessNextNode()
        {
            var currentNode = _scriptSystem.GetCurrentNode();
            
            if (currentNode != null && !string.IsNullOrEmpty(currentNode.NextNodeID))
            {
                _scriptSystem.AdvanceToNode(currentNode.NextNodeID);
                ProcessCurrentNode();
            }
            else
            {
                Debug.Log("[KuroNovel] End of scenario reached.");
                Stop();
            }
        }
    }
}
