﻿using Scriptable;
using UnityEngine;

public class Brick : GameUnit
{
    [Header("Brick Color Settings")]
    public ColorType colorType;
    [SerializeField] private ColorData colorData;
    [SerializeField] private Renderer meshRenderer;

    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }

    public ColorType ColorType()
    {
        return colorType;
    }

    public void BrickSpawn()
    {
        if (gameObject == null) return;
        gameObject.SetActive(true);
    }

    public void BrickDespawn()
    {
        OnDespawn(0f);
    }
}