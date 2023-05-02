using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Lanes;
using static Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノートの View クラス
    /// </summary>
    public class NoteView : MonoBehaviour
    {
        #region Field
        /// <summary> ノートの色 </summary>
        [SerializeField] private SpriteRenderer noteColor;
        /// <summary> NoteFrame オブジェクト </summary>
        [SerializeField] private GameObject noteFrameObj;
        /// <summary> NoteColor オブジェクト </summary>
        [SerializeField] private GameObject noteColorObj;
        /// <summary> 判定の猶予座標 </summary>
        private float margin = 0.5f;
        /// <summary> 初期 z 座標 </summary>
        private float initPosZ;
        /// <summary> レーンに追従するかどうか </summary>
        private bool followLane = true;
        /// <summary> 追従するレーンの transform </summary>
        private Transform laneTf;
        #endregion

        #region Property
        /// <summary> transform キャッシュ </summary>
        public Transform tfCache { get; private set; }
        /// <summary> ノートの幅 </summary>
        public float width { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// 状態を初期化するメソッド
        /// </summary>
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTime"> 判定時間 </param>
        /// <param name="noteParam"> ノートのパラメータ </param>
        /// <param name="isSlideNote"> スライドノートかどうか </param>
        public void Init(float generatedTime, float judgementTime, NoteParam noteParam, bool isSlideNote)
        {
            // レーンを追従するか判定し、追従する場合はレーンの transform を取得
            if (noteParam.laneNum < 0)
            {
                followLane = false;
            }
            else
            {
                laneTf = GameObject.FindWithTag("Lanes").GetComponent<LanesManager>().laneTransforms[noteParam.laneNum];
            }
            // 初期位置を設定
            tfCache = gameObject.transform;
            initPosZ = LANE_HEIGHT;
            if (noteParam.var_4 > 0)
            {
                initPosZ = LANE_HEIGHT * (judgementTime - generatedTime) / (noteParam.var_4 - generatedTime);
            }
            Move(1.0f);
            // 色を設定
            if (isSlideNote && noteParam.type != ElementType.None)
            {
                SetColor(ElementType.Slide);
            }
            else
            {
                SetColor(noteParam.type);
            }
            // サイズを設定
            width = noteParam.var_1;
        }

        /// <summary>
        /// ノートを移動させるメソッド
        /// </summary>
        /// <param name="rate"> 移動させる割合 </param>
        public void Move(float rate)
        {
            float newPosZ = initPosZ * rate;
            if (followLane)
            {
                tfCache.localPosition = new Vector3(laneTf.position.x, 0, newPosZ);
                tfCache.localScale = new Vector3(laneTf.localScale.x * width, tfCache.localScale.y, tfCache.localScale.z);
            }
            else
            {
                Vector3 pos = tfCache.localPosition;
                pos.z = newPosZ;
                tfCache.localPosition = pos;
            }
        }

        /// <summary>
        /// ノートの x 座標のみを更新させるメソッド
        /// </summary>
        /// <param name="posX"> 更新後の x 座標 </param>
        /// <param name="scaleX"> 更新後の幅 </param>
        public void MoveX(float posX, float scaleX)
        {
            Vector3 pos = tfCache.localPosition;
            pos.x = posX;
            tfCache.localPosition = pos;
            Vector3 scale = tfCache.localScale;
            scale.x = scaleX * width;
            tfCache.localScale = scale;
        }

        /// <summary>
        /// posX がノートの範囲内かどうか判定するメソッド
        /// </summary>
        /// <param name="posX"> 判定する x 座標 </param>
        /// <returns> 範囲内かどうか </returns>
        public bool Touched(float posX)
        {
            return (
                tfCache.position.x - (tfCache.lossyScale.x / 2.0f) - margin <= posX &&
                tfCache.position.x + (tfCache.lossyScale.x / 2.0f) + margin >= posX
            );
        }

        /// <summary>
        /// ノートの色を設定するメソッド
        /// </summary>
        /// <param name="type"> 要素の種類 </param>
        public void SetColor(ElementType type)
        {
            noteFrameObj.SetActive(true);
            noteColorObj.SetActive(true);
            switch (type)
            {
                case ElementType.Tap:
                    noteColor.color = TAP_NOTE_COLOR;
                    break;
                case ElementType.Swipe:
                    noteColor.color = SWIPE_NOTE_COLOR;
                    break;
                case ElementType.Flick:
                    noteColor.color = FLICK_NOTE_COLOR;
                    break;
                case ElementType.Slide:
                    noteColor.color = SLIDE_NOTE_COLOR;
                    break;
                case ElementType.None:
                    noteFrameObj.SetActive(false);
                    noteColorObj.SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// View を非表示にするメソッド
        /// </summary>
        public void Remove()
        {
            noteFrameObj.SetActive(false);
            noteColorObj.SetActive(false);
            DOVirtual.DelayedCall (0.1f, () => gameObject.SetActive(false), false);
        }
        #endregion
    }
}