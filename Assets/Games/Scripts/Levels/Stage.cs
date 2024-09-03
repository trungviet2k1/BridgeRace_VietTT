using Scriptable;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header("Brick Settings")]
    [SerializeField] protected Brick brickPrefab;
    [SerializeField] protected int rows;
    [SerializeField] protected int columns;
    [SerializeField] protected float rowSpacing;
    [SerializeField] protected float columnSpacing;

    [Header("Floor")]
    [SerializeField] protected Stage floor;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        Character[] characters = FindCharacters();
        if (characters == null || characters.Length == 0) return;

        Player playerCharacter = null;
        List<Bot> bots = new();

        foreach (Character character in characters)
        {
            if (character is Player player)
            {
                playerCharacter = player;
            }
            else if (character is Bot bot)
            {
                bots.Add(bot);
            }
        }

        if (playerCharacter == null || bots.Count < 3) return;

        ResetBricks(floor.transform);
        GenerateBricks(floor.transform, playerCharacter, bots.ToArray());
    }

    private void ResetBricks(Transform floor)
    {
        Transform brickPoint = floor.Find("BrickPoint");
        if (brickPoint == null) return;

        foreach (Transform child in brickPoint)
        {
            if (child.TryGetComponent<Brick>(out var brick))
            {
                HBPool.Despawn(brick);
            }
        }
    }

    private void GenerateBricks(Transform floor, Player playerCharacter, Bot[] bots)
    {
        Transform brickPoint = floor.Find("BrickPoint");
        if (brickPoint == null) return;

        Vector3 startPosition = brickPoint.position
            - new Vector3((columns - 1) * columnSpacing / 2, 0, (rows - 1) * rowSpacing / 2);

        List<int> positions = new();
        for (int i = 0; i < rows * columns; i++)
        {
            positions.Add(i);
        }

        Shuffle(positions);

        HashSet<int> playerBrickPositions = new(positions.GetRange(0, 10));
        HashSet<int>[] botBrickPositions = new HashSet<int>[bots.Length];

        for (int i = 0; i < bots.Length; i++)
        {
            botBrickPositions[i] = new HashSet<int>(positions.GetRange(10 + i * 10, 10));
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 brickPosition = new(j * columnSpacing, 0.1f, i * rowSpacing);
                Vector3 finalPosition = startPosition + brickPosition;
                Quaternion brickRotation = Quaternion.Euler(0, 90, 0);
                Brick brick = HBPool.Spawn<Brick>(PoolType.Brick, finalPosition, brickRotation);
                brick.transform.SetParent(brickPoint);

                int currentPosition = i * columns + j;

                if (playerBrickPositions.Contains(currentPosition))
                {
                    brick.ChangeColor(playerCharacter.color);
                }
                else
                {
                    for (int k = 0; k < bots.Length; k++)
                    {
                        if (botBrickPositions[k].Contains(currentPosition))
                        {
                            brick.ChangeColor(bots[k].color);
                            break;
                        }
                    }
                }
            }
        }
    }

    public List<Brick> FindBricksWithColor(ColorType colorType)
    {
        List<Brick> bricksWithColor = new();

        Transform brickPoint = transform.Find("BrickPoint");
        if (brickPoint == null) return bricksWithColor;

        foreach (Transform child in brickPoint)
        {
            if (child.TryGetComponent<Brick>(out Brick brick))
            {
                if (brick.ColorType() == colorType)
                {
                    bricksWithColor.Add(brick);
                }
            }
        }

        return bricksWithColor;
    }

    private Character[] FindCharacters()
    {
        GameObject[] characterObjects = GameObject.FindGameObjectsWithTag(Constants.CHARACTER);
        Character[] characters = new Character[characterObjects.Length];
        for (int i = 0; i < characterObjects.Length; i++)
        {
            characters[i] = characterObjects[i].GetComponent<Character>();
        }
        return characters;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}