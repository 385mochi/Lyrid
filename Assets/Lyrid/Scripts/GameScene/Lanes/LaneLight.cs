using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaneLight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer laneLight;

    private Tweener tweener;

    public void LightUp()
    {
        if (tweener != null)
        {
            tweener.Kill();
        }
        laneLight.color = new Color(laneLight.color.r, laneLight.color.g, laneLight.color.b, 0.5f);
        tweener = DOTween.ToAlpha(
            ()=> laneLight.color,
            color => laneLight.color = color,
            0f, // 目標値
            0.4f // 所要時間
        ).OnComplete(() => {tweener = null;});
    }
}
