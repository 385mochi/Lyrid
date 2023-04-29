using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.Easings;
using Lyrid.Common.UI;
using Lyrid.GatewayScene.UI;

namespace Lyrid.GatewayScene
{
    /// <summary>
    /// GatewayScene を管理するクラス
    /// </summary>
    public class GatewaySceneManager : MonoBehaviour
    {
        [SerializeField] private UserAuth userAuth;
        [SerializeField] private GameObject signUpButtonObj;
        [SerializeField] private GameObject logInButtonObj;
        [SerializeField] private GameObject logOutButtonObj;

        private void Start()
        {
            // 既にログイン済みの場合の処理
            if (userAuth.IsLoggingIn)
            {
                signUpButtonObj.SetActive(false);
                logInButtonObj.SetActive(false);
                logOutButtonObj.SetActive(true);
            }
        }
    }
}