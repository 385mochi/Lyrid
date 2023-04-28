using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using static Lyrid.Common.CommonConsts;
using Lyrid.SelectScene;


namespace Lyrid.ResultScene
{
    /// <summary>
    /// ResultScene を管理するクラス
    /// </summary>
    public class ResultSceneManager : MonoBehaviour
    {
        #region Field
        [SerializeField] private Image musicImage;
        [SerializeField] private Image backGroundImage;
        [SerializeField] private Text musicNameText;
        [SerializeField] private Text composerNameText;
        [SerializeField] private Image diffFrame;
        [SerializeField] private Text levelText;
        [SerializeField] private Text scoreText;
        [SerializeField] private Text achievementText;
        [SerializeField] private Text perfectNumText;
        [SerializeField] private Text greatNumText;
        [SerializeField] private Text goodNumText;
        [SerializeField] private Text badNumText;
        [SerializeField] private Text missNumText;
        [SerializeField] private Text maxChainAndAccuracyText;
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private GameObject selectSceneObj;
        #endregion

        #region Methods
        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Init(
            Sprite musicImage,
            Sprite backGroundImage,
            string musicName,
            string composerName,
            Difficulty diff,
            int level,
            int score,
            bool isHighScore,
            bool isFullChain,
            bool isAllPerfect,
            int perfectNum,
            int greatNum,
            int goodNum,
            int badNum,
            int missNum,
            int maxChain,
            int totalChain,
            float accuracy
        )
        {
            // リザルトシーンを表示
            gameObject.SetActive(true);

            // 各パラメータを設定
            this.musicImage.sprite = musicImage;
            this.backGroundImage.sprite = backGroundImage;
            musicNameText.text = musicName;
            composerNameText.text = composerName;
            levelText.text = level.ToString();
            scoreText.text = score.ToString("0000000");
            achievementText.text = (isHighScore ? "+ High Score\n" : "") + (isFullChain ? "+ Full Chain" : "") + (isAllPerfect ? "+ All Perfect" : "");
            perfectNumText.text = perfectNum.ToString();
            greatNumText.text = greatNum.ToString();
            goodNumText.text = goodNum.ToString();
            badNumText.text = badNum.ToString();
            missNumText.text = missNum.ToString();
            maxChainAndAccuracyText.text = $"| Max Chain : {maxChain} / {totalChain}\n| Accuracy : {accuracy.ToString("F3")} %";

            switch (diff)
            {
                case Difficulty.Normal:
                    diffFrame.color = NORMAL_DIFF_COLOR;
                    levelText.color = NORMAL_DIFF_COLOR;
                    break;
                case Difficulty.Hard:
                    diffFrame.color = HARD_DIFF_COLOR;
                    levelText.color = HARD_DIFF_COLOR;
                    break;
                case Difficulty.Expert:
                    diffFrame.color = EXPERT_DIFF_COLOR;
                    levelText.color = EXPERT_DIFF_COLOR;
                    break;
            }

            // ロード画面を非表示にする
            loadingScreen.SetInvisible();
        }

        /// <summary>
        /// 選択シーンをロードするメソッド
        /// </summary>
        public void LoadSelectScene()
        {
            // ロード画面を表示させる
            loadingScreen.SetVisible(backGroundImage.sprite).OnComplete(() => {
                selectSceneObj.GetComponent<SelectSceneManager>().Init();
                gameObject.SetActive(false);
            });
        }
        #endregion
    }

}