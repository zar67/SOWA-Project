using Extensions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class RecordingPanelUI : MonoBehaviour
{
    [SerializeField] private RectTransform m_panelHolder;
    [SerializeField] private float m_panelMoveSpeed;

    [SerializeField] private Button m_closeButton;
    [SerializeField] private Button m_addButton;

    [SerializeField] private Button m_changeLocationButton;
    [SerializeField] private Button m_addTagButton;

    [SerializeField] private Transform m_tagsHolder;

    public void OpenPanel()
    {
        m_panelHolder.anchoredPosition = new Vector2(m_panelHolder.anchoredPosition.x, -m_panelHolder.rect.height);
        m_panelHolder.DOAnchorPosY(0, m_panelMoveSpeed);
    }

    public void ClosePanel()
    {
        m_panelHolder.anchoredPosition = new Vector2(m_panelHolder.anchoredPosition.x, 0);
        m_panelHolder.DOAnchorPosY(-m_panelHolder.rect.height, m_panelMoveSpeed)
            .OnComplete(() => m_tagsHolder.DestroyChildren(true));
    }

    private void OnEnable()
    {
        m_closeButton.onClick.AddListener(ClosePanel);
        m_addButton.onClick.AddListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.AddListener(ChangeLocation);
        m_addTagButton.onClick.AddListener(AddTag);
    }

    private void OnDisable()
    {
        m_closeButton.onClick.RemoveListener(ClosePanel);
        m_addButton.onClick.RemoveListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.RemoveListener(ChangeLocation);
        m_addTagButton.onClick.RemoveListener(AddTag);
    }

    private void RecordLitterAndClose()
    {
        // Create litter data and send to database.
        ClosePanel();
    }

    private void ChangeLocation()
    {
        // For future, add the ability to input a specific location.
    }

    private void AddTag()
    {
        // For future, add tags to litter to specify material, type etc.
    }
}
