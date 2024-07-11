using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class BasePopupData
{
    public PopupType Type;

    public bool ShowCloseButton;
    public Dictionary<string, Action> CloseResultActions = new Dictionary<string, Action>();

    public Action OnOpenStarted;
    public Action OnOpenComplete;
    public Action OnCloseStarted;
    public Action OnCloseComplete;
}

public class BasePopup : MonoBehaviour
{
    public const string CLOSE_RESULT_CLOSE_BUTTON = "close";

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

    protected event Action OnOpenStarted;
    protected event Action OnOpenComplete;
    protected event Action OnCloseStarted;
    protected event Action OnCloseComplete;

    protected Dictionary<string, Action> m_closeResultActions = new Dictionary<string, Action>();

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

        m_closeResultActions = data.CloseResultActions;

        m_closeButtonHolder.SetActive(data.ShowCloseButton);

        if (data.ShowCloseButton)
        {
            if (data.CloseResultActions.ContainsKey(CLOSE_RESULT_CLOSE_BUTTON))
            {
                m_closeButton.onClick.AddListener(() => data.CloseResultActions[CLOSE_RESULT_CLOSE_BUTTON]?.Invoke());
            }

            m_closeButton.onClick.AddListener(() => Close(CLOSE_RESULT_CLOSE_BUTTON));
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

    public virtual void Close(string closeResult = CLOSE_RESULT_CLOSE_BUTTON)
    {
        m_animator.SetTrigger(m_closeTrigger);

        CurrentState = PopupState.CLOSING;
        OnCloseStarted?.Invoke();

        if (closeResult != null && !string.IsNullOrWhiteSpace(closeResult) && m_closeResultActions.ContainsKey(closeResult))
        {
            m_closeResultActions[closeResult]?.Invoke();
        }
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
