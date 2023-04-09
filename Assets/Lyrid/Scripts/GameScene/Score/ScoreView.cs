using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Lyrid.GameScene.Score
{
    /// <summary>
    /// スコアの View を管理するクラス
    /// </summary>
    public class ScoreView : MonoBehaviour
    {
        #region Field
        /// <summary> スコアを表示するテキスト </summary>
        [SerializeField] private Text scoreText;
        /// <summary> DOTween のための Tweener インスタンス </summary>
        private Tweener tweener;
        #endregion

        #region Method
        /// <summary>
        /// スコアのテキストを更新するメソッド
        /// </summary>
        /// <param name="score"> スコア </param>
        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString("0000000");
            if (tweener != null)
            {
                tweener.Kill();
            }
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            tweener = gameObject.transform.DOScale(
                new Vector3(1, 1, 1), // スケール値
                0.2f                  // 演出時間
            ).OnComplete(() => {tweener = null;});
        }

        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Reset()
        {
            scoreText.text = "0000000";
        }
        #endregion
    }
}