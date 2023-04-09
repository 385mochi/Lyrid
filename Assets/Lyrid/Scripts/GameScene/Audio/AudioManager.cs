using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using CriWare.Assets;
using Lyrid.GameScene.Charts;
using Lyrid.Common;

namespace Lyrid.GameScene.Audio
{
    /// <summary>
    /// GameScene の Audio を管理するクラス
    /// </summary>
    public class AudioManager
    {
        #region Field
        /// <summary> CriAtomExPlayer のインスタンス (楽曲用) </summary>
        private CriAtomExPlayer musicPlayer;
        /// <summary> CriAtomExPlayer のインスタンス (タップ音用) </summary>
        private CriAtomExPlayer tapSoundPlayer;
        // <summary> CriAtomExPlayback のインスタンス </summary>
        private CriAtomExPlayback playback;
        /// <summary> 楽曲の acb ファイル </summary>
        private CriAtomExAcb music;
        /// <summary> タップ音の acb ファイル </summary>
        private CriAtomExAcb tapSound;
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
        /// <param name="music"> 楽曲の acb ファイル </param>
        /// <param name="initTime"> 時間の初期値 </param>
        public AudioManager(CriAtomExAcb music, CriAtomExAcb tapSound, float initTime)
        {
            this.music = music;
            this.time = initTime;
            this.initTime = initTime;
            musicPlayer = new CriAtomExPlayer(true);
            musicPlayer.SetCue(music, 0);
            tapSoundPlayer = new CriAtomExPlayer();
            tapSoundPlayer.SetCue(tapSound, 0);
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
                    playback = musicPlayer.Start();
                    nowPlaying = true;
                }
            }
            else
            {
                // samplingFrame 毎に実際の楽曲時間をサンプリングする
                if (samplingFlameCounter++ >= samplingFlame)
                {
                    samplingFlameCounter = 0;
                    time = playback.GetTimeSyncedWithAudio() * 0.001f;
                }
                // サンプリング時以外は deltaTime 分だけ進める
                else {
                    time += deltaTime;
                }
            }
        }

        public void PlayTapSound()
        {
            tapSoundPlayer.Start();
        }

        public void Reset()
        {
            musicPlayer.Stop();
            nowPlaying = false;
            time = initTime;
        }
        #endregion
    }
}