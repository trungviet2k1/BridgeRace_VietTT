using Scriptable;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [Header("Stair Settings")]
    public ColorType stairColor;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private ColorData colorData;
    [SerializeField] private LayerMask characterLayerMask;

    [HideInInspector] public bool isActive;
    private Character currentCharacter;

    private void Start()
    {
        if (meshRenderer == null) return;

        SetStairActive(false);
    }

    public void ActivateStair(Character character)
    {
        if (character.GetBrickCount() > 0 && !isActive)
        {
            SetStairActive(true);
            ChangeStairColor(character.color);
            ConsumeBrick(character);
            currentCharacter = character;
        }
    }

    public void SetStairActive(bool isActive)
    {
        meshRenderer.enabled = isActive;
        this.isActive = isActive;
    }

    public void ChangeStairColor(ColorType colorType)
    {
        if (colorData == null) return;
        stairColor = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }

    private void ConsumeBrick(Character character)
    {
        if (character != null && character.GetBrickCount() > 0)
        {
            character.RemoveBrick();
        }
    }

    public Character GetCurrentCharacter()
    {
        return currentCharacter;
    }
}