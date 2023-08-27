using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using LyridEditor.Lanes;
using LyridEditor.UI;

namespace LyridEditor.Input
{
    /// <summary>
    /// キー入力を管理するクラス
    /// </summary>
    public class KeyInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject bars;
        [SerializeField] private Palette palette;
        [SerializeField] private LaneManager laneManager;

        void Update()
        {
            CheckUpDownArrowKey();
            CheckDigitKey();
        }

        private void CheckUpDownArrowKey()
        {
             if (Keyboard.current.upArrowKey.isPressed)
            {
                MoveBars(-30.0f);
            }
            if (Keyboard.current.downArrowKey.isPressed)
            {
                MoveBars(30.0f);
            }
        }

        private void CheckDigitKey()
        {
            int digit = 0;
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                digit = 1;
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                digit = 2;
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                digit = 3;
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                digit = 4;
            }
            else if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                digit = 5;
            }
            else if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                digit = 6;
            }
            else if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                digit = 7;
            }
            else if (Keyboard.current.digit8Key.wasPressedThisFrame)
            {
                digit = 8;
            }
            else if (Keyboard.current.digit9Key.wasPressedThisFrame)
            {
                digit = 9;
            }
            else if (Keyboard.current.digit0Key.wasPressedThisFrame)
            {
                digit = 10;
            }

            // 1 ~ 9, 0 のいずれかが押されたときはレーンの切り替えを行う
            if (digit != 0)
            {
                laneManager.SwitchLane(digit-1);
                palette.SwitchLane(digit-1);
            }
        }

        private void MoveBars(float dir)
        {
            if (dir > 0 && bars.transform.position.y >= 0)
            {
                Vector3 pos = bars.transform.position;
                pos.y = 0;
                bars.transform.position = pos;
            }
            else
            {
                Vector3 pos = bars.transform.position;
                pos.y += dir * Time.deltaTime;
                bars.transform.position = pos;
            }
        }
    }
}