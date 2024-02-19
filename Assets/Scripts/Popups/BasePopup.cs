using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BasePopupData
{
    public PopupType Type;

    public bool ShowCloseButton;
    public UnityAction CloseButtonAction;
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

        m_closeButtonHolder.SetActive(data.ShowCloseButton);

        if (data.ShowCloseButton)
        {
            if (data.CloseButtonAction != null)
            {
                m_closeButton.onClick.AddListener(data.CloseButtonAction);
            }

            m_closeButton.onClick.AddListener(Close);
        }
    }

    public virtual void Open()
    {
        gameObject.SetActive(true);
        m_animator.SetTrigger(m_openTrigger);

        CurrentState = PopupState.OPENING;
    }

    public virtual void Close()
    {
        m_animator.SetTrigger(m_closeTrigger);

        CurrentState = PopupState.CLOSING;
    }

    // Called by the animator when the open animation is complete.
    protected virtual void OnOpenComplete()
    {
        CurrentState = PopupState.OPEN;
    }

    // Called by the animator when the close animation is complete.
    protected virtual void OnCloseComplete()
    {
        Cleanup();
    }

    protected virtual void Cleanup()
    {
        gameObject.SetActive(false);

        m_closeButtonHolder.SetActive(false);
        m_closeButton.onClick.RemoveAllListeners();

        CurrentState = PopupState.CLOSED;
    }
}
