using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using LyridEditor.UI;
using LyridEditor.Lanes;

namespace LyridEditor.Input
{
    /// <summary>
    /// マウス入力を管理するクラス
    /// </summary>
    public class MouseInputManager : MonoBehaviour
    {
        private Camera camera;
        private Mouse mouse;
        private LaneManager laneManager;
        private Palette palette;

        void Start()
        {
            mouse = Mouse.current;
            camera = GameObject.Find("EditorCamera").GetComponent<Camera>();
            laneManager = GameObject.Find("Lanes").GetComponent<LaneManager>();
            palette = GameObject.Find("Palette").GetComponent<Palette>();
        }

        void Update()
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                OnLeftButtonDown();
            }
        }

        public void OnLeftButtonDown()
        {
            if (palette.status == Palette.Status.Lane)
            {
                Vector2 mousePos = mouse.position.ReadValue();
                Vector3 screenPos = new Vector3(mousePos.x, mousePos.y, 0);
                Vector3 worldPos = camera.ScreenToWorldPoint(screenPos);
                laneManager.AddLaneUnit(worldPos);
            }
        }
    }
}