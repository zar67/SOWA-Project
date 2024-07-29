using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordBinController : MonoBehaviour
{
    [SerializeField] private Button m_binButton;

    private void OnEnable()
    {
        m_binButton.onClick.AddListener(HandleRecordBinClicked);
    }

    private void OnDisable()
    {
        m_binButton.onClick.RemoveListener(HandleRecordBinClicked);
    }

    private void HandleRecordBinClicked()
    {
        PopupManager.Instance.OpenPopup(new GenericInfoPopupData()
        {
            BodyText = "By recording a bin location you are helping others find the easier way to dispose of any litter they find. Please note that bin locations will not update immediately.",
            ShowCloseButton = true,
            Type = PopupType.GENERIC_INFO,
            ButtonDatas = new PopupButtonData[]
            {
                new PopupButtonData()
                {
                    Text = "Record New Bin",
                    CloseOnClick = true,
                    Action = BinRecordingManager.Instance.RequestNewBinLocation,
                    CloseResult = "add_bin"
                }
            }
        });
    }
}

