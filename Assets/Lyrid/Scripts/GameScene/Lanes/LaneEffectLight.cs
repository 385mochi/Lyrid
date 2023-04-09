using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Lyrid.GameScene.Charts;
using static Lyrid.GameScene.GameSceneConsts;

/// <summary>
/// レーンのタップエフェクトのクラス
/// </summary>
public class LaneEffectLight : MonoBehaviour
{
    #region Field
    /// <summary> 自身が持つ SpriteRenderer </summary>
    [SerializeField] private SpriteRenderer laneEffectLight;
    /// <summary> DOTween のための Tweener インスタンス </summary>
    private Tweener tweener;
    #endregion

    #region Methods
    /// <summary>
    /// レーンを光らせるメソッド
    /// </summary>
    /// <param name="type"> 要素の種類 </param>
    /// <param name="width"> レーンの幅を 1 としたときの幅 </param>
    public void LightUp(ElementType type, float width)
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        Color32 color;
        switch (type)
        {
            case ElementType.Tap:
                color = TAP_NOTE_COLOR;
                break;
            case ElementType.Swipe:
                color = SWIPE_NOTE_COLOR;
                break;
            case ElementType.Flick:
                color = FLICK_NOTE_COLOR;
                break;
            case ElementType.Slide:
                color = SLIDE_NOTE_COLOR;
                break;
            default:
                color = new Color32(0, 0, 0, 0);
                break;
        }
        color.a = 200;
        Vector3 scale = gameObject.transform.localScale;
        scale.x = width;
        gameObject.transform.localScale = scale;
        laneEffectLight.color = color;
        tweener = DOTween.ToAlpha(
            () => laneEffectLight.color,
            color => laneEffectLight.color = color,
            0f, // 目標値
            0.4f // 所要時間
        ).OnComplete(() => {tweener = null;});
    }
    #endregion
}
