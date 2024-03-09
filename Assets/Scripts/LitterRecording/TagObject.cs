using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TagObject : MonoBehaviour
{
    public static event Action<string> RemoveTagClicked;

    [SerializeField] private TextMeshProUGUI m_tagText;
    [SerializeField] private Button m_removeButton;

    public void PopulateTag(string tag)
    {
        m_tagText.text = tag;
    }

    private void OnEnable()
    {
        m_removeButton.onClick.AddListener(RemoveTag);
    }

    private void OnDisable()
    {
        m_removeButton.onClick.RemoveListener(RemoveTag);
    }

    private void RemoveTag()
    {
        RemoveTagClicked?.Invoke(m_tagText.text);
        Destroy(gameObject);
    }
}
