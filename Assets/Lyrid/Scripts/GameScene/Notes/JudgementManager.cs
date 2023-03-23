using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Input;

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
        // TouchInputManager のインスタンス
        private TouchInputManager touchInputManager;
        #endregion

        #region Constructor
        public JudgementManager(TouchInputManager touchInputManager, bool autoPlay)
        {
            this.touchInputManager = touchInputManager;
            this.autoPlay = autoPlay;
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
        public void ManagedUpdate(float time)
        {
            Judge(time);
        }

        // 判定対象を追加するメソッド
        public void AddTarget(IJudgeable target)
        {
            judgementTargets.Add(target);
        }

        // 判定するメソッド
        private void Judge(float time)
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
            // 通常プレイの場合
            else
            {
                // 各 Touch についてノートを判定
                List<int> touchTypeList = touchInputManager.touchTypeList;
                List<float> posXList = touchInputManager.posXList;
                for (int i = 0; i < touchTypeList.Count; i++)
                {
                    int touchType = touchTypeList[i];
                    float posX = posXList[i];
                    // 判定対象リストを前側からチェック
                    for (int j = 0; j < judgementTargets.Count; j++)
                    {
                        Note target = (Note)judgementTargets[j];
                        JudgementType judgementType = target.Judge(time, touchType, posX);
                        // 判定が None でなければそれを判定とする
                        if (!target.judged && judgementType != JudgementType.None)
                        {
                            target.judged = true;
                            Debug.Log(judgementType.ToString());
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                // Miss 対象またはすでに判定済みのノートがあれば削除
                for (int i = judgementTargets.Count - 1; i >= 0; i--)
                {
                    Note target = (Note)judgementTargets[i];
                    JudgementType judgementType = target.Judge(time, 0, 100);
                    if (judgementType == JudgementType.Miss)
                    {
                        target.judged = true;
                        Debug.Log(judgementType.ToString());
                    }
                    if (target.judged)
                    {
                        judgementTargets.RemoveAt(i);
                        target.Remove();
                    }
                }
            }
        }
        #endregion
    }
}