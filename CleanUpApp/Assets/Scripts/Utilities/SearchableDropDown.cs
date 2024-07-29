using Extensions;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchableDropDown : MonoBehaviour
{
    [SerializeField] private DropdownButton m_dropdownItemPrefab = null;
    [SerializeField] private int m_maxScrollRectSize = 180;

    [SerializeField] private TMP_InputField m_inputField = null;
    [SerializeField] private ScrollRect m_scrollRect = null;
    [SerializeField] private RectTransform m_scrollRectTransform;
    [SerializeField] private RectTransform m_content = null;

    private List<string> m_dropdownOptions = new List<string>();
    private List<DropdownButton> m_initializedItems = new List<DropdownButton>();

    public event Action OnSelect;
    public event Action OnDeselect;
    public event Action<string> OnValueChanged;

    public string Value => m_inputField.text;

    public void SetOptions(string[] options)
    {
        m_dropdownOptions = new List<string>(options);
        SetupScrollItems();
    }

    public void Clear()
    {
        m_inputField.text = string.Empty;
        SetScrollActive(false);
    }

    private void OnEnable()
    {
        m_inputField.onValueChanged.AddListener(HandleInputValueChanged);
        m_inputField.onSelect.AddListener(HandleSelect);
        m_inputField.onDeselect.AddListener(HandleDeselect);
        DropdownButton.SelectButtonClicked += HandleItemSelected;
    }

    private void OnDisable()
    {
        m_inputField.onValueChanged.RemoveListener(HandleInputValueChanged);
        m_inputField.onSelect.RemoveListener(HandleSelect);
        m_inputField.onDeselect.RemoveListener(HandleDeselect);
        DropdownButton.SelectButtonClicked -= HandleItemSelected;
    }

    private void SetupScrollItems()
    {
        m_content.DestroyChildren();
        m_initializedItems = new List<DropdownButton>();

        foreach (string option in m_dropdownOptions)
        {
            DropdownButton newOption = Instantiate(m_dropdownItemPrefab, m_content);
            newOption.PopulateOption(option);
            m_initializedItems.Add(newOption);
        }

        FilterDropdown();
    }

    private void ResizeScrollRect()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)m_content.transform);

        float length = Mathf.Min(m_maxScrollRectSize, m_content.sizeDelta.y + 5);
        m_scrollRectTransform.sizeDelta = new Vector2(m_scrollRectTransform.sizeDelta.x, length);
    }

    private void HandleInputValueChanged(string newValue)
    {
        FilterDropdown(newValue);
    }

    private void HandleSelect(string value)
    {
        OnSelect?.Invoke();
    }

    private void HandleDeselect(string value)
    {
        OnDeselect?.Invoke();
    }

    private void FilterDropdown(string input = "")
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            foreach (DropdownButton option in m_initializedItems)
            {
                option.gameObject.SetActive(true);
            }

            ResizeScrollRect();
            m_scrollRect.gameObject.SetActive(false);
            return;
        }

        int count = 0;
        foreach (DropdownButton option in m_initializedItems)
        {
            bool isValidOption = option.Value.ToLower().Contains(input.ToLower());

            option.gameObject.SetActive(isValidOption);

            if (isValidOption)
            {
                count++;
            }
        }

        SetScrollActive(count > 0);
        ResizeScrollRect();
    }

    private void SetScrollActive(bool active)
    {
        m_scrollRect.gameObject.SetActive(active);
    }

    private void HandleItemSelected(string value)
    {
        m_inputField.text = value;

        Clear();
        OnValueChanged?.Invoke(value);
    }
}
