using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Audio
{
    // GameScene の Audio を管理するクラス
    public class AudioManager
    {
        #region Field
        // 楽曲の AudioSource
        private AudioSource music;
        // 現在の再生時間
        private float time = -5.0f;
        // 再生中かどうか
        private bool nowPlaying = false;
        // 実際の楽曲再生位置を取得する間隔
        private int samplingFlame = 5;
        // 'samplingFlame' のためのカウンタ
        private int samplingFlameCounter = 0;
        #endregion

        #region Constructor
        public AudioManager(AudioSource music)
        {
            this.music = music;
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
        public void ManagedUpdate()
        {
            // deltaTime を取得
            float deltaTime = Time.deltaTime;
            // time が 0 になるまですすめる
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
                // samplingFrame ごとに実際の楽曲時間をサンプリングする
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

        // 現在の再生時間を返すメソッド
        public float GetTime()
        {
            return time;
        }
        #endregion
    }
}