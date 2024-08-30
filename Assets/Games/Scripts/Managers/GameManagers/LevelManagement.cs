
using UnityEngine;

public class LevelManagement : Singleton<LevelManagement>
{
    [Header("Level Settings")]
    [SerializeField] Level[] levels;
    [SerializeField] Player player;

    private Level currentLevel;
    private int currentLevelIndex = 0;

    public void Start()
    {
        OnLoadLevel(0);
        OnInit();
    }

    //khoi tao trang thai bat dau game
    public void OnInit()
    {
        player.OnInit();
        if (currentLevel == null) return;
        player.transform.position = currentLevel.GetStartPoint();
    }

    //reset trang thai khi ket thuc game
    public void OnReset()
    {
        //player.OnDespawn();
        //for (int i = 0; i < bots.Count; i++)
        //{
        //    bots[i].OnDespawn();
        //}

        //bots.Clear();
        //SimplePool.CollectAll();
    }

    //tao prefab level moi
    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        GameObject map = GameObject.Find("Map");
        if (map == null) return;

        currentLevel = Instantiate(levels[level], map.transform);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
        OnLoadLevel(currentLevelIndex);
    }
}