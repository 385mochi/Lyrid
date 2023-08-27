using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using LyridEditor.UI;

namespace LyridEditor.Lanes
{
    /// <summary>
    /// レーンユニットの終了点のクラス
    /// </summary>
    public class EndLine : MonoBehaviour
    {
        private Camera camera;
        private Vector3 offset;
        private Mouse mouse;
        private Palette palette;
        private int laneIndex;
        private LaneUnit laneUnit;

        [SerializeField] private GameObject lineObj;
        [SerializeField] private GameObject circleObj;

        public float width { get; private set; }

        public void Init(int laneIndex, LaneUnit laneUnit)
        {
            mouse = Mouse.current;
            camera = GameObject.Find("EditorCamera").GetComponent<Camera>();
            palette = GameObject.Find("Palette").GetComponent<Palette>();
            width = 1.0f;
            this.laneIndex = laneIndex;
            this.laneUnit = laneUnit;
        }

        public void UpdateWidth(float newWidth)
        {
            lineObj.transform.localScale = new Vector3(newWidth, 0.05f, 1);
            width = newWidth;
        }

        public void _OnMouseDown()
        {
            if (palette.status == Palette.Status.Pointer && palette.laneIndex == laneIndex)
            {
                Vector2 mousePos = mouse.position.ReadValue();
                this.offset = transform.position - camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));
            }
        }

        public void _OnMouseDrag()
        {
            if (palette.status == Palette.Status.Pointer && palette.laneIndex == laneIndex)
            {
                Vector2 mousePos = mouse.position.ReadValue();
                Vector3 currentScreenPoint = new Vector3(mousePos.x, mousePos.y, 0);
                Vector3 currentPosition = camera.ScreenToWorldPoint(currentScreenPoint) + this.offset;
                transform.position = currentPosition;
            }
        }

        public void _OnMouseUp()
        {
            if (palette.status == Palette.Status.Pointer && palette.laneIndex == laneIndex)
            {
                laneUnit.MoveEndPos();
            }
        }

        /// <summary>
        /// 色を変更する
        /// </summary>
        /// <param name="color"> 変更後の色 </param>
        public void UpdateColor(Color32 color)
        {
            lineObj.GetComponent<SpriteRenderer>().color = color;
            circleObj.GetComponent<SpriteRenderer>().color = color;
        }
    }
}