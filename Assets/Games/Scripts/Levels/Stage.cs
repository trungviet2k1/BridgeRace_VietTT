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

    private bool bricksGenerated = false;

    private void Start()
    {
        OnInit();
        ResetBricks(gameObject.transform);
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

        if (playerCharacter == null || bots.Count < 1) return;

        if (bricksGenerated)
        {
            ResetBricks(gameObject.transform);
            GenerateBricks(gameObject.transform, playerCharacter, bots.ToArray());
        }

    }

    public void ResetBricks(Transform floor)
    {
        Transform brickPoint = floor.Find("BrickPoint");
        if (brickPoint == null) return;

        List<Brick> bricks = new();
        foreach (Transform child in brickPoint)
        {
            if (child.TryGetComponent<Brick>(out var brick))
            {
                bricks.Add(brick);
            }
        }

        foreach (var brick in bricks)
        {
            HBPool.Despawn(brick);
        }

        bricksGenerated = false;
    }

    public void GenerateBricks(Transform floor, Player playerCharacter, Bot[] bots)
    {
        if (bricksGenerated) return;

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

        int totalCharacters = bots.Length + 1;
        int bricksPerCharacter = Mathf.Max(1, (rows * columns) / totalCharacters);

        HashSet<int> playerBrickPositions = new(positions.GetRange(0, bricksPerCharacter));
        HashSet<int>[] botBrickPositions = new HashSet<int>[bots.Length];

        for (int i = 0; i < bots.Length; i++)
        {
            int startIdx = bricksPerCharacter + i * bricksPerCharacter;
            int endIdx = Mathf.Min(startIdx + bricksPerCharacter, positions.Count);
            botBrickPositions[i] = new HashSet<int>(positions.GetRange(startIdx, endIdx - startIdx));
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 brickPosition = new(j * columnSpacing, 0.1f, i * rowSpacing);
                Vector3 finalPosition = startPosition + brickPosition;
                Quaternion brickRotation = Quaternion.Euler(0, 90, 0);
                Brick brick = HBPool.Spawn<Brick>(PoolType.Brick, finalPosition, brickRotation);
                if (brick == null) continue;
                brick.TF.SetParent(brickPoint);

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

        bricksGenerated = true;
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

    public Character[] FindCharacters()
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