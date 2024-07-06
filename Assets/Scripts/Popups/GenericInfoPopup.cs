using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenericInfoPopupData : BasePopupData
{
    public string BodyText;

    public PopupButtonData[] ButtonDatas;
}

[Serializable]
public class PopupButtonData
{
    public string Text;
    public UnityAction Action;
    public bool CloseOnClick;
}

public class GenericInfoPopup : BasePopup
{
    [Serializable]
    public struct ButtonReference
    {
        public GameObject Holder;
        public Button Button;
        public TextMeshProUGUI Text;
    }

    [Header("Text References")]
    [SerializeField] protected TextMeshProUGUI m_bodyText;

    [Header("Button References")]
    [SerializeField] protected ButtonReference[] m_buttons;

    public override PopupType Type => PopupType.GENERIC_INFO;

    public override void Init(BasePopupData data)
    {
        base.Init(data);

        var infoData = data as GenericInfoPopupData;

        m_bodyText.text = infoData.BodyText;

        for(int i = 0; i < m_buttons.Length; i++)
        {
            bool isActive = i < infoData.ButtonDatas.Length;

            ButtonReference button = m_buttons[i];

            if (isActive)
            {
                button.Holder.SetActive(true);
                if (infoData.ButtonDatas[i].Action != null)
                {
                    button.Button.onClick.AddListener(infoData.ButtonDatas[i].Action);
                }
                button.Text.text = infoData.ButtonDatas[i].Text;

                if (infoData.ButtonDatas[i].CloseOnClick)
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

    // Called by the animator when the open animation is complete.
    protected override void HandleOpenComplete()
    {
        base.HandleOpenComplete();

        foreach (ButtonReference button in m_buttons)
        {
            button.Button.interactable = true;
        }
    }

    protected override void Cleanup()
    {
        base.Cleanup();

        m_bodyText.text = string.Empty;

        foreach (ButtonReference button in m_buttons)
        {
            button.Holder.SetActive(false);
            button.Button.onClick.RemoveAllListeners();
            button.Text.text = string.Empty;
            button.Button.interactable = false;
        }
    }
}
