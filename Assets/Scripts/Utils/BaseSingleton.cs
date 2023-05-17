using UnityEngine;

namespace JengaTest.Utils
{
    public abstract class BaseSingleton<T> : MonoBehaviour where T : Component
    {
        private static object _lock = new object();
        private static bool applicationIsQuitting = false;
        [SerializeField] private bool persistent = true;
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (applicationIsQuitting)
                {
                    return null;
                }
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject(typeof(T).Name);
                            _instance = singleton.AddComponent<T>();
                        }
                    }
                    return _instance;
                }
            }
        }

        private void Awake()
        {
            if (persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
            OnAwake();
        }
        private void OnDestroy()
        {
            if (persistent)
            {
                applicationIsQuitting = true;
            }
            OnCallDestroy();
        }
        private void OnApplicationQuit()
        {
            applicationIsQuitting = true;
            OnCallApplicationQuit();
        }
        protected virtual void OnAwake() { }
        protected virtual void OnCallDestroy() { }
        protected virtual void OnCallApplicationQuit() { }
    }
}