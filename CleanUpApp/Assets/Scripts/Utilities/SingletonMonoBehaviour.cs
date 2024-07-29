using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
{
    public static T Instance
    {
        get; private set;
    }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != (this as T))
        {
            Destroy(this);
        }
        else
        {
            Instance = this as T;
        }
    }
}
