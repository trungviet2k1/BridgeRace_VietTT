using Scriptable;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] protected Transform[] startPoints;
    [SerializeField] protected Transform finishPoint;

    [Header("List of floors")]
    [SerializeField] protected Stage[] floors;

    [Header("Characters")]
    [SerializeField] private GameObject botPrefab;

    private void Start()
    {
        OnInit();
        SpawnPlayerAndBots();
    }

    public Vector3 GetStartPoint()
    {
        return startPoints.Length > 0 ? startPoints[0].position : Vector3.zero;
    }

    public Vector3 GetFinishPoint()
    {
        return finishPoint != null ? finishPoint.position : Vector3.zero;
    }

    public void OnInit()
    {
        if (floors == null || floors.Length == 0) return;

        foreach (Stage floor in floors)
        {
            floor.OnInit();
        }
    }

    public void ResetLevel()
    {
        foreach (Stage floor in floors)
        {
            floor.ResetBricks(floor.transform);
            floor.GenerateBricks(floor.transform, FindObjectOfType<Player>(), FindObjectsOfType<Bot>());
        }

        SpawnPlayerAndBots();
    }

    private void SpawnPlayerAndBots()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;

        Transform botContainer = GameObject.Find("Bot").transform;
        if (botContainer == null) return;

        if (startPoints.Length < 4) return;

        List<Transform> availableStartPoints = new(startPoints);

        // Set position for player
        Transform playerStartPoint = GetRandomStartPoint(availableStartPoints);
        player.transform.position = playerStartPoint.position;

        // Set positions for bots
        HashSet<ColorType> usedColors = new() { player.color, ColorType.Gray };
        for (int i = 0; i < 3; i++)
        {
            Transform botStartPoint = GetRandomStartPoint(availableStartPoints);
            //GameObject bot = Instantiate(botPrefab, botStartPoint.position, Quaternion.identity, botContainer.transform);
            Bot bot = HBPool.Spawn<Bot>(PoolType.Bot, botStartPoint.position, Quaternion.identity);
            bot.transform.SetParent(botContainer);

            Bot botCharacter = bot.GetComponent<Bot>();
            ColorType botColor;
            do
            {
                botColor = GetRandomColor();
            } while (usedColors.Contains(botColor));

            botCharacter.ChangeColor(botColor);
            usedColors.Add(botColor);
        }
    }

    // Get random starting point
    private Transform GetRandomStartPoint(List<Transform> availableStartPoints)
    {
        int randomIndex = Random.Range(0, availableStartPoints.Count);
        Transform selectedPoint = availableStartPoints[randomIndex];
        availableStartPoints.RemoveAt(randomIndex);
        return selectedPoint;
    }

    // Set random color for bots
    private ColorType GetRandomColor()
    {
        return (ColorType)Random.Range(0, System.Enum.GetValues(typeof(ColorType)).Length);
    }
}