using UnityEngine;

namespace Kuro.Utilities.DesignPattern
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static readonly object _lock = new();
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (_lock)
                {
                    _instance = FindAnyObjectByType<T>();

                    if (_instance is null)
                    {
                        GameObject singletonObject = new(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                    }

                    if (_instance._dontDestroyOnLoad)
                        DontDestroyOnLoad(_instance.gameObject);

                    _instance.OnInitialize();
                }

                return _instance;
            }
        }

        public static bool IsInitialized => _instance is not null;

        [SerializeField]
        private bool _dontDestroyOnLoad = true;

        protected virtual void OnInitialize()
        {

        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);

                OnInitialize();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void DestroyIntance()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}
