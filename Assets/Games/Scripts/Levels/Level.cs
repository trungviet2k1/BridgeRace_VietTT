using Scriptable;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] protected Transform[] startPoints;
    [SerializeField] protected Transform finishPoint;
    [SerializeField] protected int botAmount;

    [Header("List of Floors")]
    [SerializeField] protected List<Stage> stages;

    private Player player;
    private readonly List<Bot> bots = new();

    private void Start()
    {
        OnInit();
        player = FindObjectOfType<Player>();
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
        if (stages == null || stages.Count == 0) return;

        foreach (Stage stage in stages)
        {
            stage.OnInit();
        }
    }

    public void ResetLevel()
    {
        if (player == null || stages == null || stages.Count == 0) return;

        foreach (Stage stage in stages)
        {
            stage.ResetBricks(gameObject.transform);
            stage.GenerateBricks(stage.transform, player, bots.ToArray());
        }

        player.OnInit();

        foreach (Bot bot in bots)
        {
            bot.OnInit();
        }

        SpawnPlayerAndBots();
    }

    public void EndLevel()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(null);
            bots[i].StopMove();
            bots.Clear();
        }
    }

    private void SpawnPlayerAndBots()
    {
        Transform botContainer = GameObject.Find("Bot").transform;
        if (startPoints.Length < 1 || player == null || botContainer == null) return;

        List<Transform> availableStartPoints = new(startPoints);

        // Set position for player
        Transform playerStartPoint = GetRandomStartPoint(availableStartPoints);
        player.TF.position = playerStartPoint.position;

        int effectiveBotAmount = Mathf.Min(botAmount, availableStartPoints.Count);
        HashSet<ColorType> usedColors = new() { player.color, ColorType.Gray };

        for (int i = 0; i < effectiveBotAmount; i++)
        {
            Transform botStartPoint = GetRandomStartPoint(availableStartPoints);
            Bot bot = HBPool.Spawn<Bot>(PoolType.Bot, botStartPoint.position, Quaternion.identity);
            bot.TF.SetParent(botContainer);

            Bot botCharacter = bot.GetComponent<Bot>();
            ColorType botColor;
            do
            {
                botColor = GetRandomColor();
            } while (usedColors.Contains(botColor));

            botCharacter.ChangeColor(botColor);
            usedColors.Add(botColor);
            bots.Add(botCharacter);
        }

        SpawnPlayerAndBotsInStages(player, bots);
    }

    private void SpawnPlayerAndBotsInStages(Player player, List<Bot> bots)
    {
        foreach (Stage stage in stages)
        {
            stage.ResetBricks(transform);
            stage.GenerateBricks(stage.transform, player, bots.ToArray());
        }
    }

    private Transform GetRandomStartPoint(List<Transform> availableStartPoints)
    {
        if (availableStartPoints.Count == 0) return null;

        int randomIndex = Random.Range(0, availableStartPoints.Count);
        Transform selectedPoint = availableStartPoints[randomIndex];
        availableStartPoints.RemoveAt(randomIndex);
        return selectedPoint;
    }

    private ColorType GetRandomColor()
    {
        return (ColorType)Random.Range(1, System.Enum.GetValues(typeof(ColorType)).Length);
    }

    private void Update()
    {
        foreach (Stage stage in stages)
        {
            if (IsCharacterOnStage(stage))
            {
                stage.GenerateBricks(stage.transform, player, bots.ToArray());
            }
        }
    }

    private bool IsCharacterOnStage(Stage stage)
    {
        Character[] characters = stage.FindCharacters();
        foreach (Character character in characters)
        {
            if (character != null)
            {
                Vector3 characterPosition = character.TF.position;
                Ray ray = new(characterPosition + Vector3.up * 1f, Vector3.down);
                if (Physics.Raycast(ray, out RaycastHit hit, 2f, LayerMask.GetMask("Ground")))
                {
                    if (hit.collider.transform.IsChildOf(stage.transform))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}