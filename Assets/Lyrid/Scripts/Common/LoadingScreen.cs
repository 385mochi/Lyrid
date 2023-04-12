using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.Easings;

namespace Lyrid.Common
{
    /// <summary>
    /// ロード中の画面を管理するクラス
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        #region Field
        [SerializeField] private GameObject screenImageObj;
        private Image screenImage;
        [SerializeField] private GameObject loadingIconObj;
        #endregion

        /// <summary>
        /// ロード画面を表示するメソッド
        /// </summary>
        /// <param name="sprite"> 画面に表示させる画像 </param>
        /// <returns> Tweener のインスタンス </returns>
        public Tweener SetVisible(Sprite sprite)
        {
            screenImage = screenImageObj.GetComponent<Image>();
            screenImage.sprite = sprite;
            screenImageObj.SetActive(true);

            // 背景でマスクする
            loadingIconObj.transform.DOScale(1.0f, 0.2f).SetEase(GetEaseType(1));
            Tweener tweener = DOTween.ToAlpha(
                () => screenImage.color,
                color => screenImage.color = color,
                1.0f, // 目標値
                0.2f  // 所要時間
            ).SetEase(GetEaseType(1));
            return tweener;
        }

        /// <summary>
        /// ロード画面を非表示にするメソッド
        /// </summary>
        /// <returns> Tweener のインスタンス </returns>
        public Tweener SetInvibible()
        {
            // 背景を消す
            loadingIconObj.transform.DOScale(0.0f, 0.2f).SetDelay(1.0f).SetEase(GetEaseType(1));
            Tweener tweener = DOTween.ToAlpha(
                () => screenImage.color,
                color => screenImage.color = color,
                0.0f, // 目標値
                0.2f  // 所要時間
            ).SetEase(GetEaseType(1)).SetDelay(1.0f).OnComplete(() =>
            {
                screenImageObj.SetActive(false);
            });
            return tweener;
        }
    }
}