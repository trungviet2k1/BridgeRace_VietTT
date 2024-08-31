using UnityEngine;

public class FirstDoor : MonoBehaviour
{
    [SerializeField] protected BoxCollider firstDoor;
    private Character playerCharacter;

    private void Start()
    {
        if (playerCharacter == null && firstDoor == null) return;
        playerCharacter = FindPlayerCharacter();
        UpdateDoorState();
    }

    private void Update()
    {
        UpdateDoorState();
    }

    private void UpdateDoorState()
    {
        if (playerCharacter != null && playerCharacter.GetBrickCount() > 0)
        {
            firstDoor.isTrigger = true;
        }
    }

    private Character FindPlayerCharacter()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 0)
        {
            return playerObjects[0].GetComponent<Character>();
        }
        return null;
    }
}