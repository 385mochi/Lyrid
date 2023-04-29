using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.Easings;

namespace Lyrid.Common.UI
{
    /// <summary>
    /// メッセージが表示されるときの背景のクラス
    /// </summary>
    public class BackPanel : MonoBehaviour
    {
        [SerializeField] Image panel;
        public void SetPanel(bool b)
        {
            if (b)
            {
                panel.color = new Color(0, 0, 0, 0);
                gameObject.SetActive(true);
                DOTween.ToAlpha(
                    () => panel.color,
                    color => panel.color = color,
                    0.85f, // 目標値
                    0.2f // 所要時間
                );
            }
            else
            {
                DOTween.ToAlpha(
                    () => panel.color,
                    color => panel.color = color,
                    0.0f, // 目標値
                    0.2f // 所要時間
                ).OnComplete(() => { gameObject.SetActive(false); });
            }
        }
    }
}