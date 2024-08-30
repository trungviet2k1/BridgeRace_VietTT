using Scriptable;
using UnityEngine;

public class Brick : GameUnit
{
    [Header("Brick Color Settings")]
    public ColorType colorType;
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected Renderer meshRenderer;

    public void ChangeColor(ColorType colorType)
    {
        this.colorType = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }

    public void ResetBrick()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        HBPool.Despawn(this);
    }
}