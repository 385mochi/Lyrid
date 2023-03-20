using System.Collections;
using System.Collections.Generic;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    // ノーツ判定を管理するクラス
    public class JudgementManager
    {
        #region Field
        // 判定対象のリスト
        private List<IJudgeable> judgementTargets = new List<IJudgeable>();
        // オートプレイかどうか
        private bool autoPlay;
        #endregion

        #region Constructor
        public JudgementManager(bool autoPlay)
        {
            this.autoPlay = autoPlay;
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
        public void ManagedUpdate(float time)
        {
            // オートプレイの場合
            if (autoPlay)
            {
                for (int i = judgementTargets.Count - 1; i >= 0; i--)
                {
                    Note target = (Note)judgementTargets[i];
                    // 判定時間になったら Perfect 判定とし、対象を削除
                    if (target.judgementTime - 0.008f <= time)
                    {
                        target.judged = true;
                        judgementTargets.RemoveAt(i);
                        target.Remove();
                    }
                }
            }
        }

        // 判定対象を追加する
        public void AddTarget(IJudgeable target)
        {
            judgementTargets.Add(target);
        }
        #endregion
    }
}