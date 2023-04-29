using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.Easings;

namespace Lyrid.Common.UI
{
    /// <summary>
    /// ポップアップメッセージを管理するクラス
    /// </summary>
    public class PopUpMessage : MonoBehaviour
    {
        #region Field
        [SerializeField] private Text messageText;
        [SerializeField] private BackPanel backPanel;
        [SerializeField] private GameObject messagePanel;
        #endregion

        #region Methods
        public void PopUp(string message)
        {
            messageText.text = message;
            backPanel.SetPanel(true);
            messagePanel.transform.localScale = new Vector3(0, 0, 0);
            messagePanel.transform.DOScale(new Vector3(1, 1, 1), 0.4f)
            .SetEase(GetEaseType(15));
            gameObject.SetActive(true);
        }

        public void Close()
        {
            backPanel.SetPanel(false);
            messagePanel.transform.DOScale(new Vector3(0, 0, 0), 0.4f)
            .SetEase(GetEaseType(15))
            .OnComplete(() => { gameObject.SetActive(false); });
        }
        #endregion
    }
}