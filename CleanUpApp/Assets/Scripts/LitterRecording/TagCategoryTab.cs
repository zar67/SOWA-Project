using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagCategoryTab : MonoBehaviour
{
    [SerializeField] private Toggle m_toggle;
    [SerializeField] private TextMeshProUGUI m_titleText;

    private string m_category;

    public static Action<string> OnTabSelected;

    private void OnEnable()
    {
        m_toggle.onValueChanged.AddListener(HandleSelectButtonClicked);
    }

    private void OnDisable()
    {
        m_toggle.onValueChanged.RemoveListener(HandleSelectButtonClicked);
    }

    public void Populate(string category, ToggleGroup group, bool select = false)
    {
        m_category = category;
        m_titleText.text = category;

        m_toggle.group = group;
        m_toggle.isOn = select;
    }

    private void HandleSelectButtonClicked(bool active)
    {
        if (active)
        {
            OnTabSelected?.Invoke(m_category);
        }
    }
}
