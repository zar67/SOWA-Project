using System;
using UnityEngine;

public class BootstrapperDependancy : MonoBehaviour
{
    public bool IsComplete
    {
        get; 
        private set;
    } 

    public event Action<BootstrapperDependancy> OnDependancyComplete;

    public void SetComplete()
    {
        IsComplete = true;
        OnDependancyComplete?.Invoke(this);
    }
}
