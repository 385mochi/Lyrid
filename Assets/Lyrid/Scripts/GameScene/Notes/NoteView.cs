using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;

namespace Lyrid.GameScene.Notes
{
    // ノートの View クラス
    public class NoteView : MonoBehaviour
    {
        #region Field
        // レーンの高さ
        [SerializeField] private float laneHeight;
        // 初期座標
        private Vector3 initPos;
        // transform キャッシュ
        private Transform tfCache;
        #endregion

        #region Methods
        // 状態をリセットするメソッド
        public void Reset(NoteParam noteParam)
        {
            tfCache = gameObject.transform;
            tfCache.localPosition = new Vector3(0, 0, laneHeight);
        }
        // ノートを移動させるメソッド
        public void Move(float rate)
        {
            tfCache.localPosition = new Vector3(0, 0, laneHeight * rate);
        }
        // View を非表示にするメソッド
        public void Remove()
        {
            gameObject.SetActive(false);
        }
        // posX がノートの範囲内かどうか判定するメソッド
        public bool Touched(float posX, float margin)
        {
            return (
                tfCache.position.x - (tfCache.lossyScale.x / 2.0f) - margin <= posX &&
                tfCache.position.x + (tfCache.lossyScale.x / 2.0f) + margin >= posX
            );
        }
        #endregion
    }
}