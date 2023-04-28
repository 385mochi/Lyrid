using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using CriWare;
using CriWare.Assets;
using Lyrid.Common;

namespace Lyrid.SelectScene.Audio
{
    /// <summary>
    /// 楽曲のクリップ音声 を再生するプレイヤーのクラス
    /// </summary>
    public class MusicClipPlayer
    {
        /// <summary> CriAtomExPlayer のインスタンス </summary>
        private CriAtomExPlayer player;
        /// <summary> CriAtomExAcb のインスタンス </summary>
        private CriAtomExAcb musicAcb;
        /// <summary> AssetLoader のインスタンス </summary>
        private AssetLoader assetLoader;

        public MusicClipPlayer()
        {
            player = new CriAtomExPlayer();
            assetLoader = GameObject.FindWithTag("AssetLoader").GetComponent<AssetLoader>();
        }

        public async Task Play(string key)
        {
            player.Stop();
            musicAcb = await assetLoader.LoadAudioAsync($"{key}_audio");
            player.SetCue(musicAcb, 1);
            player.Start();
        }

        public void Stop()
        {
            player.Stop();
        }
    }
}