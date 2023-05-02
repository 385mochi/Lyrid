using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// オブジェクトプールのクラス
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Field
        /// <summary> オブジェクトプール </summary>
        private List<GameObject> pool;
        /// <summary> プールするオブジェクトの Prefab </summary>
        [SerializeField] private GameObject prefabObj;
        /// <summary> 初期オブジェクト数 </summary>
        [SerializeField] int initNum;

        #endregion

        #region Methods
        void Start()
        {
            Reset();
        }

        /// <summary>
        /// プールを初期化する
        /// </summary>
        public void Reset()
        {
            // プールが存在するときは含まれるオブジェクトをすべて破壊
            if (pool != null)
            {
                foreach(GameObject gameObject in pool)
                {
                    Destroy(gameObject);
                }
            }
            // プールを生成して指定された個数のオブジェクトを追加する
            pool = new List<GameObject>();
            for(int i = 0; i < initNum; i++)
            {
                GameObject obj = Instantiate(prefabObj, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        /// <summary>
        /// プールからオブジェクトを取り出す
        /// </summary>
        /// <returns> 取り出したオブジェクト </returns>
        public GameObject GetObject()
        {
            // プール内に非アクティブのオブジェクトがあればそれを返す
            foreach (GameObject obj in pool)
            {
                if (obj.activeSelf == false)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
            // すべて使用済みなら新たにオブジェクトを作成
            GameObject newObj = Instantiate(prefabObj, transform);
            pool.Add(newObj);
            newObj.SetActive(true);
            return newObj;
        }
        #endregion
    }
}