using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using KuroNovel.Data;

namespace KuroNovel.Core
{
    public class VisualNovelManager : MonoBehaviour
    {
        private static VisualNovelManager _instance;

        public static VisualNovelManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("VisualNovelManager");
                    _instance = go.AddComponent<VisualNovelManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        [SerializeField] private NovelSettings _settings;

        // Core systems
        private ScriptSystem _scriptSystem;
        private AssetSystem _assetSystem;
        private UISystem _uiSystem;
        private StateSystem _stateSystem;
        private VariableSystem _variableSystem;
        private InputSystem _inputSystem;
        private LocalizationSystem _localizationSystem;
        private SoundSystem _soundSystem;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeSystems();
        }

        private void InitializeSystems()
        {
            _variableSystem = new VariableSystem();
            _assetSystem = new AssetSystem();
            _soundSystem = new SoundSystem();
            _inputSystem = new InputSystem();
            _localizationSystem = new LocalizationSystem();
            _scriptSystem = new ScriptSystem(_variableSystem);
            _uiSystem = new UISystem(_settings);
            _stateSystem = new StateSystem(_scriptSystem, _uiSystem, _variableSystem, _assetSystem, _soundSystem, _inputSystem);
        }

        public static async void Start(string scenarioID, Dictionary<string, object> initialVariables = null)
        {
            await Instance.StartNovel(scenarioID, initialVariables);
        }

        private async Task StartNovel(string scenarioID, Dictionary<string, object> initialVariables)
        {
            // Initialize variables if provided
            if (initialVariables != null)
            {
                _variableSystem.SetInitialVariables(initialVariables);
            }

            // Load the scenario
            bool success = await _scriptSystem.LoadScenarioAsync(scenarioID);
            
            if (!success)
            {
                Debug.LogError($"[KuroNovel] Failed to load scenario: {scenarioID}");
                return;
            }

            // Initialize UI
            _uiSystem.Initialize();
            
            // Begin execution
            _stateSystem.BeginExecution();
        }

        public void Pause()
        {
            _stateSystem.Pause();
        }

        public void Resume()
        {
            _stateSystem.Resume();
        }

        public void Stop()
        {
            _stateSystem.Stop();
        }
    }
}
