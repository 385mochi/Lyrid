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
    /// ログアウト処理を管理するクラス
    /// </summary>
    public class LogOutManager : MonoBehaviour
    {
        [SerializeField] BackPanel backPanel;
        [SerializeField] UserAuth userAuth;

        /// <summary>
        /// UserAuth からログアウトを行うメソッド
        /// </summary>
        public void LogOut()
        {
            userAuth.LogOut();
        }

        public void SetLogOutPanel(bool b)
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