using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LyridEditor.Lanes
{
    /// <summary>
    /// レーンユニットの開始点のクラス
    /// </summary>
    public class MiddleLine : MonoBehaviour
    {
        /// <summary> メッシュレンダラー </summary>
        private SkinnedMeshRenderer meshRenderer;
        /// <summary> メッシュ </summary>
        private Mesh mesh;
        /// <summary> メッシュを構成する頂点集合 </summary>
        private Vector3[] vertices;
        /// <summary> メッシュを構成する三角形集合 </summary>
        private int[] triangles;
        [SerializeField] private GameObject startLineObj;
        [SerializeField] private GameObject endLineObj;

        void Awake()
        {
            // メッシュを生成
            mesh = new Mesh();
            vertices = new Vector3[] {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)
            };
            mesh.SetVertices(vertices);
            triangles = new int[] {0, 2, 1, 1, 2, 3};
            mesh.SetTriangles(triangles, 0);
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
            meshRenderer.sharedMesh = mesh;
        }

        public void UpdateLine()
        {
            Vector3 startPos = startLineObj.transform.localPosition;
            Vector3 endPos = endLineObj.transform.localPosition;
            float startWidth = startLineObj.GetComponent<StartLine>().width;
            float endWidth = endLineObj.GetComponent<EndLine>().width;

            // 頂点を更新
            vertices = new Vector3[] {
                new Vector3(startPos.x - startWidth * 0.5f, startPos.y, 0),
                new Vector3(startPos.x + startWidth * 0.5f, startPos.y, 0),
                new Vector3(endPos.x - endWidth * 0.5f, endPos.y, 0),
                new Vector3(endPos.x + endWidth * 0.5f, endPos.y, 0)
            };

            Debug.Log($"{startPos.x - startWidth * 0.5f}, {startPos.y}");
            Debug.Log($"{startPos.x + startWidth * 0.5f}, {startPos.y}");
            Debug.Log($"{endPos.x - endWidth * 0.5f}, {endPos.y}");
            Debug.Log($"{endPos.x + endWidth * 0.5f}, {endPos.y}");
            mesh.SetVertices(vertices);
        }

        /// <summary>
        /// 色を変更する
        /// </summary>
        /// <param name="color"> 変更後の色 </param>
        public void UpdateColor(Color32 color)
        {
            GetComponent<Renderer>().material.color = color;
        }
    }
}