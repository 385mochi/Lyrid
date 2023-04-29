using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NCMB;
using Lyrid.Common.UI;
using Lyrid.GatewayScene.UI;

namespace Lyrid.GatewayScene
{
    /// <summary>
    /// ユーザー認証を管理するクラス
    /// </summary>
    public class UserAuth : MonoBehaviour
    {
        #region Field
        [SerializeField] private PopUpMessage popUpMessage;
        [SerializeField] private GameObject signUpButtonObj;
        [SerializeField] private GameObject logInButtonObj;
        [SerializeField] private GameObject logOutButtonObj;
        [SerializeField] private SignUpManager signUpManager;
        [SerializeField] private LogInManager logInManager;
        [SerializeField] private LogOutManager logOutManager;
        #endregion

        #region Property
        /// <summary> 現在ログイン中かどうか </summary>
        public bool IsLoggingIn { get { return (NCMBUser.CurrentUser != null); } }
        #endregion

        /// <summary>
        /// メールアドレスによって会員登録を行うメソッド
        /// </summary>
        /// <param name="mail"> メールアドレス </param>
        public void SignUp(string mail)
        {
            NCMBUser.RequestAuthenticationMailAsync(mail, (NCMBException e) => {
                if (e != null)
                {
                    if (e.ErrorMessage == "mailAddress is duplication.")
                    {
                        popUpMessage.PopUp("そのメールアドレスは既に登録されています。");
                    }
                    else
                    {
                        popUpMessage.PopUp(e.ErrorMessage);
                    }
                }
                else
                {
                    popUpMessage.PopUp("入力アドレス宛にメールが送信されました。");
                    signUpManager.SetSignUpPanel(false);
                }
            });
        }

        /// <summary>
        /// メールアドレスとパスワードでログインを行うメソッド
        /// </summary>
        /// <param name="mail"> メールアドレス </param>
        /// <param name="password"> パスワード </param>
        public void LogIn(string mail, string password)
        {
            NCMBUser.LogInWithMailAddressAsync(mail, password, (NCMBException e) => {
                if (e != null)
                {
                    if (e.ErrorMessage == "Authentication error with ID/PASS incorrect.")
                    {
                        popUpMessage.PopUp("メールアドレスまたはパスワードが間違っています。");
                    }
                    else
                    {
                        popUpMessage.PopUp("ログインできませんでした。");
                    }
                }
                else
                {
                    popUpMessage.PopUp("ログインしました。\n\n" + "ユーザー名: " + NCMBUser.CurrentUser.UserName);
                    logInManager.SetLogInPanel(false);
                    signUpButtonObj.SetActive(false);
                    logInButtonObj.SetActive(false);
                    logOutButtonObj.SetActive(true);
                }
            });
        }

        /// <summary>
        /// ログアウトするメソッド
        /// </summary>
        public void LogOut()
        {
            NCMBUser.LogOutAsync((NCMBException e) => {
                if (e != null)
                {
                    popUpMessage.PopUp("ログアウトできませんでした。");
                }
                else
                {
                    popUpMessage.PopUp("ログアウトしました。");
                    logOutManager.SetLogOutPanel(false);
                    signUpButtonObj.SetActive(true);
                    logInButtonObj.SetActive(true);
                    logOutButtonObj.SetActive(false);
                }
            });
        }
    }
}