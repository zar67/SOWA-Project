using UnityEngine;
using UnityEngine.UI;

public class LitterRecordingPopup : BasePopup
{
    [Header("Add Button References")]
    [SerializeField] private Button m_addButton;

    [Header("Location References")]
    [SerializeField] private Button m_changeLocationButton;

    [Header("Tags References")]
    [SerializeField] private Button m_addTagButton;
    [SerializeField] private Transform m_tagsHolder;

    public override PopupType Type => PopupType.LITTER_RECORDING;

    private void OnEnable()
    {
        m_addButton.onClick.AddListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.AddListener(ChangeLocation);
        m_addTagButton.onClick.AddListener(AddTag);
    }

    private void OnDisable()
    {
        m_addButton.onClick.RemoveListener(RecordLitterAndClose);
        m_changeLocationButton.onClick.RemoveListener(ChangeLocation);
        m_addTagButton.onClick.RemoveListener(AddTag);
    }

    private void RecordLitterAndClose()
    {
        LitterRecordingManager.Instance.RecordLitter();
        Close();
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
