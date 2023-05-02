using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using Lyrid.GameScene.Lanes;

namespace Lyrid.GameScene.Input
{
    /// <summary>
    /// タッチ入力を管理するクラス
    /// </summary>
    public class TouchInputManager
    {
        #region Field
        /// <summary> スクリーンが 16:9 よりも横に広いかどうか </summary>
        private bool screenWide;
        /// <summary> スクリーンの中心の x 座標 </summary>
        private float screenCenterX;
        /// <summary> スクリーン座標調整用 </summary>
        private float screenNotWideInverse;
        /// <summary> スクリーン座標調整用 </summary>
        private float screenWideInverse;
        /// <summary> レーンの Transform のリスト </summary>
        private List<Transform> laneTransforms;
        /// <summary> Lane のリスト </summary>
        private List<Lane> lanes;
        #endregion

        #region Property
        /// <summary> タッチの種類のリスト </summary>
        public List<int> touchTypeList { get; private set; }
        /// <summary> タッチの x 座標のリスト </summary>
        public List<float> posXList { get; private set; }
        #endregion

        #region Constructor
        public TouchInputManager()
        {
            // EnhancedTouch を有効にする
            EnhancedTouchSupport.Enable();
            // スクリーン情報を取得する
            screenWide = Screen.width * 9 > Screen.height * 16;
            screenCenterX = Screen.width / 2;
            screenNotWideInverse = 2.0f / Screen.width * 4.22f;
            screenWideInverse = 2.0f / (Screen.height * 16.0f / 9.0f) * 4.22f;
            // レーンの Transform のリストを取得する
            laneTransforms = GameObject.FindWithTag("Lanes").GetComponent<LanesManager>().laneTransforms;
            // Lane のリストを取得する
            lanes = GameObject.FindWithTag("Lanes").GetComponent<LanesManager>().lanes;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 初期化メソッド
        /// </summary>
        public void Reset()
        {
            // レーンの Transform のリストを取得する
            laneTransforms = GameObject.FindWithTag("Lanes").GetComponent<LanesManager>().laneTransforms;
            // Lane のリストを取得する
            lanes = GameObject.FindWithTag("Lanes").GetComponent<LanesManager>().lanes;
        }

        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        public void ManagedUpdate()
        {
            // リストを初期化
            touchTypeList = new List<int>(10);
            posXList = new List<float>(10);

            // 各 touch について処理
            var activeTouches = Touch.activeTouches;
            for (int i = 0; i < activeTouches.Count; i++)
            {
                var touch = activeTouches[i];
                // リストに追加
                touchTypeList.Add(GetTouchTypeNum(touch));
                posXList.Add(TouchPosXToWorldPosX(touch.screenPosition.x));
                // レーンを光らせる
                int index = GetTouchedLane(touch);
                if (index != -1)
                {
                    if (GetTouchTypeNum(touch) != -1)
                    {
                        lanes[index].LightUp();
                    }
                }
            }
        }

        /// <summary>
        /// タッチ情報からレーン番号を返すメソッド
        /// </summary>
        /// <param name="touch"> タッチ情報 </param>
        /// <returns> レーン番号 (0-based) </returns>
        private int GetTouchedLane(Touch touch)
        {
            if (GetTouchTypeNum(touch) == 1)
            {
                int index = 0;
                foreach (Transform laneTransform in laneTransforms)
                {
                    float laneWidth = laneTransform.localScale.x;
                    float worldPosX = TouchPosXToWorldPosX(touch.screenPosition.x);
                    if (laneTransform.position.x - worldPosX <= laneWidth / 2
                    && laneTransform.position.x - worldPosX >= -laneWidth / 2) {
                        return index;
                    }
                    index++;
                }
            }
            return -1;
        }

        /// <summary>
        /// タッチしたスクリーン x 座標から判定レーン上のワールド座標に変換するメソッド
        /// </summary>
        /// <param name="touchPosX"> スクリーン x 座標 </param>
        /// <returns> 対応するワールド座標 </returns>
        private float TouchPosXToWorldPosX(float touchPosX)
        {
            return (touchPosX - screenCenterX) * ((screenWide) ? screenWideInverse : screenNotWideInverse);
        }

        /// <summary>
        /// Touch の情報から番号に変換
        /// </summary>
        /// <param name="touch"> タッチ情報 </param>
        /// <returns> TouchPhase に対応した番号 </returns>
        private int GetTouchTypeNum(Touch touch) {
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    return 1;
                case TouchPhase.Moved:
                    return 2;
                case TouchPhase.Stationary:
                    return 3;
                case TouchPhase.Ended:
                    return 4;
                case TouchPhase.Canceled:
                    return 5;
                default:
                    return 0;
            }
        }
        #endregion
    }
}