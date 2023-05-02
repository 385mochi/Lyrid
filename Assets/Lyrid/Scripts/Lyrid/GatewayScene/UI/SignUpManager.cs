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
    /// サインアップ処理を管理するクラス
    /// </summary>
    public class SignUpManager : MonoBehaviour
    {
        [SerializeField] Text mailAddress;
        [SerializeField] Button sendButton;
        [SerializeField] Image sendButtonFrame;
        [SerializeField] BackPanel backPanel;
        [SerializeField] UserAuth userAuth;

        private void OnEnable()
        {
            Debug.Log(mailAddress.text);
            mailAddress.text = "";
            SetInteractable(false);
            DOTween.ToAlpha(
                () => sendButtonFrame.color,
                color => sendButtonFrame.color = color,
                0.2f, 0.2f
            );
        }

        /// <summary>
        /// 入力されたメールアドレスが有効かどうか調べるメソッド
        /// </summary>
        public void CheckMailAddress()
        {
            string mail = mailAddress.text;
            if (!string.IsNullOrEmpty(mail) &&
                Regex.IsMatch(mail, @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$", RegexOptions.IgnoreCase))
            {
                SetInteractable(true);
            }
            else
            {
                SetInteractable(false);
            }
        }

        /// <summary>
        /// 入力されたメールアドレスで UserAuth からサインアップを行うメソッド
        /// </summary>
        public void SendMailAddress()
        {
            SetInteractable(false);
            string mail = mailAddress.text;
            userAuth.SignUp(mail);
        }

        public void SetInteractable(bool b)
        {
            if (b)
            {
                sendButton.interactable = true;
                DOTween.ToAlpha(
                    () => sendButtonFrame.color,
                    color => sendButtonFrame.color = color,
                    1.0f, 0.2f
                );
            }
            else
            {
                sendButton.interactable = false;
                DOTween.ToAlpha(
                    () => sendButtonFrame.color,
                    color => sendButtonFrame.color = color,
                    0.2f, 0.2f
                );
            }
        }

        public void SetSignUpPanel(bool b)
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