using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private List<Stair> stairs;
    [SerializeField] private List<SecondDoor> secondDoors;
    [SerializeField] private FinalDoor finalDoor;

    private Character bridgeCompleter;

    private void Start()
    {
        StartCoroutine(CheckBridgeStatus());
    }

    private IEnumerator CheckBridgeStatus()
    {
        while (!IsBridgeComplete())
        {
            yield return new WaitForSeconds(0.5f);
        }
        OpenSecondDoors();
        if (finalDoor != null)
        {
            finalDoor.SetBridgeComplete();
        }
    }

    private bool IsBridgeComplete()
    {
        foreach (var stair in stairs)
        {
            if (!stair.isActive)
            {
                return false;
            }

            if (bridgeCompleter == null)
            {
                bridgeCompleter = stair.GetCurrentCharacter();
            }
        }
        return true;
    }

    private void OpenSecondDoors()
    {
        if (bridgeCompleter == null) return;

        foreach (var door in secondDoors)
        {
            door.ChangeColorAndHide(bridgeCompleter);
        }
    }
}