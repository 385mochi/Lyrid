using System.Collections;
using System.Collections.Generic;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    // ノーツ移動を管理するクラス
    public class MovementManager
    {
        #region Field
        // 移動対象のリスト
        private List<IMovable> movingTargets = new List<IMovable>();
        #endregion

        #region Constructor
        public MovementManager()
        {
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
        public void ManagedUpdate(float time)
        {
            for (int i = movingTargets.Count - 1; i >= 0; i--)
            {
                Note target = (Note)movingTargets[i];
                // 判定されていなければ移動、されていれば対象を削除する
                if (!target.judged)
                {
                    target.Move(time);
                }
                else
                {
                    movingTargets.RemoveAt(i);
                }
            }
        }

        // 移動対象を追加する
        public void AddTarget(IMovable target)
        {
            movingTargets.Add(target);
        }
        #endregion
    }
}