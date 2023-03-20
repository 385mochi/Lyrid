using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Notes;
using Lyrid.GameScene.Audio;

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
            // 譜面を生成
            chart = new Chart(testCsvFile);
            chart.DisplayInfo();
            // AudioManager のインスタンスを生成
            audioManager = new AudioManager(music);
            // MovementManager のインスタンスを生成
            movementManager = new MovementManager();
            // JudgementManager のインスタンスを生成
            judgementManager = new JudgementManager(audioPlay);
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
            movementManager.ManagedUpdate(time);
            judgementManager.ManagedUpdate(time);
            notesManager.ManagedUpdate(time);
        }
        #endregion
    }

}