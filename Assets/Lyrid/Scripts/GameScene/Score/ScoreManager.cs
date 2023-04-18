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
        /// <summary> 現在のチェイン数 </summary>
        private int chain;
        /// <summary> 今まで全てパーフェクト判定かどうか </summary>
        private bool allPerfectEver;
        /// <summary> ノート判定による得点率の計算用係数 </summary>
        private float noteJudgementRateCoeff;
        /// <summary> チェインによる得点率の計算用係数 </summary>
        private float chainRateCoeff;
        /// <summary> 精度の計算用係数 </summary>
        private float accuracyCoeff;
        /// <summary> スコアの View </summary>
        private ScoreView scoreView;
        /// <summary> チェインの View </summary>
        private ChainView chainView;
        /// <summary> 判定表示のテキストの View </summary>
        private JudgementView judgementView;
        #endregion

        #region Property
        /// <summary> 判定対象の数 </summary>
        public int scoreTargetNum { get; private set; }
        /// <summary> 現在のスコア </summary>
        public int score { get; private set; }
        // <summary> 最大 Chain 数 </summary>
        public int maxChain { get; private set; }
        /// <summary> Perfect 判定数 </summary>
        public int perfectNum { get; private set; }
        /// <summary> Great 判定数 </summary>
        public int greatNum { get; private set; }
        /// <summary> Good 判定数 </summary>
        public int goodNum { get; private set; }
        /// <summary> Bad 判定数 </summary>
        public int badNum { get; private set; }
        /// <summary> Miss 判定数 </summary>
        public int missNum { get; private set; }
        public bool isFullChain { get; private set; }
        public bool isAllPerfect { get; private set; }
        public float accuracy { get; private set; }
        #endregion

        #region Constructor
        /// <param name="scoreTargetNum"> 判定対象の数 </param>
        public ScoreManager(int scoreTargetNum)
        {
            this.scoreTargetNum = scoreTargetNum;
            score = 0;
            chain = 0;
            maxChain = 0;
            perfectNum = 0;
            greatNum = 0;
            goodNum = 0;
            badNum = 0;
            missNum = 0;
            allPerfectEver = true;
            isFullChain = false;
            isAllPerfect = false;
            accuracy = 0;
            // 100000 / 判定対象の数
            noteJudgementRateCoeff = 900000.0f / scoreTargetNum;
            // 900000 / ((判定対象の数 - 1) * 判定対象の数 / 2) を計算
            chainRateCoeff = 100000.0f / ((scoreTargetNum - 1) * scoreTargetNum / 2.0f);
            // 判定対象の数の逆数
            accuracyCoeff = 100.0f / scoreTargetNum;
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
            maxChain = 0;
            perfectNum = 0;
            greatNum = 0;
            goodNum = 0;
            badNum = 0;
            missNum = 0;
            allPerfectEver = true;
            isFullChain = false;
            isAllPerfect = false;
            accuracy = 0;
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
                    perfectNum++;
                    chain++;
                    accuracy += 1.0f * accuracyCoeff;
                    score += (int)(noteJudgementRateCoeff * 1.0f);
                    score += (int)(chainRateCoeff * (chain - 1));
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfectEver);
                    judgementView.UpdateJudgement(JudgementType.Perfect);
                    // Debug.Log($"判定: <color=orange>{judgementType}</color>");
                    break;
                case JudgementType.Great:
                    allPerfectEver = false;
                    greatNum++;
                    chain++;
                    accuracy += 0.8f * accuracyCoeff;
                    score += (int)(noteJudgementRateCoeff * 0.8f);
                    score += (int)(chainRateCoeff * (chain - 1));
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfectEver);
                    judgementView.UpdateJudgement(JudgementType.Great);
                    // Debug.Log($"判定: <color=yellow>{judgementType}</color>");
                    break;
                case JudgementType.Good:
                    allPerfectEver = false;
                    goodNum++;
                    chain = 0;
                    accuracy += 0.5f * accuracyCoeff;
                    score += (int)(noteJudgementRateCoeff * 0.5f);
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfectEver);
                    judgementView.UpdateJudgement(JudgementType.Good);
                    // Debug.Log($"判定: <color=green>{judgementType}</color>");
                    break;
                case JudgementType.Bad:
                    allPerfectEver = false;
                    badNum++;
                    chain = 0;
                    accuracy += 0.2f * accuracyCoeff;
                    score += (int)(noteJudgementRateCoeff * 0.2f);
                    scoreView.UpdateScore(score);
                    chainView.UpdateChain(chain, allPerfectEver);
                    judgementView.UpdateJudgement(JudgementType.Bad);
                    // Debug.Log($"判定: <color=cyan>{judgementType}</color>");
                    break;
                case JudgementType.Miss:
                    allPerfectEver = false;
                    missNum++;
                    chain = 0;
                    chainView.UpdateChain(chain, allPerfectEver);
                    judgementView.UpdateJudgement(JudgementType.Miss);
                    // Debug.Log($"判定: <color=blue>{judgementType}</color>");
                    break;
            }

            // 最大コンボ数を更新
            if (maxChain < chain)
            {
                maxChain = chain;
            }

            // AP・FC 判定
            if (chain == scoreTargetNum)
            {
                // ALL PERFECT !!!
                if (allPerfectEver)
                {
                    score = 1000000;
                    accuracy = 100.00f;
                    scoreView.UpdateScore(score);
                    isAllPerfect = true;
                }
                // FULL CHAIN !!
                else
                {
                    isFullChain = true;
                }
            }
        }
        #endregion
    }
}