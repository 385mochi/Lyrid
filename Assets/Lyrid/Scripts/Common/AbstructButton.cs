using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Lyrid.Common
{
    /// <summary>
    /// 全てのボタンの親クラスとなる抽象クラス
    /// </summary>
    public abstract class AbstractButton : MonoBehaviour
    {
        #region Field
        protected Button button;
        private bool enableTripleTap;
        private bool isActive;
        private float timeLimit;
        private int counter;
        private float timer;
        private Image buttonImage;
        #endregion

        #region Constructor
        public AbstractButton(Button button, bool enableTripleTap)
        {
            this.button = button;
            this.enableTripleTap = enableTripleTap;
            isActive = true;
            timeLimit = 1.0f;
            counter = 0;
            timer = 0;
            buttonImage = button.GetComponent<Image>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// ボタンの機能を実行する抽象メソッド
        /// </summary>
        public abstract void Execute();

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
                    DOTween.ToAlpha(
                        () => buttonImage.color,
                        color => buttonImage.color = color,
                        0f,  // 目標値
                        0.5f // 所要時間
                    );
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
                DOTween.ToAlpha(
                    () => buttonImage.color,
                    color => buttonImage.color = color,
                    (counter / 3.0f), // 目標値
                    0.5f              // 所要時間
                );
            }
        }

        /// <summary>
        /// ボタンを初期化するメソッド
        /// </summary>
        protected void Reset()
        {
            isActive = true;
            counter = 0;
            timer = 0;
        }
        #endregion
    }
}