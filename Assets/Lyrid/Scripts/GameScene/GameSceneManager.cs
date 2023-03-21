using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Notes;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Input;

namespace Lyrid.GameScene
{
    // GameScene を管理するクラス
    public class GameSceneManager : MonoBehaviour
    {
        #region Field
        // オートプレイかどうか
        [SerializeField] bool audioPlay;
        // 譜面の csv ファイル
        [SerializeField] private TextAsset testCsvFile;
        // 音源
        [SerializeField] private AudioSource music;
        // 譜面のインスタンス
        private Chart chart;
        // AudioManager のインスタンス
        private AudioManager audioManager;
        // TouchInputManager のインスタンスを生成
        private TouchInputManager touchInputManager;
        // MovementManager のインスタンスを生成
        private MovementManager movementManager;
        // JudgementManager のインスタンスを生成
        private JudgementManager judgementManager;
        // NotesManager のインスタンス
        private NotesManager notesManager;
        #endregion

        #region Methods
        // 最初のフレームで呼ばれるメソッド
        void Start()
        {
            // 60fps に設定
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            // 譜面を生成
            chart = new Chart(testCsvFile);
            chart.DisplayInfo();
            // AudioManager のインスタンスを生成
            audioManager = new AudioManager(music);
            // TouchInputManager のインスタンスを生成
            touchInputManager = new TouchInputManager();
            // MovementManager のインスタンスを生成
            movementManager = new MovementManager();
            // JudgementManager のインスタンスを生成
            judgementManager = new JudgementManager(touchInputManager, audioPlay);
            // NotesManager のインスタンスを生成
            notesManager = new NotesManager(chart, movementManager, judgementManager);
        }

        // フレーム毎に呼ばれるメソッド
        void Update()
        {
            // AudioManager を Update し、現在の再生時間を取得
            audioManager.ManagedUpdate();
            float time = audioManager.GetTime();
            // 各インスタンスを Update
            touchInputManager.ManagedUpdate();
            movementManager.ManagedUpdate(time);
            judgementManager.ManagedUpdate(time);
            notesManager.ManagedUpdate(time);
        }
        #endregion
    }

}