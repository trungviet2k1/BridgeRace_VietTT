using UnityEngine;

public class LevelManagement : Singleton<LevelManagement>
{
    [Header("Level Settings")]
    [SerializeField] protected Level[] levels;
    [SerializeField] protected Player player;

    [HideInInspector] public Level currentLevel;
    private int levelIndex = 0;

    private void Start()
    {
        OnLoadLevel(levelIndex);
        OnInit();
    }

    public void OnInit()
    {
        player.OnInit();
        if (currentLevel == null) return;
        player.transform.position = currentLevel.GetStartPoint();
    }

    public void OnReset()
    {
        //player.OnDespawn();
        //for (int i = 0; i < bots.Count; i++)
        //{
        //    bots[i].OnDespawn();
        //}

        //bots.Clear();
        HBPool.CollectAll();
    }

    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        GameObject map = GameObject.Find("======Map======");
        if (map == null) return;

        if (level < levels.Length)
        {
            currentLevel = Instantiate(levels[level], map.transform);
            currentLevel.OnInit();
        }
        else
        {
            NoMoreLevels();
        }
    }

    public void LoadNextLevel()
    {
        levelIndex++;
        OnReset();
        OnLoadLevel(levelIndex);
        OnInit();
    }

    private void NoMoreLevels()
    {
        Debug.Log("No more levels to load. Implement the desired action here.");
        Application.Quit();
    }
}