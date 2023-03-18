using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.Charts;


namespace Lyrid
{
    // GameScene を管理するクラス
    public class GameSceneManager : MonoBehaviour
    {
        [SerializeField]
        private TextAsset testCsvFile;
        private Chart testChart;


        void Start()
        {
            testChart = new Chart(testCsvFile);
            testChart.DisplayInfo();
        }


        void Update()
        {

        }
    }

}