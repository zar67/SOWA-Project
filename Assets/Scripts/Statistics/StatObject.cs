using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatObject : MonoBehaviour
{
    [SerializeField] private Image m_crownImage;
    [SerializeField] private TextMeshProUGUI m_amountText;
    [SerializeField] private TextMeshProUGUI m_typeText;

    public void Populate(string type, long amount)
    {
        m_typeText.text = type;
        m_amountText.text = amount.ToString();
    }
}
