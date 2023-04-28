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

        #region Property
        public bool isVisible { get; private set; }
        #endregion

        void Start()
        {
            isVisible = screenImageObj.activeSelf;
        }

        /// <summary>
        /// ロード画面を表示するメソッド
        /// </summary>
        /// <param name="sprite"> 画面に表示させる画像 </param>
        /// <returns> Tween のインスタンス </returns>
        public Tween SetVisible(Sprite sprite)
        {
            // 既に表示されている場合は何もしない
            if (isVisible) return DOVirtual.DelayedCall(0.1f, () => {});

            isVisible = true;

            // 背景を設定する
            screenImage = screenImageObj.GetComponent<Image>();
            screenImage.sprite = sprite;

            // 背景を表示させる
            screenImageObj.SetActive(true);
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
        /// ロード画面を表示するメソッド (画像指定なし)
        /// </summary>
        /// <returns> Tween のインスタンス </returns>
        public Tween SetVisible()
        {
            // 既に表示されている場合は何もしない
            if (isVisible) return DOVirtual.DelayedCall(0, () => {});

            isVisible = true;

            // 背景を表示させる
            screenImageObj.SetActive(true);
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
        /// <returns> Tween のインスタンス </returns>
        public Tween SetInvisible()
        {
            // 既に非表示の場合は何もしない
            if (!isVisible) return DOVirtual.DelayedCall(0.1f, () => {});

            isVisible = false;

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