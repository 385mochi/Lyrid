using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Notes
{
    // オブジェクトプールのクラス
    public class ObjectPool : MonoBehaviour
    {
        #region Field
        // オブジェクトプール
        private List<GameObject> pool;
        // プールするオブジェクトの Prefab
        [SerializeField] private GameObject prefabObj;
        #endregion

        #region Constructor
        void Start()
        {
            Reset(30);
        }
        #endregion

        #region Methods
        // プールを初期化する
        public void Reset(int size) {
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
            for(int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefabObj, transform);
                obj.SetActive(false);
                pool.Add(obj);
            }
        }

        // プールからオブジェクトを取り出す
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