using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BasePopupData
{
    public PopupType Type;

    public bool ShowCloseButton;
    public Action CloseButtonAction;

    public Action OnOpenStarted;
    public Action OnOpenComplete;
    public Action OnCloseStarted;
    public Action OnCloseComplete;
}

public class BasePopup : MonoBehaviour
{
    public enum PopupState
    {
        CLOSED,
        OPENING,
        OPEN,
        CLOSING,
    }

    protected event Action OnOpenStarted;
    protected event Action OnOpenComplete;
    protected event Action OnCloseStarted;
    protected event Action OnCloseComplete;

    [Header("Animation References")]
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected string m_openTrigger = "Open";
    [SerializeField] protected string m_closeTrigger = "Close";

    [Header("Close Button References")]
    [SerializeField] protected GameObject m_closeButtonHolder;
    [SerializeField] protected Button m_closeButton;

    public virtual PopupType Type => PopupType.NONE;

    public PopupState CurrentState
    {
        get; private set;
    }

    public bool IsOpeningOrOpen => CurrentState == PopupState.OPENING || CurrentState == PopupState.OPEN;

    public bool IsClosingOrClosed => CurrentState == PopupState.CLOSING || CurrentState == PopupState.CLOSED;

    public virtual void Init(BasePopupData data)
    {
        // Make sure all listeners are removed from old initialisations.
        Cleanup();

        OnOpenStarted = data.OnOpenStarted;
        OnOpenComplete = data.OnOpenComplete;
        OnCloseStarted = data.OnCloseStarted;
        OnCloseComplete = data.OnCloseComplete;

        m_closeButtonHolder.SetActive(data.ShowCloseButton);

        if (data.ShowCloseButton)
        {
            if (data.CloseButtonAction != null)
            {
                m_closeButton.onClick.AddListener(() => data.CloseButtonAction());
            }

            m_closeButton.onClick.AddListener(Close);
        }
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();

        m_animator.SetTrigger(m_openTrigger);

        CurrentState = PopupState.OPENING;
        OnOpenStarted?.Invoke();
    }

    public virtual void Close()
    {
        m_animator.SetTrigger(m_closeTrigger);

        CurrentState = PopupState.CLOSING;
        OnCloseStarted?.Invoke();
    }

    // Called by the animator when the open animation is complete.
    protected virtual void HandleOpenComplete()
    {
        CurrentState = PopupState.OPEN;
        OnOpenComplete?.Invoke();
    }

    // Called by the animator when the close animation is complete.
    protected virtual void HandleCloseComplete()
    {
        Cleanup();
        OnCloseComplete?.Invoke();
    }

    protected virtual void Cleanup()
    {
        gameObject.SetActive(false);

        m_closeButtonHolder.SetActive(false);
        m_closeButton.onClick.RemoveAllListeners();

        CurrentState = PopupState.CLOSED;
    }
}
