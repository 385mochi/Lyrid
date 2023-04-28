using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CriWare;
using CriWare.Assets;
using Lyrid.Common;

namespace Lyrid.Common.Audio
{
    /// <summary>
    /// SE を再生するプレイヤーのクラス
    /// </summary>
    public class SEPlayer: MonoBehaviour
    {
        /// <summary> CriAtomExPlayer のインスタンス </summary>
        private CriAtomExPlayer player;
        /// <summary> SE 音の acb ファイル </summary>
        private CriAtomExAcb soundEffect;
        /// <summary> AssetLoader のインスタンス </summary>
        private AssetLoader assetLoader;

        private async void Start()
        {
            assetLoader = GameObject.FindWithTag("AssetLoader").GetComponent<AssetLoader>();
            player = new CriAtomExPlayer();
            soundEffect = await assetLoader.LoadAudioAsync("sound_effect");
        }

        public void Play(int id)
        {
            player.SetCue(soundEffect, id);
            player.Start();
        }
    }
}