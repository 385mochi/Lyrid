using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CriWare;
using CriWare.Assets;
using DG.Tweening;
using Lyrid.Common;
using static Lyrid.Common.Easings;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Input;
using Lyrid.GameScene.Lanes;
using Lyrid.GameScene.Notes;
using Lyrid.GameScene.Score;
using Lyrid.GameScene.UI;
using Lyrid.ResultScene;

namespace Lyrid.GameScene
{
    /// <summary>
    /// GameScene を管理するクラス
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        #region Field
        /// <summary> オートプレイかどうか </summary>
        [SerializeField] bool autoPlay;
        /// <summary> 譜面の csv ファイル </summary>
        [SerializeField] private TextAsset testCsvFile;
        /// <summary> リセットボタン </summary>
        [SerializeField] private ReplayButton replayButton;
        /// <summary> ロード画面 </summary>
        [SerializeField] private LoadingScreen loadingScreen;
        /// <summary> 楽曲ジャケット画像のアイコン </summary>
        [SerializeField] private Image musicImage;
        /// <summary> 背景画像 </summary>
        [SerializeField] private Image backGroundImage;
        /// <summary> 楽曲名のテキスト </summary>
        [SerializeField] private Text musicNameText;
        /// <summary> 作曲者名のテキスト </summary>
        [SerializeField] private Text composerNameText;
        /// <summary> リザルトシーン </summary>
        [SerializeField] private GameObject resultScene;
        /// <summary> AssetLoader のインスタンス </summary>
        private AssetLoader assetLoader;
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
        /// <summary> NoteManager のインスタンス </summary>
        private NoteManager noteManager;
        /// <summary> NoteLineManager のインスタンス </summary>
        private NoteLineManager noteLineManager;
        /// <summary> TimeLine のインスタンス </summary>
        private TimeLine timeLine;
        /// <summary> GameScene の状態を表す列挙型 </summary>
        private enum Status { Start, Playing, Ended }
        /// <summary> GameScene の状態 </summary>
        private Status status;
        #endregion

        #region Methods
        void OnEnable()
        {
            Init("saisyuu_koufukuron");
        }

        void Update()
        {
            if (status == Status.Playing)
            {
                // AudioManager を Update し、現在の再生時間を取得
                audioManager.ManagedUpdate();
                float time = audioManager.time;

                // AudioManager の状態が Ended に変化したら、状態を Ended とする
                if (audioManager.status == AudioManager.Status.Ended)
                {
                    status = Status.Ended;
                    OnTrackComplete();
                }

                // 各 Manager を Update
                touchInputManager.ManagedUpdate();
                judgementManager.ManagedUpdate(time);
                movementManager.ManagedUpdate(time);
                noteManager.ManagedUpdate(time);
                noteLineManager.ManagedUpdate();

                // タイムラインを更新
                timeLine.UpdateLine(time);
            }
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public async void Init(string key)
        {
            // ロード画面を表示させる
            loadingScreen.SetVisible();

            // 状態を初期化
            status = Status.Start;

            // AssetLoader のインスタンスを生成
            assetLoader = new AssetLoader();

            // 楽曲名と作曲者の情報を取得
            string musicName = "最終幸福論";
            string composerName = "∑";
            musicNameText.text = musicName;
            composerNameText.text = composerName;

            // 楽曲のジャケット画像をロード・設定
            musicImage.sprite = await assetLoader.LoadSpriteAsync($"{key}_image");
            backGroundImage.sprite = await assetLoader.LoadSpriteAsync($"{key}_image_blurred");

            // 楽曲をロード・設定
            CriAtomExAcb music = await assetLoader.LoadAudioAsync($"{key}_audio");
            CriAtomEx.CueInfo cueInfo;
            music.GetCueInfo(0, out cueInfo);
            float musicLength = cueInfo.length * 0.001f;

            // タップ音をロード
            CriAtomExAcb tapSound = await assetLoader.LoadAudioAsync("tapsound");

            // DOTween を初期化
            DOTween.Init();

            // 譜面を生成
            chart = new Chart(testCsvFile);

            // LanesManager のインスタンスを取得し、レーンを生成する
            lanesManager = GameObject.Find("Lanes").GetComponent<LanesManager>();
            lanesManager.SetLanes(chart.maxLaneNum, chart.initLaneNum, chart.laneWidth, chart.setLaneVisible);

            // 各 Manager のインスタンスを生成
            audioManager = new AudioManager(music, tapSound, -2.0f);
            touchInputManager = new TouchInputManager();
            movementManager = new MovementManager();
            scoreManager = new ScoreManager(chart.totalJudgementTargetsNum);
            judgementManager = new JudgementManager(audioManager, scoreManager, touchInputManager, autoPlay);
            noteLineManager = new NoteLineManager();
            noteManager = new NoteManager(chart, movementManager, judgementManager, noteLineManager);

            // タイムラインを取得
            timeLine = GameObject.Find("TimeLine").GetComponent<TimeLine>();
            timeLine.SetLength(musicLength);

            // 60fps に設定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;

            // 状態を更新
            status = Status.Playing;

            // ロード画面を非表示にする
            loadingScreen.SetInvisible();
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            // 状態を初期化
            status = Status.Start;

            // 動作している DOTween を全て Kill
            DOTween.KillAll();

            // ロード画面を表示させる
            loadingScreen.SetVisible(backGroundImage.sprite).OnComplete(() =>
            {
                // オブジェクトプールをリセット
                GameObject.FindWithTag("NotePool").GetComponent<ObjectPool>().Reset();
                GameObject.FindWithTag("SlideNoteLinePool").GetComponent<ObjectPool>().Reset();
                GameObject.FindWithTag("NoteLinePool").GetComponent<ObjectPool>().Reset();

                // 各 Manager をリセット
                audioManager.Reset();
                movementManager.Reset();
                judgementManager.Reset();
                scoreManager.Reset();
                noteManager.Reset();
                noteLineManager.Reset();
                lanesManager.Reset();
                touchInputManager.Reset();

                // タイムラインをリセット
                timeLine.Reset();

                // ボタンの状態を初期化
                replayButton.Reset();

                // ロード画面を非表示にする
                loadingScreen.SetInvisible();

                // 状態を更新
                status = Status.Playing;
            });
        }

        /// <summary>
        /// 楽曲の再生終了時の処理を行うメソッド
        /// </summary>
        private void OnTrackComplete()
        {
            // ロード画面を表示させる
            loadingScreen.SetVisible(backGroundImage.sprite).OnComplete(() =>
            {
                // リザルトシーンを表示
                resultScene.GetComponent<ResultSceneManager>().Init(
                    musicImage.sprite,
                    backGroundImage.sprite,
                    musicNameText.text,
                    composerNameText.text,
                    8,
                    scoreManager.score,
                    true,
                    scoreManager.isFullChain,
                    scoreManager.isAllPerfect,
                    scoreManager.perfectNum,
                    scoreManager.greatNum,
                    scoreManager.goodNum,
                    scoreManager.badNum,
                    scoreManager.missNum,
                    scoreManager.maxChain,
                    scoreManager.scoreTargetNum,
                    scoreManager.accuracy
                );
                gameObject.SetActive(false);
            });
        }
        #endregion
    }

}