namespace Kuro.Utilities.DesignPattern
{
    public abstract class LegacySingleton<T> where T : LegacySingleton<T>, new()
    {
        private static readonly object _lock = new();
        private static T _instance;
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance != null) return _instance;

                    _instance = new T();
                    _instance.OnInitialize();
                    return _instance;
                }
            }
        }

        public static bool IsInitialized => _instance is not null;

        public virtual void OnInitialize()
        {

        }

        public void DestroyIntance()
        {
            _instance.OnDestroy();
            _instance = null;
        }

        protected virtual void OnDestroy()
        {

        }
    }
}
