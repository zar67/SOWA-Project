using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LitterObject : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TextMeshProUGUI m_text;

    private List<LitterData> m_data;

    private void OnEnable()
    {
        m_button.onClick.AddListener(HandleButtonClicked);
    }

    private void OnDisable()
    {
        m_button.onClick.RemoveListener(HandleButtonClicked);
    }

    private void HandleButtonClicked()
    {
        PopupManager.Instance.OpenPopup(new LitterObjectData()
        {
            Type = PopupType.LITTER_OBJECT,
            ShowCloseButton = true,
            Data = m_data
        });
    }

    public void SetData(List<LitterData> data)
    {
        m_data = data;
        m_text.text = data.Count.ToString();
    }
}
