using UnityEngine;
using System.Collections;
using Scriptable;

public class SecondDoor : MonoBehaviour
{
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private ColorData colorData;

    public void ChangeColorAndHide(Character character)
    {
        ChangeDoorColor(character);
        StartCoroutine(HideDoor());
    }

    private void ChangeDoorColor(Character character)
    {
        if (meshRenderer == null || colorData == null) return;
        ColorType playerColor = character.color;
        meshRenderer.material = colorData.GetMat(playerColor);
    }

    private IEnumerator HideDoor()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}