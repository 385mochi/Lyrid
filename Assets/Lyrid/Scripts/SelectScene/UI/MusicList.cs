using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Lyrid.Common;
using static Lyrid.Common.CommonConsts;
using static Lyrid.Common.Easings;
using Lyrid.SelectScene.Audio;

namespace Lyrid.SelectScene.UI
{
    public class MusicList : MonoBehaviour
    {
        [SerializeField] GameObject contentObj;
        [SerializeField] GameObject musicBoxPrefab;
        [SerializeField] MusicInfoDisplay musicInfoDisplay;
        private MusicClipPlayer musicClipPlayer;
        private List<MusicBox> musicBoxList;
        private List<float> snapPosList;
        private float spacing;
        private int selectedIndex;

        #region Property
        public MusicBox selectedMusicBox { get { return musicBoxList[selectedIndex]; } }
        public Difficulty difficulty { get; private set; }
        #endregion

        public async Task Init()
        {
            // Contents 以下のオブジェクトをすべて破棄する
            foreach (Transform child in contentObj.transform)
            {
                Destroy(child.gameObject);
            }

            musicClipPlayer = new MusicClipPlayer();
            await InputMusicInfo();
            if (difficulty != Difficulty.None) SetDifficulty(difficulty);
            else SetDifficulty(Difficulty.Normal);
            SetSnapPos();
            selectedIndex = -1;
            Select();
        }

        public async void Select()
        {
            // 最も近い MusicBox のインデックスを計算
            float pos = contentObj.GetComponent<RectTransform>().anchoredPosition.y;
            int index = -1;
            if (pos < snapPosList[0])
            {
                index = 0;
            }
            else if (snapPosList[snapPosList.Count - 1] < pos)
            {
                index = snapPosList.Count - 1;
            }
            else
            {
                for (int i = 0; i < snapPosList.Count; i++)
                {
                    if (snapPosList[i] - spacing <= pos &&
                        snapPosList[i] + spacing >= pos)
                    {
                        index = i;
                        break;
                    }
                }
            }

            // スナップを行う
            contentObj.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0.0f, snapPosList[index]), 0.2f)
                .SetEase(GetEaseType(3));
            musicBoxList[index].transform.DOScale(1.1f, 0.2f)
                .SetEase(GetEaseType(3));

            // 前回選択した曲と同じであればこれ以降の処理を行わない
            if (selectedIndex == index) return;
            selectedIndex = index;

            // 選択された楽曲情報の表示を更新する
            MusicBox musicBox = musicBoxList[index];
            musicInfoDisplay.UpdateDisplay(
                musicBox.musicSprite,
                musicBox.musicName,
                musicBox.composerName,
                musicBox.bpm,
                musicBox.genre,
                musicBox.normalDiff,
                musicBox.hardDiff,
                musicBox.expertDiff
            );

            // 選択した楽曲を再生する
            await musicClipPlayer.Play(musicBox.key);
        }

        public void Deselect()
        {
            musicBoxList[selectedIndex].transform.DOScale(1.0f, 0.2f)
                .SetEase(GetEaseType(3));
        }

        /// <summary>
        /// MusicInfo から取得した楽曲情報をリストに反映させるメソッド
        /// </summary>
        private async Task InputMusicInfo()
        {
            musicBoxList = new List<MusicBox>();

            AssetLoader assetLoader = GameObject.FindWithTag("AssetLoader").GetComponent<AssetLoader>();
            TextAsset musicInfo = await assetLoader.LoadTextAssetAsync("MusicInfo");
            StringReader reader = new StringReader(musicInfo.text);

            bool labeled = false;
            int keyIndex = -1;
            int musicNameIndex = -1;
            int composerNameIndex = -1;
            int bpmIndex = -1;
            int genreIndex = -1;
            int normalDiffIndex = -1;
            int hardDiffIndex = -1;
            int expertDiffIndex = -1;

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string[] strArray = line.Split(',');
                foreach(string str in strArray) str.Trim();

                if (!labeled)
                {
                    // Header を読み取る
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        switch (strArray[i])
                        {
                            case "Key":
                                keyIndex = i;
                                break;
                            case "MusicName":
                                musicNameIndex = i;
                                break;
                            case "ComposerName":
                                composerNameIndex = i;
                                break;
                            case "Bpm":
                                bpmIndex = i;
                                break;
                            case "Genre":
                                genreIndex = i;
                                break;
                            case "NormalDiff":
                                normalDiffIndex = i;
                                break;
                            case "HardDiff":
                                hardDiffIndex = i;
                                break;
                            case "ExpertDiff":
                                expertDiffIndex = i;
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    labeled = true;
                }
                else
                {
                    // MusicBox を作成する
                    GameObject musicBoxObj = Instantiate(musicBoxPrefab, contentObj.transform);
                    MusicBox musicBox = musicBoxObj.GetComponent<MusicBox>();
                    await musicBox.Init(
                        strArray[keyIndex],
                        strArray[musicNameIndex],
                        strArray[composerNameIndex],
                        (bpmIndex == -1) ? "" : strArray[bpmIndex],
                        (genreIndex == -1) ? "" : strArray[genreIndex],
                        int.Parse(strArray[normalDiffIndex]),
                        int.Parse(strArray[hardDiffIndex]),
                        int.Parse(strArray[expertDiffIndex])
                    );
                    musicBoxList.Add(musicBox);
                }
            }
            reader.Close();
        }

        public void SetDifficulty(Difficulty diff)
        {
            difficulty = diff;
            musicInfoDisplay.SetDifficulty(diff);
            foreach (MusicBox musicBox in musicBoxList)
            {
                musicBox.SetDifficulty(diff);
            }
        }

        public void StopMusicClip()
        {
            musicClipPlayer.Stop();
        }

        private void SetSnapPos()
        {
            snapPosList = new List<float>();
            var vlp = contentObj.GetComponent<VerticalLayoutGroup>();
            spacing = vlp.spacing;
            for (int i = 0; i < musicBoxList.Count; i++)
            {
                snapPosList.Add(spacing * i);
            }
        }
    }
}