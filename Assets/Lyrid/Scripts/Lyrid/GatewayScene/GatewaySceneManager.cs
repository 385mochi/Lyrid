using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using Lyrid.Common.UI;
using Lyrid.GatewayScene.UI;

namespace Lyrid.GatewayScene
{
    /// <summary>
    /// GatewayScene を管理するクラス
    /// </summary>
    public class GatewaySceneManager : MonoBehaviour
    {
        [SerializeField] private string serverURL;
        [SerializeField] private UserAuth userAuth;
        [SerializeField] private PopUpMessage popUpMessage;
        [SerializeField] private CheckDownloadManager checkDownloadManager;
        [SerializeField] private AssetLoader assetLoader;
        [SerializeField] private GameObject signUpButtonObj;
        [SerializeField] private GameObject logInButtonObj;
        [SerializeField] private GameObject logOutButtonObj;
        [SerializeField] private GameObject progressBarObj;
        [SerializeField] private Text titleMessage;
        [SerializeField] private GameObject homeSceneObj;
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private Sprite titleImage;
        private bool isProcessing;
        private List<string> keys;

        private void Start()
        {
            // 既にログイン済みの場合の処理
            if (userAuth.IsLoggingIn)
            {
                signUpButtonObj.SetActive(false);
                logInButtonObj.SetActive(false);
                logOutButtonObj.SetActive(true);
            }
            titleMessage.text = "タッチして開始";
            isProcessing = false;
        }

        public void StartInitProcessing()
        {
            // 処理中ならなにもしない
            if (isProcessing) return;
            isProcessing = true;

            // 右下ボタンを非表示にする
            signUpButtonObj.SetActive(false);
            logInButtonObj.SetActive(false);
            logOutButtonObj.SetActive(false);

            // サーバーに接続できるか調べる
            titleMessage.text = "サーバーに接続しています...";
            StartCoroutine(CheckServer(FinishedServerCheck));
        }

        private IEnumerator CheckServer(Action<bool> callback, int timeOut = 2)
        {
            // 回線に繋がっているかチェック
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                callback(false);
                yield break;
            }

            // サーバーに接続できるかチェック
            var request = new UnityWebRequest(serverURL) {timeout = timeOut};
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"サーバー接続確認失敗[{serverURL}] : result {request.result} : error {request.error}");
                callback(false);
                yield break;
            }

            callback(true);
            yield break;
        }

        private void FinishedServerCheck(bool success)
        {
            if (success)
            {
                CheckRemoteContents();
            }
            else
            {
                popUpMessage.PopUp("サーバーに接続できませんでした。");
                ExitProcessing();
            }
        }

        private async void CheckRemoteContents()
        {
            titleMessage.text = "追加データを確認中...";

            // key の情報をロードする
            keys = new List<string>();
            var csvFile = await assetLoader.LoadTextAssetAsync("keys");
            StringReader reader = new StringReader(csvFile.text);
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                if (line != "") keys.Add(line);
                Debug.Log(line);
            }
            reader.Close();

            // 各アセットの合計容量を取得する
            var handle = Addressables.GetDownloadSizeAsync(keys);
            var size = handle.WaitForCompletion();

            // ダウンロードするものがあれば確認する
            if (size > 0)
            {
                checkDownloadManager.SetSize(size);
                checkDownloadManager.SetCheckDownloadPanel(true);
            }
            else
            {
                EndInitProcessing();
            }
        }

        public void DownloadRemoteContents()
        {
            titleMessage.text = "追加データをダウンロード中...";
            progressBarObj.SetActive(true);
            Slider progressBar = progressBarObj.GetComponent<Slider>();
            for (int i = 0; i < keys.Count; i++)
            {
                titleMessage.text = $"追加データをダウンロード中 ({i+1}/{keys.Count}))";
                var handle = Addressables.DownloadDependenciesAsync(keys[i], true);
                handle.WaitForCompletion();
                progressBar.value = (i + 1.0f) / keys.Count;
            }
            EndInitProcessing();
        }

        public void EndInitProcessing()
        {
            titleMessage.text = "ゲームを起動します...";
            LoadHomeScene();
        }

        /// <summary>
        /// HomeScene をロードするメソッド
        /// </summary>
        public void LoadHomeScene()
        {
            // AssetLoader の handle を解放
            assetLoader.ReleaseAll();

            // ロード画面を表示させる
            DOVirtual.DelayedCall (1.0f, () => {
                loadingScreen.SetVisible(titleImage).OnComplete(() =>
                {
                    // HomeScene を表示
                    homeSceneObj.SetActive(true);

                    // GatewayScene を非表示にする
                    gameObject.SetActive(false);
                });
            });
        }

        public void ExitProcessing()
        {
            // 既にログイン済みの場合の処理
            if (userAuth.IsLoggingIn)
            {
                signUpButtonObj.SetActive(false);
                logInButtonObj.SetActive(false);
                logOutButtonObj.SetActive(true);
            }
            else
            {
                signUpButtonObj.SetActive(true);
                logInButtonObj.SetActive(true);
                logOutButtonObj.SetActive(false);
            }
            titleMessage.text = "タッチして開始";
            isProcessing = false;
        }
    }
}