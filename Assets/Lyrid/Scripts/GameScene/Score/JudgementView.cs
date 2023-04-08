using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.GameScene.Notes;
using static Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Score
{
    /// <summary>
    /// 判定表示の View を管理するクラス
    /// </summary>
    public class JudgementView : MonoBehaviour
    {
        #region Field
        /// <summary> 判定を表示するテキスト </summary>
        [SerializeField] private Text judgementText;
        /// <summary> DOTween のための Sequence インスタンス </summary>
        private Sequence sequence;
        #endregion

        #region Method
        /// <summary>
        /// 判定表示のテキストを更新するメソッド
        /// </summary>
        /// <param name="judgementType"> 判定の種類 </param>
        public void UpdateJudgement(JudgementType judgementType)
        {
            if (sequence != null)
            {
                sequence.Kill();
            }
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            sequence = DOTween.Sequence()
            .Append(gameObject.transform.DOScale(
                new Vector3(1, 1, 1), // スケール値
                0.1f                  // 演出時間
            ))
            .AppendInterval(1.0f)
            .OnComplete(() =>
            {
                sequence = null;
                judgementText.text = "";
            });

            // 色を変更
            switch (judgementType)
            {
                case JudgementType.None:
                    break;
                case JudgementType.Perfect:
                    judgementText.text = "PERFECT";
                    break;
                case JudgementType.Great:
                judgementText.text = "GREAT";
                    break;
                case JudgementType.Good:
                judgementText.text = "GOOD";
                    break;
                case JudgementType.Bad:
                judgementText.text = "BAD";
                    break;
                case JudgementType.Miss:
                judgementText.text = "MISS";
                    break;
            }
        }
        #endregion
    }
}