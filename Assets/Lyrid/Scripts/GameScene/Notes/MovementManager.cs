using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノーツ移動を管理するクラス
    /// </summary>
    public class MovementManager
    {
        #region Field
        /// <summary> 移動対象のリスト </summary>
        private List<IMovable> targets;
        #endregion

        #region Constructor
        public MovementManager()
        {
            targets = new List<IMovable>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        public void ManagedUpdate(float time)
        {
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (!targets[i].Move(time))
                {
                    targets.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            targets = new List<IMovable>();
        }

        /// <summary>
        /// 移動対象を追加するメソッド
        /// </summary>
        /// <param name="target"> 移動対象 </param>
        public void AddTarget(IMovable target)
        {
            targets.Add(target);
        }
        #endregion
    }
}