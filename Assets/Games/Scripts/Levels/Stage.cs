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
    [SerializeField] protected Stage[] floors;

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        Player playerCharacter = FindPlayerCharacter();
        foreach (Stage floor in floors)
        {
            ResetBricks(floor.transform);
            GenerateBricks(floor.transform, playerCharacter);
        }
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

    private void GenerateBricks(Transform floor, Player playerCharacter)
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
                    brick.ChangeColor(ColorType.Red);
                }
            }
        }
    }

    private Player FindPlayerCharacter()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 0)
        {
            return playerObjects[0].GetComponent<Player>();
        }
        return null;
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