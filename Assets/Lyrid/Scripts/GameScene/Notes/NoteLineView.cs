using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 同時押しノートの View クラス
    /// </summary>
    public class NoteLineView : MonoBehaviour
    {
        #region Field
        [SerializeField] private LineRenderer lineRenderer;
        #endregion

        #region Methods
        public void UpdateLine(Vector3 left, float leftWidth, Vector3 right, float rightWidth)
        {
            left.x += leftWidth * 0.5f;
            right.x -= rightWidth * 0.5f;
            lineRenderer.SetPosition(0, left);
            lineRenderer.SetPosition(1, right);
        }

        public void Remove()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}