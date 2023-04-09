using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using static Lyrid.Common.Easings;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Input;
using Lyrid.GameScene.Lanes;
using Lyrid.GameScene.Notes;
using Lyrid.GameScene.Score;

namespace Lyrid.GameScene
{
    /// <summary>
    /// GameScene を管理するクラス
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        #region Field
        /// <summary> オートプレイかどうか </summary>
        [SerializeField] bool audioPlay;
        /// <summary> 譜面の csv ファイル </summary>
        [SerializeField] private TextAsset testCsvFile;
        /// <summary> 音源の AudioSource </summary>
        [SerializeField] private AudioSource music;
        /// <summary> リセットボタン </summary>
        [SerializeField] private ReplayButton replayButton;
        /// <summary> リセット中の背景のオブジェクト </summary>
        [SerializeField] private GameObject frontImageObj;
        /// <summary> ロード中のアイコン </summary>
        [SerializeField] private Image loadingIcon;
        /// <summary> 譜面のインスタンス </summary>
        private Chart chart;
        /// <summary> LanesManager のインスタンス </summary>
        private LanesManager lanesManager;
        /// <summary> AudioManager のインスタンス </summary>
        private AudioManager audioManager;
        /// <summary> TouchInputManager のインスタンス </summary>
        private TouchInputManager touchInputManager;
        /// <summary> MovementManager のインスタンス </summary>
        private MovementManager movementManager;
        /// <summary> JudgementManager のインスタンス </summary>
        private JudgementManager judgementManager;
        /// <summary> ScoreManager のインスタンス </summary>
        private ScoreManager scoreManager;
        /// <summary> NotesManager のインスタンス </summary>
        private NotesManager notesManager;
        #endregion

        #region Methods
        void OnEnable()
        {
            // 60fps に設定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Init();
        }

        void Update()
        {
            // AudioManager を Update し、現在の再生時間を取得
            audioManager.ManagedUpdate();
            float time = audioManager.time;
            // 各 Manager を Update
            touchInputManager.ManagedUpdate();
            notesManager.ManagedUpdate(time);
            movementManager.ManagedUpdate(time);
            judgementManager.ManagedUpdate(time);
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        private void Init()
        {
            // DOTween を初期化
            DOTween.Init();

            // 譜面を生成
            chart = new Chart(testCsvFile);

            // LanesManager のインスタンスを取得し、レーンを生成する
            lanesManager = GameObject.Find("Lanes").GetComponent<LanesManager>();
            lanesManager.SetLanes(chart.maxLaneNum, chart.initLaneNum, chart.laneWidth, chart.setLaneVisible);

            // 各 Manager のインスタンスを生成
            audioManager = new AudioManager(music, -2.0f);
            touchInputManager = new TouchInputManager();
            movementManager = new MovementManager();
            scoreManager = new ScoreManager(chart.totalJudgementTargetsNum);
            judgementManager = new JudgementManager(scoreManager, touchInputManager, audioPlay);
            notesManager = new NotesManager(chart, movementManager, judgementManager);
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            // 背景画像
            frontImageObj.SetActive(true);
            Image frontImage = frontImageObj.GetComponent<Image>();

            // 動作しているものを全て Kill
            DOTween.KillAll();

            // 背景でマスクする
            DOTween.ToAlpha(
                () => loadingIcon.color,
                color => loadingIcon.color = color,
                1.0f, // 目標値
                0.2f  // 所要時間
            ).SetEase(GetEaseType(1));
            DOTween.ToAlpha(
                () => frontImage.color,
                color => frontImage.color = color,
                1.0f, // 目標値
                0.2f  // 所要時間
            ).SetEase(GetEaseType(1)).OnComplete(() => {

                // オブジェクトプールをリセット
                GameObject.FindWithTag("NoteObjectPool").GetComponent<ObjectPool>().Reset();
                GameObject.FindWithTag("SlideNoteLineObjectPool").GetComponent<ObjectPool>().Reset();

                // 各 Manager をリセット
                audioManager.Reset();
                movementManager.Reset();
                judgementManager.Reset();
                scoreManager.Reset();
                notesManager.Reset();
                lanesManager.Reset();
                touchInputManager.Reset();

                // ボタンの状態を初期化
                replayButton.Reset();

                // 背景を消す
                DOTween.ToAlpha(
                    () => loadingIcon.color,
                    color => loadingIcon.color = color,
                    0.0f, // 目標値
                    0.2f  // 所要時間
                ).SetDelay(1.0f).SetEase(GetEaseType(1));
                DOTween.ToAlpha(
                    () => frontImage.color,
                    color => frontImage.color = color,
                    0.0f, // 目標値
                    0.2f  // 所要時間
                ).SetEase(GetEaseType(1)).SetDelay(1.0f).OnComplete(() => {
                    frontImageObj.SetActive(false);
                });
            });
        }
        #endregion
    }

}