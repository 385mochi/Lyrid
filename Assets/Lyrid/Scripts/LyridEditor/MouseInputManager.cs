using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LyridEditor
{
    /// <summary>
    /// マウス入力を管理するクラス
    /// </summary>
    public class MouseInputManager : MonoBehaviour
    {
        [SerializeField] private GameObject bars;

        void Update()
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                Vector3 pos = bars.transform.position;
                pos.y -= 16.0f * Time.deltaTime;
                bars.transform.position = pos;
            }
            if (Keyboard.current.downArrowKey.isPressed)
            {
                Vector3 pos = bars.transform.position;
                pos.y += 16.0f * Time.deltaTime;
                bars.transform.position = pos;
            }
        }
    }
}