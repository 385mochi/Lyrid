using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// レーンのタップエフェクトのクラス
/// </summary>
public class LaneLight : MonoBehaviour
{
    #region Field
    /// <summary> 自身が持つ SpriteRenderer </summary>
    [SerializeField] private SpriteRenderer laneLight;
    /// <summary> DOTween のための Tweener インスタンス </summary>
    private Tweener tweener;
    #endregion

    #region Methods
    /// <summary>
    /// レーンを光らせるメソッド
    /// </summary>
    public void LightUp()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        laneLight.color = new Color(laneLight.color.r, laneLight.color.g, laneLight.color.b, 0.5f);
        tweener = DOTween.ToAlpha(
            () => laneLight.color,
            color => laneLight.color = color,
            0f, // 目標値
            0.4f // 所要時間
        ).OnComplete(() => {tweener = null;});
    }
    #endregion
}
