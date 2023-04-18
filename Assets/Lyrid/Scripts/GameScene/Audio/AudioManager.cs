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
        /// <summary> 実際の楽曲再生位置を取得する間隔 </summary>
        private int samplingFlame = 5;
        /// <summary> samplingFlame のためのカウンタ </summary>
        private int samplingFlameCounter;
        /// <summary> 時間の初期値 </summary>
        private float initTime;
        /// <summary> 今フレームにタップ音が鳴らされたかどうか <summary>
        private bool taoSoundPlayedInThisFrame;
        /// <summary> deltaTime キャッシュ用変数 </summary>
        private float deltaTimeCache;
        /// <summary> 再生状態を表す列挙型 </summary>
        public enum Status { Stop, Playing, Ended }
        #endregion

        #region Property
        /// <summary> 現在の再生時間 </summary>
        public float time { get; private set; }
        /// <summary> 再生状態 </summary>
        public Status status { get; private set; }
        #endregion

        #region Constructor
        /// <param name="music"> 楽曲の acb ファイル </param>
        /// <param name="initTime"> 時間の初期値 </param>
        public AudioManager(CriAtomExAcb music, CriAtomExAcb tapSound, float initTime)
        {
            this.music = music;
            this.time = initTime;
            this.initTime = initTime;
            samplingFlameCounter = 0;
            musicPlayer = new CriAtomExPlayer(true);
            musicPlayer.SetCue(music, 0);
            tapSoundPlayer = new CriAtomExPlayer();
            tapSoundPlayer.SetCue(tapSound, 0);
            Reset();
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        public void ManagedUpdate()
        {
            // deltaTime を取得
            deltaTimeCache = Time.deltaTime;

            // 今回のフレームでタップ音を鳴らしたかどうか
            taoSoundPlayedInThisFrame = false;

            switch (status)
            {
                case Status.Stop:
                    // time が正になるまで進める
                    if (time < 0)
                    {
                        time += deltaTimeCache;
                    }
                    // 再生を開始する
                    else
                    {
                        playback = musicPlayer.Start();
                        status = Status.Playing;
                    }
                    break;
                case Status.Playing:
                    // samplingFrame 毎に実際の楽曲時間をサンプリングする
                    if (samplingFlameCounter++ > samplingFlame)
                    {
                        samplingFlameCounter = 0;
                        time = playback.GetTimeSyncedWithAudio() * 0.001f;

                        // 取得した再生時間が 0 であれば状態を End とする
                        if (time <= 0 || time > 10.0f)
                        {
                            musicPlayer.Stop();
                            status = Status.Ended;
                        }
                    }
                    // サンプリング時以外は deltaTime 分だけ進める
                    else
                    {
                        time += deltaTimeCache;
                    }
                    break;
                case Status.Ended:
                    break;
            }
        }

        public void PlayTapSound()
        {
            if (!taoSoundPlayedInThisFrame)
            {
                tapSoundPlayer.Start();
                taoSoundPlayedInThisFrame = true;
            }
        }

        public void Reset()
        {
            status = Status.Stop;
            musicPlayer.Stop();
            time = initTime;
            samplingFlameCounter = 0;
        }
        #endregion
    }
}