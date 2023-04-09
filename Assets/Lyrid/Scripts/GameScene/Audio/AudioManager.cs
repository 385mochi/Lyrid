using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Audio
{
    /// <summary>
    /// GameScene の Audio を管理するクラス
    /// </summary>
    public class AudioManager
    {
        #region Field
        /// <summary> 楽曲の AudioSource </summary>
        private AudioSource music;
        /// <summary> 再生中かどうか </summary>
        private bool nowPlaying = false;
        /// <summary> 実際の楽曲再生位置を取得する間隔 </summary>
        private int samplingFlame = 5;
        /// <summary> samplingFlame のためのカウンタ </summary>
        private int samplingFlameCounter = 0;
        /// <summary> 時間の初期値 </summary>
        private float initTime;
        #endregion

        #region Property
        /// <summary> 現在の再生時間 </summary>
        public float time { get; private set; }
        #endregion

        #region Constructor
        /// <param name="music"> 楽曲の AudioSource </param>
        /// <param name="initTime"> 時間の初期値 </param>
        public AudioManager(AudioSource music, float initTime)
        {
            this.music = music;
            this.time = initTime;
            this.initTime = initTime;
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        public void ManagedUpdate()
        {
            // deltaTime を取得
            float deltaTime = Time.deltaTime;
            // time が 0 になるまで進める
            if (!nowPlaying)
            {
                time += deltaTime;
                if (time >= 0)
                {
                    music.Play();
                    nowPlaying = true;
                }
            }
            else
            {
                // samplingFrame 毎に実際の楽曲時間をサンプリングする
                if (samplingFlameCounter++ >= samplingFlame)
                {
                    samplingFlameCounter = 0;
                    time = music.time;
                }
                // サンプリング時以外は deltaTime 分だけ進める
                else {
                    time += deltaTime;
                }
            }
        }

        public void Reset()
        {
            music.Stop();
            nowPlaying = false;
            time = initTime;
        }
        #endregion
    }
}