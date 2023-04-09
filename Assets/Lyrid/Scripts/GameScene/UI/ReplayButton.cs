using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.GameScene;

namespace Lyrid.Common
{
    /// <summary>
    /// リプレイボタンのクラス
    /// </summary>
    public class ReplayButton : MonoBehaviour
    {
        #region Field
        [SerializeField] bool enableTripleTap;
        private bool isActive;
        private float timeLimit;
        private int counter;
        private float timer;
        private Image buttonImage;
        /// <summary> DOTween のための Tweener インスタンス </summary>
        private Tweener tweener;
        #endregion

        #region Methods
        void OnEnable()
        {
            isActive = true;
            timeLimit = 1.0f;
            counter = 0;
            timer = 0;
            buttonImage = gameObject.GetComponent<Image>();
        }

        void Update()
        {
            // アクティブでなければなにもしない
            if (!isActive)
            {
                return;
            }
            // 3 連続タップが有効でなければなにもしない
            if (!enableTripleTap)
            {
                return;
            }
            // 3 連続タップかどうか判定
            if (timer > timeLimit)
            {
                // timeLimit を超過したらカウンタをリセット
                if (counter != 0)
                {
                    Color color = buttonImage.color;
                    color.a = counter / 3.0f;
                    buttonImage.color = color;
                    if (tweener != null)
                    {
                        tweener.Kill();
                    }
                    tweener = DOTween.ToAlpha(
                        () => buttonImage.color,
                        color => buttonImage.color = color,
                        0f,  // 目標値
                        0.5f // 所要時間
                    ).OnComplete(() => {tweener = null;});
                }
                counter = 0;
                return;
            }
            else {
                // タイマーを進め，カウンタが 3 以上になったらボタンの機能を実行する
                timer += Time.deltaTime;
                if (counter >= 3)
                {
                    Execute();
                }
            }
        }

        /// <summary>
        /// ボタンの機能を実行するメソッド
        /// </summary>
        public void Execute()
        {
            isActive = false;
            GameObject.Find("GameScene").GetComponent<GameSceneManager>().Reset();
        }

        /// <summary>
        /// ボタンが押されたときに実行するメソッド
        /// </summary>
        public void OnClick()
        {
            // アクティブでなければ何もしない
            if (!isActive)
            {
                return;
            }
            // 3 連タップが有効でなければボタンの機能を実行する
            if (!enableTripleTap)
            {
                Execute();
            }
            // そうでなければタイマーをリセットしてカウンタを進める
            else {
                timer = 0;
                counter++;
                Color color = buttonImage.color;
                color.a = (counter-1) / 3.0f;
                buttonImage.color = color;
                if (tweener != null)
                {
                    tweener.Kill();
                }
                tweener = DOTween.ToAlpha(
                    () => buttonImage.color,
                    color => buttonImage.color = color,
                    (counter / 3.0f), // 目標値
                    0.5f              // 所要時間
                ).OnComplete(() => {tweener = null;});
            }
        }

        /// <summary>
        /// ボタンを初期化するメソッド
        /// </summary>
        public void Reset()
        {
            isActive = true;
            counter = 0;
            timer = 0;
            Color color = buttonImage.color;
            color.a = 0;
            buttonImage.color = color;
        }
        #endregion
    }
}