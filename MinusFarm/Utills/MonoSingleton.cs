using UnityEngine;

namespace CW
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError($"{typeof(T).ToString()} Singleton ins not have in 하이라키");
                    }
                }

                return _instance;
            }
        }
    }
}