using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Lanes;
using static Lyrid.GameScene.GameSceneConsts;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// スライドノートラインの View クラス
    /// </summary>
    public class SlideNoteLineView : MonoBehaviour
    {
        #region Field
        /// <summary> 始点の transform </summary>
        private Transform startTransform;
        /// <summary> 終点の transform </summary>
        private Transform endTransform;
        /// <summary> 制御点の x 座標の割合 </summary>
        private float controlRateX;
        /// <summary> 制御点の z 座標の割合 </summary>
        private float controlRateZ;
        /// <summary> ベジェ曲線の要素数 </summary>
        private int bezierCurveSize;
        /// <summary> 要素数の逆数 </summary>
        private float bezierCurveSizeInverse;
        /// <summary> 描画幅 </summary>
        private float deltaWidth = 0.1f;
        /// <summary> 自身が持つ SkinnedMeshRenderer </summary>
        private SkinnedMeshRenderer meshRenderer;
        /// <summary> Mesh のインスタンス </summary>
        private Mesh mesh;
        /// <summary> メッシュを構成する頂点集合 </summary>
        private Vector3[] vertices;
        /// <summary> メッシュを構成する三角形集合 </summary>
        private int[] triangles;
        /// <summary> ダミーノート更新用変数 </summary>
        private int dummyNoteIndex = 0;
        #endregion

        #region Methods
        /// <summary>
        /// 初期化メソッド
        /// </summary>
        /// <param name="startTransform"> 始点の Transform </param>
        /// <param name="endTransform"> 終点の Transform </param>
        /// <param name="controlRateX"> 制御点の x 座標 </param>
        /// <param name="controlRateZ"> 制御点の z 座標 </param>
        public void Init(Transform startTransform, Transform endTransform, float controlRateX, float controlRateZ)
        {
            this.startTransform = startTransform;
            this.endTransform = endTransform;
            this.controlRateX = controlRateX;
            this.controlRateZ = controlRateZ;
            dummyNoteIndex = 0;
            // 要素数を計算
            bezierCurveSize = (controlRateX == controlRateZ) ? 1 : (int)((endTransform.position.z - startTransform.position.z) / deltaWidth);
            bezierCurveSizeInverse = 1.0f / bezierCurveSize;
            // メッシュを生成
            mesh = new Mesh();
            vertices = new Vector3[bezierCurveSize * 2 + 2];
            UpdateVertices();
            mesh.SetVertices(vertices);
            triangles = new int[bezierCurveSize * 6];
            SetTriangles();
            meshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            meshRenderer.sharedMesh = mesh;
        }

        /// <summary>
        /// ラインを移動させるメソッド
        /// </summary>
        public void Move()
        {
            UpdateVertices();
        }

        /// <summary>
        /// ラインを削除するメソッド
        /// </summary>
        public void Remove()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// z 方向の位置の割合からラインの x 座標を取得するメソッド
        /// </summary>
        /// <param name="rate"> z 方向の位置の割合 </param>
        /// <returns> 対応する x 座標 </returns>
        public float GetPosX(float rate)
        {
            // ラインが直線のとき
            if (controlRateX == controlRateZ)
            {
                return startTransform.position.x + (endTransform.position.x - startTransform.position.x) * rate;
            }
            // 曲線のとき
            else
            {
                // 曲線の z 座標が最も近くなるまで進める
                float posZ = startTransform.position.z + (endTransform.position.z - startTransform.position.z) * rate;
                while (dummyNoteIndex < bezierCurveSize - 1 && vertices[dummyNoteIndex * 2].z < posZ)
                {
                    dummyNoteIndex++;
                }
                float startPosX = (vertices[dummyNoteIndex * 2].x + vertices[dummyNoteIndex * 2 + 1].x) * 0.5f;
                float endPosX = (vertices[dummyNoteIndex * 2 + 2].x + vertices[dummyNoteIndex * 2 + 3].x) * 0.5f;
                float startPosZ = vertices[dummyNoteIndex * 2].z;
                float endPosZ = vertices[dummyNoteIndex * 2 + 2].z;
                float linearRate = (posZ - startPosZ) / (endPosZ - startPosZ);
                return startPosX + (endPosX - startPosX) * linearRate;
            }
        }

        /// <summary>
        /// メッシュを構成するためのインデックスリストを作成するメソッド
        /// </summary>
        private void SetTriangles()
        {
            int index = 0;
            for (int i = 0; i < bezierCurveSize * 2; i += 2)
            {
                triangles[index] = i;
                triangles[index+1] = i+2;
                triangles[index+2] = i+1;
                triangles[index+3] = i+1;
                triangles[index+4] = i+2;
                triangles[index+5] = i+3;
                index += 6;
            }
            mesh.SetTriangles(triangles, 0);
        }


        /// <summary>
        /// ベジェ曲線の頂点を更新するメソッド
        /// </summary>
        private void UpdateVertices()
        {
            // パラメータを設定
            float t = 0;
            float startPosX = startTransform.position.x;
            float startPosZ = startTransform.position.z;
            float endPosX = endTransform.position.x;
            float endPosZ = endTransform.position.z;
            float startWidth = startTransform.localScale.x;
            float endWidth = endTransform.localScale.x;
            float posX, posZ;
            float controlX, controlZ;
            float scX, scZ, ceX, ceZ;
            float width;

            // 頂点を計算
            vertices[0] = new Vector3(startPosX - startWidth * 0.5f, 0, startPosZ);
            vertices[1] = new Vector3(startPosX + startWidth * 0.5f, 0, startPosZ);
            for (int i = 1; i < bezierCurveSize; i++)
            {
                t += bezierCurveSizeInverse;
                controlX = startPosX + (endPosX - startPosX) * controlRateX;
                controlZ = startPosZ + (endPosZ - startPosZ) * controlRateZ;
                scX = startPosX + (controlX - startPosX) * t;
                scZ = startPosZ + (controlZ - startPosZ) * t;
                ceX = controlX + (endPosX - controlX) * t;
                ceZ = controlZ + (endPosZ - controlZ) * t;
                posX = scX + (ceX - scX) * t;
                posZ = scZ + (ceZ - scZ) * t;
                width = startWidth + (endWidth - startWidth) * t;
                vertices[i*2] = new Vector3(posX - width * 0.5f, 0, posZ);
                vertices[i*2+1] = new Vector3(posX + width * 0.5f, 0, posZ);
            }
            vertices[bezierCurveSize*2] = new Vector3(endPosX - endWidth * 0.5f, 0, endPosZ);
            vertices[bezierCurveSize*2+1] = new Vector3(endPosX + endWidth * 0.5f, 0, endPosZ);
            // メッシュに頂点を追加
            mesh.SetVertices(vertices);
        }
        #endregion
    }
}