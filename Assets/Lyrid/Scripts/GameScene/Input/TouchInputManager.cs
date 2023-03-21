using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Lyrid.GameScene.Input
{
    // タッチ入力を管理するクラス
    public class TouchInputManager
    {
        #region Field
        // タッチの種類と x 座標のリスト
        public List<int> touchTypeList;
        public List<float> posXList;
        // スクリーン情報
        private bool screenWide;
        private float screenCenterX;
        private float screenNotWideInverse;
        private float screenWideInverse;
        #endregion

        #region Constructor
        public TouchInputManager()
        {
            EnhancedTouchSupport.Enable();
            screenWide = Screen.width * 9 > Screen.height * 16;
            screenCenterX = Screen.width / 2;
            screenNotWideInverse = 2.0f / Screen.width * 4.22f;
            screenWideInverse = 2.0f / (Screen.height * 16.0f / 9.0f) * 4.22f;
        }
        #endregion

        #region Methods
        // GameSceneManager からフレームごとに呼び出されるメソッド
        public void ManagedUpdate()
        {
            // リストを初期化
            touchTypeList = new List<int>();
            posXList = new List<float>();
            // 各 touch について処理
            var activeTouches = Touch.activeTouches;
            for (int i = 0; i < activeTouches.Count; i++)
            {
                var touch = activeTouches[i];
                // リストに追加
                touchTypeList.Add(GetTouchTypeNum(touch));
                posXList.Add(TouchPosXToWorldPosX(touch.screenPosition.x));
            }
        }

        /*
        // touch の情報から，レーン番号を返す
        private int TouchLane(Touch touch) {
            if(GetTouchTypeNum(touch) == 1) {
                int index = 1;
                foreach(GameObject laneObj in lanes.laneObjects) {
                    float laneWidth = laneObj.transform.localScale.x;
                    float worldPosX = TouchPosXToWorldPosX(touch.screenPosition.x);
                    if(laneObj.transform.position.x - worldPosX <= laneWidth / 2
                    && laneObj.transform.position.x - worldPosX >= -laneWidth / 2) {
                        return index;
                    }
                    index++;
                }
            }
            return 0;
        }
        */

        // タッチしたスクリーン x 座標から判定レーン上のワールド座標に変換
        private float TouchPosXToWorldPosX(float touchPosX)
        {
            if (screenWide)
            {
                return (touchPosX - screenCenterX) * screenWideInverse;
            }
            else
            {
                return (touchPosX - screenCenterX) * screenNotWideInverse;
            }
        }

        // Touch の情報から番号に変換
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