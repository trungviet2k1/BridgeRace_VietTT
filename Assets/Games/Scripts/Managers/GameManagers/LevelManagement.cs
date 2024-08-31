
using UnityEngine;

public class LevelManagement : Singleton<LevelManagement>
{
    [Header("Level Settings")]
    [SerializeField] protected Level[] levels;
    [SerializeField] protected Player player;

    private Level currentLevel;

    public void Start()
    {
        OnLoadLevel(0);
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
        currentLevel.OnInit();
    }
}