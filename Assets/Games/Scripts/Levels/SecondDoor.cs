using UnityEngine;
using System.Collections;
using Scriptable;

public class SecondDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private Collider doorCollider;
    [SerializeField] private ColorData colorData;

    public void ChangeColorAndHide(Character character)
    {
        if (meshRenderer == null || doorCollider == null || colorData == null) return;
        ChangeDoorColor(character);
        StartCoroutine(HideDoor());
    }

    private void ChangeDoorColor(Character character)
    {
        ColorType playerColor = character.color;
        meshRenderer.material = colorData.GetMat(playerColor);
    }

    private IEnumerator HideDoor()
    {
        yield return new WaitForSeconds(0.5f);
        meshRenderer.enabled = false;
        doorCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        doorCollider.enabled = true;
    }
}