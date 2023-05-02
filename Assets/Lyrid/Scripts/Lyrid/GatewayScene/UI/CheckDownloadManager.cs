using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.Easings;
using Lyrid.Common.UI;
using Lyrid.GatewayScene;

namespace Lyrid.GatewayScene.UI
{
    /// <summary>
    /// 追加データの容量確認を管理するクラス
    /// </summary>
    public class CheckDownloadManager : MonoBehaviour
    {
        [SerializeField] BackPanel backPanel;
        [SerializeField] Text checkDownloadText;

        public void SetSize(long size)
        {
            string sizeText = string.Format("{0:F2}",  (float)(size / (1024f * 1024f)));
            checkDownloadText.text = $"追加データ ({sizeText}MB) をダウンロードします。\n" +
                "よろしいですか？\n(Wifi環境でのダウンロードを推奨します。)";
        }

        public void SetCheckDownloadPanel(bool b)
        {
            if (b)
            {
                backPanel.SetPanel(true);
                gameObject.transform.localScale = new Vector3(0, 0, 0);
                gameObject.SetActive(true);
                gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.4f)
                .SetEase(GetEaseType(15));
            }
            else
            {
                backPanel.SetPanel(false);
                gameObject.transform.DOScale(new Vector3(0, 0, 0), 0.4f)
                .SetEase(GetEaseType(15))
                .OnComplete(() => { gameObject.SetActive(false); });
            }
        }
    }
}