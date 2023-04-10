using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Lyrid.GameScene.Score
{
    /// <summary>
    /// チェインの View を管理するクラス
    /// </summary>
    public class ChainView : MonoBehaviour
    {
        #region Field
        /// <summary> チェイン数を表示するテキスト </summary>
        [SerializeField] private Text chainText;
        /// <summary> DOTween のための Tweener インスタンス </summary>
        private Tweener tweener;
        /// <summary> 前回の chain </summary>
        private int preChain = 0;
        /// <summary> チェインの状態を表す列挙型 </summary>
        private enum ChainStatus { AllPerfect, FullChain, Normal }
        /// <summary> チェインの状態 </summary>
        private ChainStatus chainStatus = ChainStatus.AllPerfect;
        #endregion

        #region Method
        /// <summary>
        /// チェイン数のテキストを更新するメソッド
        /// </summary>
        /// <param name="chain"> チェイン数 </param>
        /// <param name="allPerfect"> 全て Perfect かどうか </param>
        public void UpdateChain(int chain, bool allPerfect)
        {
            // 値を更新する
            chainText.text = chain.ToString();
            if (tweener != null)
            {
                tweener.Kill();
            }
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            tweener = gameObject.transform.DOScale(
                new Vector3(1, 1, 1), // スケール値
                0.2f                  // 演出時間
            ).OnComplete(() => {tweener = null;});

            // 色を更新する
            switch (chainStatus)
            {
                case ChainStatus.Normal:
                    break;
                case ChainStatus.FullChain:
                    if (chain < preChain)
                    {
                        chainStatus = ChainStatus.Normal;
                        chainText.color = new Color32(255, 255, 255, 200);
                    }
                    break;
                case ChainStatus.AllPerfect:
                    if (chain < preChain)
                    {
                        chainStatus = ChainStatus.Normal;
                        chainText.color = new Color32(255, 255, 255, 200);
                    }
                    else if (!allPerfect)
                    {
                        chainStatus = ChainStatus.FullChain;
                        chainText.color = new Color32(255, 240, 200, 200);
                    }
                    break;
            }

            // チェイン数を保持
            preChain = chain;
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            preChain = 0;
            chainStatus = ChainStatus.AllPerfect;
            chainText.text = "0";
            chainText.color = new Color32(200, 255, 255, 200);
        }
        #endregion
    }
}