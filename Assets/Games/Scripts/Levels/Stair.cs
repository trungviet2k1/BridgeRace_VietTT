using Scriptable;
using UnityEngine;

public class Stair : MonoBehaviour
{
    [Header("Stair Settings")]
    public ColorType stairColor;
    [SerializeField] protected Renderer meshRenderer;
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected LayerMask characterLayerMask;

    [HideInInspector] public bool isActive;
    [HideInInspector] public Bridge parentBridge;
    private Character currentCharacter;

    private void Start()
    {
        if (meshRenderer == null) return;
        SetStairActive(false);
        parentBridge = GetComponentInParent<Bridge>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();
        if (character != null && ((1 << other.gameObject.layer) & characterLayerMask) != 0)
        {
            if (parentBridge != null && parentBridge.bridgeCompleter != null && parentBridge.bridgeCompleter != character) return;
            ActivateStair(character);
        }
    }

    public void ActivateStair(Character character)
    {
        if (character.GetBrickCount() > 0 && !isActive)
        {
            SetStairActive(true);
            ChangeStairColor(character.color);
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

    public Character GetCurrentCharacter()
    {
        return currentCharacter;
    }
}