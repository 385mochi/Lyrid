using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Lyrid.Common.CommonConsts;

namespace Lyrid.SelectScene.UI
{
    public class MusicInfoDisplay : MonoBehaviour
    {
        #region Field
        [SerializeField] private Image musicImage;
        [SerializeField] private Text musicNameText;
        [SerializeField] private Text composerNameText;
        [SerializeField] private Text detailInfoText;
        [SerializeField] private Image normalDiffFrame;
        [SerializeField] private Image hardDiffFrame;
        [SerializeField] private Image expertDiffFrame;
        [SerializeField] private Text normalDiffText;
        [SerializeField] private Text hardDiffText;
        [SerializeField] private Text expertDiffText;
        #endregion

        #region Property
        private void Start()
        {
            normalDiffFrame.color = NORMAL_DIFF_COLOR;
            hardDiffFrame.color = HARD_DIFF_COLOR;
            expertDiffFrame.color = EXPERT_DIFF_COLOR;
        }

        public void UpdateDisplay(
            Sprite musicImage,
            string musicName, string composerName,
            string bpm, string genre,
            int normalDiff, int hardDiff, int expertDiff)
        {
            // 各パラメータを設定
            this.musicImage.sprite = musicImage;
            musicNameText.text = musicName;
            composerNameText.text = composerName;
            detailInfoText.text = ((bpm != "") ? $"BPM: {bpm}  " : "") +
                                  ((genre != "") ? $"GENRE: {genre}" : "");
            normalDiffText.text = normalDiff.ToString();
            hardDiffText.text = hardDiff.ToString();
            expertDiffText.text = expertDiff.ToString();

            // 楽曲情報のテキストをフェードイン
            Color musicNameTextColor = musicNameText.color;
            musicNameTextColor.a = 0;
            musicNameText.color = musicNameTextColor;
            Color composerNameTextColor = composerNameText.color;
            composerNameTextColor.a = 0;
            composerNameText.color = composerNameTextColor;
            Color detailInfoTextColor = detailInfoText.color;
            detailInfoTextColor.a = 0;
            detailInfoText.color = detailInfoTextColor;
            DOTween.ToAlpha(
                () => musicNameText.color,
                color => musicNameText.color = color,
                1.0f, 0.5f
            );
            DOTween.ToAlpha(
                () => composerNameText.color,
                color => composerNameText.color = color,
                1.0f, 0.5f
            );
            DOTween.ToAlpha(
                () => detailInfoText.color,
                color => detailInfoText.color = color,
                1.0f, 0.5f
            );
        }
        #endregion

        public void SetDifficulty(Difficulty diff)
        {
            switch (diff)
            {
                case Difficulty.Normal:
                    DOTween.ToAlpha(
                        () => normalDiffFrame.color,
                        color => normalDiffFrame.color = color,
                        1.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => hardDiffFrame.color,
                        color => hardDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => expertDiffFrame.color,
                        color => expertDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    break;
                case Difficulty.Hard:
                    DOTween.ToAlpha(
                        () => normalDiffFrame.color,
                        color => normalDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => hardDiffFrame.color,
                        color => hardDiffFrame.color = color,
                        1.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => expertDiffFrame.color,
                        color => expertDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    break;
                case Difficulty.Expert:
                    DOTween.ToAlpha(
                        () => normalDiffFrame.color,
                        color => normalDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => hardDiffFrame.color,
                        color => hardDiffFrame.color = color,
                        0.0f, 0.2f
                    );
                    DOTween.ToAlpha(
                        () => expertDiffFrame.color,
                        color => expertDiffFrame.color = color,
                        1.0f, 0.2f
                    );
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}