using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Input;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノーツ判定を管理するクラス
    /// </summary>
    public class JudgementManager
    {
        #region Field
        /// <summary> 判定対象のリスト </summary>
        private List<IJudgeable> targets = new List<IJudgeable>();
        /// <summary> 判定済みの対象のインデックスのリスト </summary>
        private List<int> judgedTargetIndexList;
        /// <summary> オートプレイかどうか </summary>
        private bool autoPlay;
        /// <summary> TouchInputManager のインスタンス </summary>
        private TouchInputManager touchInputManager;
        #endregion

        #region Constructor
        /// <param name="touchInputManager"> TouchInputManager のインスタンス </param>
        /// <param name="autoPlay"> オートプレイかどうか </param>
        public JudgementManager(TouchInputManager touchInputManager, bool autoPlay)
        {
            this.touchInputManager = touchInputManager;
            this.autoPlay = autoPlay;
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        public void ManagedUpdate(float time)
        {
            Judge(time);
        }

        /// <summary>
        /// 判定対象を追加するメソッド
        /// </summary>
        /// <param name="target"> 判定対象 </param>
        public void AddTarget(IJudgeable target)
        {
            targets.Add(target);
        }

        /// <summary>
        /// 判定を追加するメソッド
        /// </summary>
        /// <param name="type"> 判定の種類 </param>
        public void AddJudgement(JudgementType type)
        {
        }

        /// <summary>
        /// 判定するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        private void Judge(float time)
        {
            // 判定済みリストを初期化
            judgedTargetIndexList = new List<int>(10);
            // オートプレイの場合
            if (autoPlay)
            {
                for (int i = 0; i < targets.Count; i--)
                {
                    if (targets[i] is SlideNote)
                    {
                        continue;
                    }
                    Note note = (Note)targets[i];
                    // 判定時間になったら Perfect 判定とする
                    if (note.judgementTime - 0.008f <= time)
                    {
                        note.judged = true;
                    }
                }
            }
            // 通常プレイの場合
            else
            {
                IReadOnlyList<int> touchTypeList = touchInputManager.touchTypeList;
                IReadOnlyList<float> posXList = touchInputManager.posXList;
                // 各 Touch についてノートを判定
                for (int i = 0; i < touchTypeList.Count; i++)
                {
                    int touchType = touchTypeList[i];
                    float posX = posXList[i];
                    // 判定対象リストを前側からチェック
                    for (int j = 0; j < targets.Count; j++)
                    {
                        IJudgeable target = targets[j];
                        JudgementType judgementType = target.Judge(time, touchType, posX);
                        // 判定が None であれば無視
                        if (judgementType == JudgementType.None)
                        {
                            continue;
                        }
                        // 判定が Judged であれば判定済みリストに追加する
                        else if (judgementType == JudgementType.Judged)
                        {
                            judgedTargetIndexList.Add(i);
                            continue;
                        }
                        // そのほかの場合はそれを判定とする
                        else
                        {
                            Debug.Log(judgementType.ToString());
                            break;
                        }
                    }
                }
                // タッチされていないときは、存在するスライドノートのコンボを切る
                if (touchTypeList.Count == 0)
                {
                    // 判定対象リストを前側からチェック
                    for (int j = 0; j < targets.Count; j++)
                    {
                        IJudgeable target = targets[j];
                        if (target is SlideNote)
                        {
                            JudgementType judgementType = target.Judge(time, 0, 100);
                            // 判定が None であれば無視
                            if (judgementType == JudgementType.None)
                            {
                                continue;
                            }
                            // そのほかの場合はそれを判定とする
                            else
                            {
                                Debug.Log(judgementType.ToString());
                            }
                        }
                    }
                }
            }
            // すでに判定済みのノートを削除
            for (int i = judgedTargetIndexList.Count - 1; i >= 0; i--)
            {
                targets.RemoveAt(judgedTargetIndexList[i]);
            }
        }
        #endregion
    }
}