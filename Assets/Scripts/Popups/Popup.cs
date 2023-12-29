using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [Serializable]
    public struct ButtonReference
    {
        public GameObject Holder;
        public Button Button;
        public TextMeshProUGUI Text;
    }

    public enum PopupState
    {
        CLOSED,
        OPENING,
        OPEN,
        CLOSING,
    }

    [Header("Animation References")]
    [SerializeField] private Animator m_animator;
    [SerializeField] private string m_openTrigger = "Open";
    [SerializeField] private string m_closeTrigger = "Close";

    [Header("Close Button References")]
    [SerializeField] private GameObject m_closeButtonHolder;
    [SerializeField] private Button m_closeButton;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI m_bodyText;

    [Header("Button References")]
    [SerializeField] private ButtonReference[] m_buttons;

    public PopupType Type => PopupType.DEFAULT;

    public PopupState CurrentState { get; private set; }

    public bool IsOpeningOrOpen => CurrentState == PopupState.OPENING || CurrentState == PopupState.OPEN;

    public bool IsClosingOrClosed => CurrentState == PopupState.CLOSING || CurrentState == PopupState.CLOSED;

    public void Init(PopupData data)
    {
        // Make sure all listeners are removed from old initialisations.
        Cleanup();

        m_closeButtonHolder.SetActive(data.ShowCloseButton);

        if (data.ShowCloseButton)
        {
            m_closeButton.onClick.AddListener(data.CloseButtonAction);
            m_closeButton.onClick.AddListener(Close);
        }

        m_bodyText.text = data.BodyText;

        for(int i = 0; i < m_buttons.Length; i++)
        {
            bool isActive = i < data.ButtonDatas.Length;

            ButtonReference button = m_buttons[i];

            if (isActive)
            {
                button.Holder.SetActive(true);
                button.Button.onClick.AddListener(data.ButtonDatas[i].Action);
                button.Text.text = data.ButtonDatas[i].Text;

                if (data.ButtonDatas[i].CloseOnClick)
                {
                    button.Button.onClick.AddListener(Close);
                }
            }
            else
            {
                button.Holder.SetActive(false);
                button.Button.onClick.RemoveAllListeners();
                button.Text.text = string.Empty;
            }
        }
    }

    public void Open()
    {
        m_animator.SetTrigger(m_openTrigger);

        CurrentState = PopupState.OPENING;
    }

    public void Close()
    {
        m_animator.SetTrigger(m_closeTrigger);

        CurrentState = PopupState.CLOSING;
    }

    // Called by the animator when the open animation is complete.
    private void OnOpenComplete()
    {
        foreach (ButtonReference button in m_buttons)
        {
            button.Button.interactable = true;
        }

        CurrentState = PopupState.OPEN;
    }

    // Called by the animator when the close animation is complete.
    private void OnCloseComplete()
    {
        Cleanup();
    }

    private void Cleanup()
    {
        m_closeButtonHolder.SetActive(false);

        m_closeButton.onClick.RemoveAllListeners();

        m_bodyText.text = string.Empty;

        foreach (ButtonReference button in m_buttons)
        {
            button.Holder.SetActive(false);
            button.Button.onClick.RemoveAllListeners();
            button.Text.text = string.Empty;
            button.Button.interactable = false;
        }

        CurrentState = PopupState.CLOSED;
    }
}
