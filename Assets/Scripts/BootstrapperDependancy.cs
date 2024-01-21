using System;
using UnityEngine;

public class BootstrapperDependancy : MonoBehaviour
{
    public bool IsComplete
    {
        get; 
        private set;
    } 

    public event Action OnDependancyComplete;

    public void SetComplete()
    {
        IsComplete = true;
        OnDependancyComplete?.Invoke();
    }
}
