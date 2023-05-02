using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using Lyrid.Common.UI;
using static Lyrid.Common.CommonConsts;
using Lyrid.GameScene;
using Lyrid.SelectScene.UI;


namespace Lyrid.SelectScene
{
    /// <summary>
    /// SelectScene を管理するクラス
    /// </summary>
    public class SelectSceneManager : MonoBehaviour
    {
        [SerializeField] private MusicList musicList;
        [SerializeField] MusicInfoDisplay musicInfoDisplay;
        [SerializeField] LoadingScreen loadingScreen;
        [SerializeField] GameObject gameSceneObj;
        private Difficulty selectedDiff;
        private AssetLoader assetLoader;

        private async void Start()
        {
            await Init();
        }

        public async Task Init()
        {
            // 選択シーンを表示
            gameObject.SetActive(true);

            await musicList.Init();
            assetLoader = GameObject.FindWithTag("AssetLoader").GetComponent<AssetLoader>();

            // ロード画面を非表示にする
            loadingScreen.SetInvisible();
        }

        public void SetDifficulty(int diff)
        {
            switch (diff)
            {
                case 1:
                    if (selectedDiff == Difficulty.Normal) return;
                    musicList.SetDifficulty(Difficulty.Normal);
                    musicInfoDisplay.SetDifficulty(Difficulty.Normal);
                    selectedDiff = Difficulty.Normal;
                    break;
                case 2:
                    if (selectedDiff == Difficulty.Hard) return;
                    musicList.SetDifficulty(Difficulty.Hard);
                    musicInfoDisplay.SetDifficulty(Difficulty.Hard);
                    selectedDiff = Difficulty.Hard;
                    break;
                case 3:
                    if (selectedDiff == Difficulty.Expert) return;
                    musicList.SetDifficulty(Difficulty.Expert);
                    musicInfoDisplay.SetDifficulty(Difficulty.Expert);
                    selectedDiff = Difficulty.Expert;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

        /// <summary>
        /// GameScene をロードするメソッド
        /// </summary>
        public async void LoadGameScene()
        {

            // AssetLoader の handle を解放
            assetLoader.ReleaseAll();

            // 選択された MusicBox を取得
            MusicBox musicBox = musicList.selectedMusicBox;

            // 楽曲の背景用画像をロードする
            Sprite bgImage = await assetLoader.LoadSpriteAsync($"{musicBox.key}_image_blurred");

            // ロード画面を表示させる
            loadingScreen.SetVisible(bgImage).OnComplete(() =>
            {
                // 楽曲のクリップ再生を停止
                musicList.StopMusicClip();

                // ゲームシーンを表示
                gameSceneObj.SetActive(true);
                gameSceneObj.GetComponent<GameSceneManager>().Init(
                    musicBox.key,
                    musicBox.musicName,
                    musicBox.composerName,
                    musicList.difficulty,
                    musicBox.level
                );

                // 選択シーンを非表示にする
                gameObject.SetActive(false);
            });
        }
    }
}