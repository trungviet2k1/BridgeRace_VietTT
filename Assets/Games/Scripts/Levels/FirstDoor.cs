using UnityEngine;
using System.Collections.Generic;

public class FirstDoor : MonoBehaviour
{
    [SerializeField] protected BoxCollider firstDoor;
    private readonly List<Character> characters = new();

    private void Start()
    {
        characters.AddRange(FindAllCharacters());
        if (characters.Count == 0 || firstDoor == null) return;
    }

    private void Update()
    {
        UpdateDoorState();
    }

    private void UpdateDoorState()
    {
        foreach (var character in characters)
        {
            int brickCount = character.GetBrickCount();
            if (brickCount > 0)
            {
                firstDoor.isTrigger = true;
                return;
            }
        }
    }

    private Character[] FindAllCharacters()
    {
        return GameObject.FindObjectsOfType<Character>();
    }
}