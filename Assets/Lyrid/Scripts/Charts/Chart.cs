using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lyrid.Charts
{
    // 譜面を管理するクラス
    public class Chart
    {

        #region Field
        // csv から読み込んだデータを各ノート毎にインデックス付けしたリスト
        public List<NoteParam> notesData = new List<NoteParam>();
        // インデックスに対応する時間を格納するリスト
        public List<float> timeData = new List<float>();
        // スライドノートの連続するインデックスを格納するリスト
        public List<List<int>> slideNotesData = new List<List<int>>();
        // スライドノートのインデックスを一時的に記録しておくリスト
        // 生成時にインデックス順に関係なく繋がっているノートをまとめて生成するために必要
        private List<int>[] slideNoteIndexList;
        // レーン位置更新のインデックスリスト
        public List<int>[] lanePosIndexList;
        // レーン幅更新のインデックスリスト
        public List<int>[] laneWidIndexList;

        // 総ノート数
        public int totalNotesNum;
        // レーンに関するパラメータ(生成数、初期配置数、幅、可視化)
        public int maxLaneNum;
        public int initLaneNum;
        public float laneWidth;
        public bool setLaneVisible;
        #endregion


        #region Constructor
        public Chart(TextAsset csvFile)
        {
            InputFile(csvFile);
        }
        #endregion


        #region Methods
        // csv を読み込むメソッド
        private void InputFile(TextAsset csvFile)
        {
            // csv ファイルのデータを格納するリスト
            List<string[]> csvData = new List<string[]>();
            // csv の内容を StringReader に変換
            StringReader reader = new StringReader(csvFile.text);
            // 行ごとにデータを読み込み、csvData に格納する
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                string[] strArray = line.Split(',');
                foreach(string str in strArray) str.Trim();
                csvData.Add(strArray);
            }

            // 現在のインデックス
            int index = 0;
            // オフセット時間: 譜面の初期時間を楽曲のある時点に設定
            float offsetTime = 0.0f;
            // ヘッダを読んでいるかどうか
            bool readingHeader = true;
            // ヘッダーを読み込む
            while (readingHeader)
            {
                string type = csvData[index][0];
                if (type.Length != 0 && type[0] == '#')
                {
                    switch (type)
                    {
                        case "#LANE":
                            maxLaneNum = int.Parse(csvData[index][1]);
                            initLaneNum = int.Parse(csvData[index][2]);
                            laneWidth = int.Parse(csvData[index][3]);
                            setLaneVisible = !(csvData[index][1] == "0");
                            index++;
                            break;
                        case "#OFFSET":
                            offsetTime = int.Parse(csvData[index][1]);
                            index++;
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }
                }
                else
                {
                    readingHeader = false;
                    index++;
                }
            }

            // スライドノート、レーン操作リストのインスタンスを生成
            slideNoteIndexList = new List<int>[maxLaneNum];
            lanePosIndexList = new List<int>[maxLaneNum];
            laneWidIndexList = new List<int>[maxLaneNum];
            for (int i = 0; i < maxLaneNum; i++)
            {
                slideNoteIndexList[i] = new List<int>();
                lanePosIndexList[i] = new List<int>();
                laneWidIndexList[i] = new List<int>();
            }

            int bpm = 120; // BPM
            float top = 4, bottom = 4; // 拍子の分子と分母
            float time = offsetTime; // 現在の秒数

            // 1 小節ごとに判定時間を計算
            while (index < csvData.Count - 1)
            {
                int indexFrom = 0; // 小節の始めの行
                int indexTo = 0; // 小節の終わりの行
                // 先頭要素が t (tempo) なら小節の始めの行の要素を取得し、BPM と拍子を更新する
                if (csvData[index][0] == "t")
                {
                    // BPM と拍子の変更
                    bpm = int.Parse(csvData[index][1]);
                    top = float.Parse(csvData[index][2]);
                    bottom = float.Parse(csvData[index][3]);
                    indexFrom = index + 1;
                }
                // 先頭要素が end なら終了
                else if (csvData[index][0] == "end")
                {
                    break;
                }
                // それ以外なら、先頭インデックスからノーツ情報を読み取る
                else {
                    indexFrom = index;
                }

                // 小節の終わりの行の index を取得
                int j = indexFrom;
                // 次が空白行ならその行を indexTo とする
                while (j < csvData.Count)
                {
                    if (csvData[j+1][0] == "")
                    {
                        indexTo = j;
                        break;
                    }
                    j++;
                }

                // 小節の分割当たりの秒数を計算
                float splitTime = (60.0f / bpm) * 4 * (top / bottom) / (indexTo - indexFrom + 1);
                // 小節内のノーツ情報から判定時間を計算
                float t = time;
                for (int k = indexFrom; k <= indexTo; k++)
                {
                    // 行のデータを分割してリストに格納
                    List<NoteParam> splittedData = splitLine(csvData[k]);
                    foreach (NoteParam data in splittedData)
                    {
                        notesData.Add(data);
                        timeData.Add(t);
                    }
                    // 秒数を分割当たりの時間だけ進める
                    t += splitTime;
                }
                // 小節の秒数をまとめて足す(誤差軽減)
                time += (60.0f / bpm) * 4 * (top / bottom);
                // 次の小節の先頭に移動する
                index = indexTo + 2;
            }

        }

        // 1 行分のノーツ情報を 1 つずつに分割するメソッド
        private List<NoteParam> splitLine(string[] strArray)
        {
            // 分割した後のリスト
            List<NoteParam> newList = new List<NoteParam>();
            int index = 0;
            // strArray 内の要素を分割する
            while (index < strArray.Length)
            {
                // ノーツの種類によって分類
                int type = int.Parse(strArray[index]);
                NoteParam noteParam = new NoteParam();
                switch (type)
                {
                    // 空白の場合
                    case 0:
                        index++;
                        break;
                    // タップノートの場合
                    case 1:
                        noteParam.type = ElementType.Tap;
                        noteParam.laneNum = int.Parse(strArray[index + 1]);
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        newList.Add(noteParam);
                        totalNotesNum++;
                        index += 3;
                        break;
                    // スワイプノートの場合
                    case 2:
                        noteParam.type = ElementType.Swipe;
                        noteParam.laneNum = int.Parse(strArray[index + 1]);
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        newList.Add(noteParam);
                        totalNotesNum++;
                        index += 3;
                        break;
                    // フリックノートの場合
                    case 3:
                        noteParam.type = ElementType.Flick;
                        noteParam.laneNum = int.Parse(strArray[index + 1]);
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        newList.Add(noteParam);
                        totalNotesNum++;
                        index += 3;
                        break;
                    // スライドノーツの場合
                    case 4:
                        noteParam.type = ElementType.Slide;
                        noteParam.laneNum = int.Parse(strArray[index + 1]) - 1;
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        noteParam.var_2 = float.Parse(strArray[index + 3]);
                        noteParam.var_3 = float.Parse(strArray[index + 4]);
                        noteParam.id = int.Parse(strArray[index + 5]);
                        noteParam.connectionType = int.Parse(strArray[index + 6]);
                        // id に対応する一時保管リストにインデックスを記録
                        // スライドノーツの終端まで記録できたら slideNotesData に追加し，初期化
                        slideNoteIndexList[noteParam.id].Add(notesData.Count + newList.Count);
                        if (noteParam.connectionType == 0)
                        {
                            slideNotesData.Add(slideNoteIndexList[noteParam.id]);
                            slideNoteIndexList[noteParam.id] = new List<int>();
                        }
                        newList.Add(noteParam);
                        totalNotesNum++;
                        index += 7;
                        break;
                    // レーン位置変更の場合
                    case 5:
                        noteParam.type = ElementType.LanePos;
                        noteParam.laneNum = int.Parse(strArray[index + 1]) - 1;
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        noteParam.connectionType = int.Parse(strArray[index + 3]);
                        lanePosIndexList[noteParam.laneNum].Add(notesData.Count + newList.Count);
                        newList.Add(noteParam);
                        index += 4;
                        break;
                    // レーン幅変更の場合
                    case 6:
                        noteParam.type = ElementType.LaneWid;
                        noteParam.laneNum = int.Parse(strArray[index + 1]) - 1;
                        noteParam.var_1 = float.Parse(strArray[index + 2]);
                        noteParam.connectionType = int.Parse(strArray[index + 3]);
                        laneWidIndexList[noteParam.laneNum].Add(notesData.Count + newList.Count);
                        newList.Add(noteParam);
                        index += 4;
                        break;
                    // 落下スピード変化の場合
                    case 7:
                        noteParam.type = ElementType.Speed;
                        noteParam.var_1 = float.Parse(strArray[index + 1]);
                        newList.Add(noteParam);
                        index += 2;
                        break;
                    // それ以外ならエラー
                    default:
                        Debug.Assert(false);
                        break;
                }
            }
            return newList;
        }

        // 楽譜情報を log に表示させるメソッド
        public void DisplayInfo()
        {
            string info = "";
            for (int i = 0; i < notesData.Count; i++)
            {
                info += (timeData[i] + "| " + notesData[i].Info() + "\n");
            }
            Debug.Log(info);
        }
        #endregion

    }
}