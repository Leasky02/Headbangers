using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SINGLETON - Trying to instantiate another instance of singleton!");
            Destroy(this);
            return;
        }
        Instance = this as T;
        DontDestroyOnLoad(Instance);
    }
}