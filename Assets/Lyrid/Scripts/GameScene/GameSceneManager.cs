using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Notes;
using Lyrid.GameScene.Lanes;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Input;

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
        /// <summary> NotesManager のインスタンス </summary>
        private NotesManager notesManager;
        #endregion

        #region Methods
        void Start()
        {
            // 60fps に設定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            // 譜面を生成
            chart = new Chart(testCsvFile);
            // LanesManager のインスタンスを取得し、レーンを生成する
            lanesManager = GameObject.Find("Lanes").GetComponent<LanesManager>();
            lanesManager.SetLanes(chart.maxLaneNum, chart.initLaneNum, chart.laneWidth, chart.setLaneVisible);
            // AudioManager のインスタンスを生成
            audioManager = new AudioManager(music, -0.5f);
            // TouchInputManager のインスタンスを生成
            touchInputManager = new TouchInputManager();
            // MovementManager のインスタンスを生成
            movementManager = new MovementManager();
            // JudgementManager のインスタンスを生成
            judgementManager = new JudgementManager(touchInputManager, audioPlay);
            // NotesManager のインスタンスを生成
            notesManager = new NotesManager(chart, movementManager, judgementManager);
        }

        void Update()
        {
            // AudioManager を Update し、現在の再生時間を取得
            audioManager.ManagedUpdate();
            float time = audioManager.time;
            // 各インスタンスを Update
            touchInputManager.ManagedUpdate();
            judgementManager.ManagedUpdate(time);
            notesManager.ManagedUpdate(time);
            movementManager.ManagedUpdate(time);
        }
        #endregion
    }

}