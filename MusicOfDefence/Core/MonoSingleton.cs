using Unity.VisualScripting;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool applicationIsQuitting = false;
    private static object key = new object();

    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting == true)
            {
                Debug.Log("OnApplicationIsQuitting");
                return null;
            }
            lock (key)
            {

                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        instance = new GameObject(typeof(T).ToSafeString()).AddComponent<T>();
                    }
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        applicationIsQuitting = false;
    }

    private void OnApplicationQuit()
    {
        applicationIsQuitting = true;
    }

    private void OnDestroy()
    {
        applicationIsQuitting = true;
    }

}
