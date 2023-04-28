using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using static Lyrid.Common.CommonConsts;

namespace Lyrid.SelectScene.UI
{
    public class MusicBox : MonoBehaviour
    {
        #region Field
        [SerializeField] private Image musicImage;
        [SerializeField] private Text musicNameText;
        [SerializeField] private Text composerNameText;
        [SerializeField] private Text levelText;
        [SerializeField] private Image diffFrame;
        #endregion

        #region Property
        public string key { get; private set; }
        public Sprite musicSprite { get { return musicImage.sprite; } }
        public string musicName { get { return musicNameText.text; } }
        public string composerName { get { return composerNameText.text; } }
        public string bpm { get; private set;  }
        public string genre { get; private set;  }
        public int level { get { return int.Parse(levelText.text); } }
        public int normalDiff { get; private set; }
        public int hardDiff { get; private set; }
        public int expertDiff { get; private set; }
        #endregion

        #region Methods
        public async Task Init(
            string key,
            string musicName, string composerName,
            string bpm, string genre,
            int normalDiff, int hardDiff, int expertDiff
        )
        {
            this.key = key;
            AssetLoader assetLoader = GameObject.FindWithTag("AssetLoader").GetComponent<AssetLoader>();
            musicImage.sprite = await assetLoader.LoadSpriteAsync($"{key}_image");
            musicNameText.text = musicName;
            composerNameText.text = composerName;
            this.bpm = bpm;
            this.genre = genre;
            this.normalDiff = normalDiff;
            this.hardDiff = hardDiff;
            this.expertDiff = expertDiff;
        }

        public void SetDifficulty(Difficulty diff)
        {
            // 各難易度の色とレベルを設定
            switch (diff)
            {
                case Difficulty.Normal:
                    levelText.text = normalDiff.ToString();
                    levelText.color = NORMAL_DIFF_COLOR;
                    diffFrame.color = NORMAL_DIFF_COLOR;
                    break;
                case Difficulty.Hard:
                    levelText.text = hardDiff.ToString();
                    levelText.color = HARD_DIFF_COLOR;
                    diffFrame.color = HARD_DIFF_COLOR;
                    break;
                case Difficulty.Expert:
                    levelText.text = expertDiff.ToString();
                    levelText.color = EXPERT_DIFF_COLOR;
                    diffFrame.color = EXPERT_DIFF_COLOR;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }

            // 透明な状態からフェードインさせる
            Color32 levelTextColor = levelText.color;
            levelTextColor.a = 0;
            levelText.color = levelTextColor;
            Color32 diffFrameColor = diffFrame.color;
            diffFrameColor.a = 0;
            diffFrame.color = diffFrameColor;

            DOTween.ToAlpha(
                () => levelText.color,
                color => levelText.color = color,
                1.0f, 0.2f
            );
            DOTween.ToAlpha(
                () => diffFrame.color,
                color => diffFrame.color = color,
                1.0f, 0.2f
            );
        }
        #endregion
    }
}