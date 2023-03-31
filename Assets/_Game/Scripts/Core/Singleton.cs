using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    public static T Instance;

    public bool dontDestroy;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            string typename = typeof(T).Name;
            Debug.LogWarning($"More that one instance of {typename} found.");
            Debug.LogError("Clean your scene!");
            Destroy(gameObject);
        }

        Instance = this as T;

        if(dontDestroy)
            DontDestroyOnLoad(gameObject);
    }
}