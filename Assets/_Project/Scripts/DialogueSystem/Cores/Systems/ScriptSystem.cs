using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using KuroNovel.Data;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace KuroNovel.Core
{
    public class ScriptSystem
    {
        private VariableSystem _variableSystem;
        private NovelScenario _currentScenario;
        private Dictionary<string, DialogueNode> _nodeMap = new Dictionary<string, DialogueNode>();
        private string _currentNodeID;

        public ScriptSystem(VariableSystem variableSystem)
        {
            _variableSystem = variableSystem;
        }

        public async Task<bool> LoadScenarioAsync(string scenarioID)
        {
            try
            {
                // Load scenario from Addressables
                AsyncOperationHandle<NovelScenario> handle = Addressables.LoadAssetAsync<NovelScenario>($"Scenarios/{scenarioID}");
                await handle.Task;
                
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _currentScenario = handle.Result;
                    
                    // Initialize variables
                    InitializeVariables();
                    
                    // Create node map for quick access
                    _nodeMap.Clear();
                    foreach (var node in _currentScenario.Nodes)
                    {
                        _nodeMap[node.ID] = node;
                    }
                    
                    // Set starting node
                    _currentNodeID = _currentScenario.StartNodeID;
                    
                    return true;
                }
                else
                {
                    Debug.LogError($"[KuroNovel] Failed to load scenario: {scenarioID}");
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }

        private void InitializeVariables()
        {
            foreach (var variable in _currentScenario.Variables)
            {
                _variableSystem.SetVariable(variable.Name, variable.GetDefaultValue());
            }
        }

        public DialogueNode GetCurrentNode()
        {
            return _nodeMap.TryGetValue(_currentNodeID, out var node) ? node : null;
        }

        public DialogueNode GetNode(string nodeID)
        {
            return _nodeMap.TryGetValue(nodeID, out var node) ? node : null;
        }

        public void AdvanceToNode(string nodeID)
        {
            if (_nodeMap.ContainsKey(nodeID))
            {
                _currentNodeID = nodeID;
            }
            else
            {
                Debug.LogError($"[KuroNovel] Node not found: {nodeID}");
            }
        }
    }
}
