using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Notes;

namespace Lyrid.GameScene.Score
{
    /// <summary>
    /// スコアを管理するクラス
    /// </summary>
    public class ScoreManager
    {
        #region Field
        /// <summary> 判定対象の数 </summary>
        private int scoreTargetNum;
        /// <summary> 現在のチェイン数 </summary>
        private int chain;
        /// <summary> 全てパーフェクト判定かどうか </summary>
        private bool allPerfect;
        /// <summary> ノート判定による得点率の計算用係数 </summary>
        private float noteJudgementRateCoeff;
        /// <summary> チェインによる得点率の計算用係数 </summary>
        private float chainRateCoeff;
        /// <summary> スコアの View </summary>
        private ScoreView scoreView;
        /// <summary> チェインの View </summary>
        private ChainView chainView;
        /// <summary> 判定表示のテキストの View </summary>
        private JudgementView judgementView;
        #endregion

        #region Property
        /// <summary> 現在のスコア </summary>
        public int score { get; private set; }
        #endregion

        #region Constructor
        /// <param name="scoreTargetNum"> 判定対象の数 </param>
        public ScoreManager(int scoreTargetNum)
        {
            this.scoreTargetNum = scoreTargetNum;
            score = 0;
            chain = 0;
            allPerfect = true;
            // 100000 / 判定対象の数
            noteJudgementRateCoeff = 900000.0f / scoreTargetNum;
            // 900000 / ((判定対象の数 - 1) * 判定対象の数 / 2) を計算
            chainRateCoeff = 100000.0f / ((scoreTargetNum - 1) * scoreTargetNum / 2.0f);
            // 各 View を取得
            scoreView = GameObject.Find("ScoreText").GetComponent<ScoreView>();
            chainView = GameObject.Find("ChainText").GetComponent<ChainView>();
            judgementView = GameObject.Find("JudgementText").GetComponent<JudgementView>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            score = 0;
            chain = 0;
            allPerfect = true;
            scoreView.Reset();
            chainView.Reset();
            judgementView.Reset();
        }
        /// <summary>
        /// スコアを加算するメソッド
        /// </summary>
        /// <param name="judgementType"> 判定の種類 </param>
        public void AddScore(JudgementType judgementType)
        {
            switch (judgementType)
            {
                case JudgementType.None:
                    return;
                case JudgementType.Perfect:
                    chain++;
                    score += (int)(noteJudgementRateCoeff * 1.0f);
                    score += (int)(chainRateCoeff * (chain - 1));
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfect);
                    judgementView.UpdateJudgement(JudgementType.Perfect);
                    Debug.Log($"判定: <color=orange>{judgementType}</color>");
                    break;
                case JudgementType.Great:
                    chain++;
                    allPerfect = false;
                    score += (int)(noteJudgementRateCoeff * 0.8f);
                    score += (int)(chainRateCoeff * (chain - 1));
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfect);
                    judgementView.UpdateJudgement(JudgementType.Great);
                    Debug.Log($"判定: <color=yellow>{judgementType}</color>");
                    break;
                case JudgementType.Good:
                    chain = 0;
                    allPerfect = false;
                    score += (int)(noteJudgementRateCoeff * 0.5f);
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfect);
                    judgementView.UpdateJudgement(JudgementType.Good);
                    Debug.Log($"判定: <color=green>{judgementType}</color>");
                    break;
                case JudgementType.Bad:
                    chain = 0;
                    allPerfect = false;
                    score += (int)(noteJudgementRateCoeff * 0.2f);
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfect);
                    judgementView.UpdateJudgement(JudgementType.Bad);
                    Debug.Log($"判定: <color=cyan>{judgementType}</color>");
                    break;
                case JudgementType.Miss:
                    chain = 0;
                    allPerfect = false;
                    chainView.UpdateChain(chain, allPerfect);
                    judgementView.UpdateJudgement(JudgementType.Miss);
                    Debug.Log($"判定: <color=blue>{judgementType}</color>");
                    break;
            }

            if (chain == scoreTargetNum)
            {
                // ALL PERFECT !!!
                if (allPerfect)
                {
                    score = 1000000;
                    scoreView.UpdateScore(score);
                    Debug.Log($"判定: <color=magenta>[ALL PERFECT]</color>");
                }
                // FULL CHAIN !!
                else
                {
                    Debug.Log($"判定: <color=red>[FULL CHAIN]</color>");
                }
            }
        }
        #endregion
    }
}