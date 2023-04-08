using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Audio;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// スライドノートを管理するクラス
    /// </summary>
    public class SlideNote : IMovable, IJudgeable
    {
        #region Field
        /// <summary> 生成された時間 </summary>
        private float generatedTime;
        /// <summary> 判定時間のリスト </summary>
        private List<float> judgementTimeList;
        /// <summary> パラメータのリスト </summary>
        private List<NoteParam> noteParamList;
        /// <summary> 構成要素のノートのリスト </summary>
        private List<Note> noteList = new List<Note>();
        /// <summary> 構成要素のノートの transform のリスト </summary>
        private List<Transform> noteTransformList = new List<Transform>();
        /// <summary> 構成要素のラインのリスト </summary>
        private List<SlideNoteLineView> slideNoteLineViewList = new List<SlideNoteLineView>();
        /// <summary> ダミーノートのインスタンス </summary>
        private DummyNote dummyNote = null;
        /// <summary> ノート間の判定時間の差分の逆数のリスト </summary>
        private List<float> judgementTimeDiffInverseList = new List<float>();
        /// <summary> ダミーノートの座標がどのラインの座標を参照するか </summary>
        private int dummyNoteLineIndex = 0;
        /// <summary> ダミーノートが継続的に押されているかどうか </summary>
        private bool dummyNotePressed = true;
        /// <summary> オブジェクトプールのインスタンス </summary>
        private ObjectPool pool;
        /// <summary> JudgementManager のインスタンス </summary>
        JudgementManager judgementManager;
        #endregion

        #region Constructor
        /// <param name="generatedTime"> 生成時間 </param>
        /// <param name="judgementTimeList"> 判定時間のリスト </param>
        /// <param name="noteParamList"> ノートのパラメータのリスト </param>
        /// <param name="movementManager"> MovementManager のインスタンス </param>
        /// <param name="judgementManager"> JudgementManager のインスタンス </param>
        public SlideNote(
            float generatedTime, List<float> judgementTimeList, List<NoteParam> noteParamList,
            MovementManager movementManager, JudgementManager judgementManager
        )
        {
            this.generatedTime = generatedTime;
            this.judgementTimeList = judgementTimeList;
            this.noteParamList = noteParamList;
            this.judgementManager = judgementManager;
            // ノート間の判定時間の差分の逆数を計算
            for (int i = 0; i < judgementTimeList.Count - 1; i++)
            {
                judgementTimeDiffInverseList.Add(1.0f / (judgementTimeList[i + 1] - judgementTimeList[i]));
            }
            pool = GameObject.FindWithTag("SlideNoteLineObjectPool").GetComponent<ObjectPool>();
            GenerateNotes();
            for (int i = 0; i < noteList.Count; i++)
            {
                movementManager.AddTarget(noteList[i]);
                judgementManager.AddTarget(noteList[i]);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// ノートを移動するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        /// <returns> 以降 Move を実行するかどうか </returns>
        public bool Move(float time)
        {
            // ラインを移動
            for(int i = 0; i < slideNoteLineViewList.Count; i++)
            {
                slideNoteLineViewList[i].Move();
            }
            // ダミーノートを更新
            UpdateDummyNote(time);
            // 最後のノートが判定されていたら View を削除
            if (noteList[noteList.Count-1].judged)
            {
                Remove();
                return false;
            }
            return true;
        }

        /// <summary>
        /// ノートを判定するメソッド
        /// </summary>
        /// <param name="time"> 判定時間 </param>
        /// <param name="touchType"> タッチの種類 </param>
        /// <param name="posX"> タッチ位置 </param>
        /// <returns> 判定の種類 </returns>
        public JudgementType Judge(float time, int touchType, float posX)
        {
            // 先頭ノートが判定済みかつ末尾が未判定の場合は、ダミーノートの上をなぞっていなければコンボを切る
            if (noteList[0].judged && !noteList[noteList.Count - 1].judged && dummyNote != null)
            {
                if (dummyNotePressed && !dummyNote.Touched(posX))
                {
                    dummyNotePressed = false;
                    return JudgementType.Miss;
                }
            }
            // 末尾が判定済みなら Judged を返す
            if (noteList[noteList.Count - 1].judged)
            {
                return JudgementType.Judged;
            }
            return JudgementType.None;
        }

        /// <summary>
        /// スライドノートの構成要素となるノートを生成するメソッド
        /// </summary>
        private void GenerateNotes()
        {
            // 構成要素数
            int size = judgementTimeList.Count;
            // 先頭のノートを生成
            TapNote tapNote = new TapNote(generatedTime, judgementTimeList[0], noteParamList[0], true);
            noteList.Add((Note)tapNote);
            noteTransformList.Add(tapNote.view.gameObject.transform);
            // 残りのノートを生成
            for (int i = 1; i < size; i++)
            {
                int connectionType = noteParamList[i].connectionType;
                // ノートを表示する場合
                if (connectionType == 0 || connectionType == 1)
                {
                    noteParamList[i].var_4 = judgementTimeList[0];
                    SwipeNote swipeNote = new SwipeNote(generatedTime, judgementTimeList[i], noteParamList[i], true);
                    noteList.Add((Note)swipeNote);
                    noteTransformList.Add(swipeNote.view.gameObject.transform);
                }
                // ノートを非表示にする場合
                else if (connectionType == 2)
                {
                    noteParamList[i].type = ElementType.None;
                    noteParamList[i].var_4 = judgementTimeList[0];
                    SwipeNote swipeNote = new SwipeNote(generatedTime, judgementTimeList[i], noteParamList[i], true);
                    swipeNote.judged = true;
                    noteList.Add((Note)swipeNote);
                    noteTransformList.Add(swipeNote.view.gameObject.transform);
                }
                else
                {
                    Debug.Assert(false);
                }
            }
            // ラインを生成
            GenerateLine(noteTransformList);
        }

        /// <summary>
        /// スライドノートの構成要素となるラインを生成するメソッド
        /// </summary>
        /// <param name="noteTransformList"> ノートの Transform のリスト </param>
        private void GenerateLine(List<Transform> noteTransformList)
        {
            int size = noteParamList.Count;
            for (int i = 0; i < size - 1; i++)
            {
                SlideNoteLineView slideNoteLineView = pool.GetObject().GetComponent<SlideNoteLineView>();
                slideNoteLineView.Init(noteTransformList[i], noteTransformList[i+1], noteParamList[i].var_2, noteParamList[i].var_3);
                slideNoteLineViewList.Add(slideNoteLineView);
            }
        }

        /// <summary>
        /// ダミーノートを更新するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        private void UpdateDummyNote(float time)
        {
            if (dummyNote == null)
            {
                // 先頭ノートが判定済み、または判定ラインを超えたらダミーノートを生成
                if (noteList[0].judged || time > noteList[0].judgementTime - 0.008f)
                {
                    dummyNote = new DummyNote(0, 0, noteParamList[0], true);
                    dummyNote.view.Move(0.0f);
                    dummyNoteLineIndex = 0;
                    dummyNote.view.MoveX(slideNoteLineViewList[dummyNoteLineIndex].GetPosX((time - noteList[dummyNoteLineIndex].judgementTime) * judgementTimeDiffInverseList[dummyNoteLineIndex]));
                }
            }
            else
            {
                // 末尾ノートが判定済み、または判定ラインを超えたらダミーノートを削除
                if (noteList[noteList.Count-1].judged || time > noteList[noteList.Count-1].judgementTime - 0.008f)
                {
                    dummyNote.view.Remove();
                    // ラインを押し続けていたら判定に加える
                    if (dummyNotePressed)
                    {
                        judgementManager.AddJudgement(JudgementType.Perfect);
                        dummyNotePressed = false;
                    }
                }
                else
                {
                    // 判定ライン上にあるラインのインデックスまで進める
                    while (dummyNoteLineIndex < noteList.Count - 1 && time > noteList[dummyNoteLineIndex + 1].judgementTime - 0.008f)
                    {
                        dummyNoteLineIndex++;
                    }
                    dummyNote.view.MoveX(slideNoteLineViewList[dummyNoteLineIndex].GetPosX((time - noteList[dummyNoteLineIndex].judgementTime) * judgementTimeDiffInverseList[dummyNoteLineIndex]));
                }
            }
        }

        /// <summary>
        /// ノートとラインを削除するメソッド
        /// </summary>
        private void Remove()
        {
            for (int i = 0; i < noteList.Count; i++)
            {
                noteList[i].view.Remove();
            }
            for (int i = 0; i < slideNoteLineViewList.Count; i++)
            {
                slideNoteLineViewList[i].Remove();
            }
        }
        #endregion
    }
}