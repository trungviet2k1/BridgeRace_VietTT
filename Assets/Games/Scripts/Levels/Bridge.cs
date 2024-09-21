using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] protected List<Stair> stairs;
    [SerializeField] protected List<SecondDoor> secondDoors;
    [SerializeField] protected FinalDoor finalDoor;

    [HideInInspector] public Character bridgeCompleter;

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

        if (finalDoor != null)
        {
            finalDoor.SetBridgeComplete();
        }

        OpenSecondDoors();
    }

    public bool IsBridgeComplete()
    {
        foreach (var stair in stairs)
        {
            if (!stair.isActive)
            {
                return false;
            }
            else
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

        Debug.Log(bridgeCompleter);
    }
}