using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lyrid.GameScene.Audio;
using Lyrid.GameScene.Charts;
using Lyrid.GameScene.Input;
using Lyrid.GameScene.Lanes;
using Lyrid.GameScene.Score;

namespace Lyrid.GameScene.Notes
{
    /// <summary>
    /// ノート判定を管理するクラス
    /// </summary>
    public class JudgementManager
    {
        #region Field
        /// <summary> 判定対象の通常ノートのリスト </summary>
        private List<Note> targetNoteList;
        /// <summary> 判定対象の通常ノートのリスト </summary>
        private List<SlideNote> targetSlideNoteList;
        /// <summary> オートプレイかどうか </summary>
        private bool autoPlay;
        /// <summary> AudioManager のインスタンス </summary>
        private AudioManager audioManager;
        /// <summary> ScoreManager のインスタンス </summary>
        private ScoreManager scoreManager;
        /// <summary> TouchInputManager のインスタンス </summary>
        private TouchInputManager touchInputManager;
        /// <summary> LanesManager のインスタンス </summary>
        private LanesManager lanesManager;
        #endregion

        #region Constructor
        /// <param name="audioManager"> AudioManager のインスタンス </param>
        /// <param name="scoreManager"> ScoreManager のインスタンス </param>
        /// <param name="touchInputManager"> TouchInputManager のインスタンス </param>
        /// <param name="autoPlay"> オートプレイかどうか </param>
        public JudgementManager(
            AudioManager audioManager,
            ScoreManager scoreManager,
            TouchInputManager touchInputManager,
            bool autoPlay)
        {
            this.audioManager = audioManager;
            this.scoreManager = scoreManager;
            this.touchInputManager = touchInputManager;
            this.autoPlay = autoPlay;
            lanesManager = GameObject.Find("Lanes").GetComponent<LanesManager>();
            targetNoteList = new List<Note>(30);
            targetSlideNoteList = new List<SlideNote>(20);
        }
        #endregion

        #region Methods
        /// <summary>
        /// GameSceneManager からフレーム毎に呼び出されるメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        public void ManagedUpdate(float time)
        {
            Judge(time);
        }

        /// <summary>
        /// 状態をリセットするメソッド
        /// </summary>
        public void Reset()
        {
            targetNoteList = new List<Note>(30);
            targetSlideNoteList = new List<SlideNote>(20);
        }

        /// <summary>
        /// 判定対象を追加するメソッド (通常ノート)
        /// 常に判定時間の昇順になるようにする
        /// </summary>
        /// <param name="target"> 判定対象 </param>
        public void AddTarget(Note target)
        {
            for (int i = targetNoteList.Count - 1; i >= 0 ; i++)
            {
                if (targetNoteList[i].judgementTime <= target.judgementTime)
                {
                    targetNoteList.Insert(i + 1, target);
                    return;
                }
            }
            targetNoteList.Insert(0, target);
        }

        /// <summary>
        /// 判定対象を追加するメソッド (スライドノート)
        /// </summary>
        /// <param name="target"> 判定対象 </param>
        public void AddTarget(SlideNote target)
        {
            targetSlideNoteList.Add(target);
        }

        /// <summary>
        /// 判定を追加するメソッド
        /// </summary>
        /// <param name="judgementType"> 判定の種類 </param>
        public void AddJudgement(JudgementType judgementType)
        {
            scoreManager.AddScore(judgementType);
        }

        /// <summary>
        /// 判定するメソッド
        /// </summary>
        /// <param name="time"> 現在の時間 </param>
        private void Judge(float time)
        {
            // オートプレイの場合
            if (autoPlay)
            {
                // 通常ノートの処理
                for (int i = 0; i < targetNoteList.Count; i++)
                {
                    Note note = targetNoteList[i];
                    // 判定時間になったら Perfect 判定とする
                    if (!note.judged && note.judgementTime - 0.008f <= time)
                    {
                        audioManager.PlayTapSound();
                        lanesManager.lanes[note.noteParam.laneNum].EffectLightUp(note.noteParam.type, note.view.width);
                        scoreManager.AddScore(JudgementType.Perfect);
                        note.Remove();
                        note.judged = true;
                    }
                }
            }
            // 通常プレイの場合
            else
            {
                IReadOnlyList<int> touchTypeList = touchInputManager.touchTypeList;
                IReadOnlyList<float> posXList = touchInputManager.posXList;

                // 各 Touch について通常ノートを判定
                for (int i = 0; i < touchTypeList.Count; i++)
                {
                    int touchType = touchTypeList[i];
                    float posX = posXList[i];

                    // 判定対象リストを前側からチェック
                    for (int j = 0; j < targetNoteList.Count; j++)
                    {
                        Note note = targetNoteList[j];

                        // 判定済みであれば無視
                        if (note.judged)
                        {
                            continue;
                        }

                        JudgementType judgementType = note.Judge(time, touchType, posX);

                        // 判定が None であれば無視
                        if (judgementType == JudgementType.None)
                        {
                            continue;
                        }
                        // そのほかの場合はそれを判定とする
                        else
                        {
                            audioManager.PlayTapSound();
                            lanesManager.lanes[note.noteParam.laneNum].EffectLightUp(note.noteParam.type, note.view.width);
                            scoreManager.AddScore(judgementType);
                            note.judged = true;
                            break;
                        }
                    }
                }
                // タッチされていないときは、touchType = 0 で通常ノートを判定
                if (touchTypeList.Count == 0)
                {
                    // 判定対象リストを前側からチェック
                    for (int i = 0; i < targetNoteList.Count; i++)
                    {
                        Note note = targetNoteList[i];

                        // 判定済みであれば無視
                        if (note.judged)
                        {
                            continue;
                        }

                        JudgementType judgementType = note.Judge(time, 0, 0);

                        // 判定が Miss であれば判定する
                        if (judgementType == JudgementType.Miss)
                        {
                            scoreManager.AddScore(judgementType);
                            note.judged = true;
                        }
                    }
                }

                // スライドノートを判定
                for (int i = 0; i < targetSlideNoteList.Count; i++)
                {
                    SlideNote slideNote = targetSlideNoteList[i];

                    // 判定済みであれば無視
                    if (slideNote.judged)
                    {
                        continue;
                    }

                    JudgementType judgementType = slideNote.Judge(time, touchTypeList, posXList);

                    // 判定が None であれば無視
                    if (judgementType == JudgementType.None)
                    {
                        continue;
                    }
                    // そのほかの場合はそれを判定とする
                    else
                    {
                        scoreManager.AddScore(judgementType);
                        slideNote.judged = true;
                    }
                }
            }

            // すでに判定済みのノートを削除
            for (int i = targetNoteList.Count - 1; i >= 0; i--)
            {
                if (targetNoteList[i].judged)
                {
                    targetNoteList.RemoveAt(i);
                }
            }
            for (int i = targetSlideNoteList.Count - 1; i >= 0; i--)
            {
                if (targetSlideNoteList[i].judged)
                {
                    targetSlideNoteList.RemoveAt(i);
                }
            }
        }
        #endregion
    }
}