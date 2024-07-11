using UnityEngine;

public class FTUEHighlightComponent : MonoBehaviour
{
    [SerializeField] private string m_stageID;

    [SerializeField] private GameObject m_highlightHolder;

    private void Start()
    {
        m_highlightHolder.SetActive(false);

        FTUEManager.Instance.OnSetFTUEHighlight -= ShowHighlight;
        FTUEManager.Instance.OnSetFTUEHighlight += ShowHighlight;
    }

    private void ShowHighlight(string stageID)
    {
        m_highlightHolder.SetActive(stageID == m_stageID);
    }
}
