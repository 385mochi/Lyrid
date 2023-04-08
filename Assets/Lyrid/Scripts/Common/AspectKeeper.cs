using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    #region Field
    /// <summary> 対象とするカメラ </summary>
    [SerializeField] private Camera targetCamera;
    /// <summary> 目的解像度 </summary>
    [SerializeField] private Vector2 aspectVec;
    #endregion

    #region methods
    void Start()
    {
        Execute();
    }

    /// <summary>
    /// AspectKeeper を実行するメソッド
    /// </summary>
    private void Execute()
    {
        //画面のアスペクト比
        var screenAspect = Screen.width / (float)Screen.height;
        //目的のアスペクト比
        var targetAspect = aspectVec.x / aspectVec.y;
        //目的アスペクト比にするための倍率
        var magRate = targetAspect / screenAspect;
        //Viewport初期値でRectを作成
        var viewportRect = new Rect(0, 0, 1, 1);
        if (magRate < 1)
        {
            //使用する横幅を変更
            viewportRect.width = magRate;
            //中央寄せ
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;
        }
        else
        {
            //使用する縦幅を変更
            viewportRect.height = 1 / magRate;
            //中央余生
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;
        }
        //カメラのViewportに適用
        targetCamera.rect = viewportRect;
    }
    #endregion
}