using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// 同時押しノートのクラス
    /// </summary>
    public class NoteLine
    {
        #region Field
        private List<Note> noteList;
        private NoteLineView view;
        private Transform leftTf;
        private Transform rightTf;
        #endregion

        #region Property
        public float judgementTime { get; private set; }
        public int noteNum { get { return noteList.Count; } }
        #endregion

        #region Constructor
        public NoteLine(float judgementTime)
        {
            this.judgementTime = judgementTime;
            noteList = new List<Note>();
            view = GameObject.FindWithTag("NoteLinePool").GetComponent<ObjectPool>().GetObject().GetComponent<NoteLineView>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// NoteLineManager から呼び出される更新メソッド
        /// </summary>
        public void UpdateLine()
        {
            // 判定済みのノートをリストから除外
            for (int i = 0; i < noteList.Count; i++)
            {
                if (noteList[i].judged)
                {
                    noteList.RemoveAt(i);
                }
            }

            // ノートが 0 個であれば View を削除する
            if (noteList.Count == 0)
            {
                view.Remove();
                return;
            }

            // ノートが 1 個であればラインの始点終点を 0 にする
            if (noteList.Count == 1)
            {
                view.UpdateLine(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                return;
            }

            // 左端と右端のノートの Transform を取得
            float minX = 10000f;
            float maxX = -10000f;
            for (int i = 0; i < noteList.Count; i++)
            {
                Transform noteTf = noteList[i].view.tfCache;
                if (noteTf.position.x < minX)
                {
                    leftTf = noteTf;
                    minX = noteTf.position.x;
                }
                if (maxX < noteTf.position.x)
                {
                    rightTf = noteTf;
                    maxX = noteTf.position.x;
                }
            }

            // z 座標が同じであればラインの View を更新
            view.UpdateLine(leftTf.position, rightTf.position);
        }

        public void AddNote(Note note)
        {
            noteList.Add(note);
        }
        #endregion
    }
}