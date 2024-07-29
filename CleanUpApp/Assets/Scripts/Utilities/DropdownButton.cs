using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownButton : MonoBehaviour
{
    public static event Action<string> SelectButtonClicked;

    [SerializeField] private TextMeshProUGUI m_tagText;
    [SerializeField] private Button m_selectButton;

    public string Value => m_tagText.text;

    public void PopulateOption(string value)
    {
        m_tagText.text = value;
    }

    private void OnEnable()
    {
        m_selectButton.onClick.AddListener(HandleSelectClicked);
    }

    private void OnDisable()
    {
        m_selectButton.onClick.RemoveListener(HandleSelectClicked);
    }

    private void HandleSelectClicked()
    {
        SelectButtonClicked?.Invoke(m_tagText.text);
    }
}
