using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LyridEditor
{
    /// <summary>
    /// 全ての小節を管理するクラス
    /// </summary>
    public class Bars : MonoBehaviour
    {
        #region Field
        [SerializeField] private GameObject barPrefab;
        [SerializeField] private GameObject addBarButton;
        private List<GameObject> barList;
        private float widthRate = 0.142f;
        #endregion

        #region Methods
        private void Start()
        {
            Init();
        }

        public void Init()
        {
            barList = new List<GameObject>();
        }

        public void AddBar()
        {
            GameObject barObj = Instantiate(barPrefab, this.transform);
            if (barList.Count != 0)
            {
                barObj.transform.localPosition = new Vector3(0, barList[barList.Count-1].transform.localPosition.y + barList[barList.Count-1].transform.localScale.y, 0);
            }
            barList.Add(barObj);
            addBarButton.transform.position = new Vector3(
                addBarButton.transform.position.x,
                barList[barList.Count-1].transform.position.y + barList[barList.Count-1].transform.localScale.y * 0.5f,
                addBarButton.transform.position.z
            );
            Bar bar = barObj.GetComponent<Bar>();
            bar.Init(barList.Count, 120, 4, 4, 4);
        }
        #endregion
    }
}