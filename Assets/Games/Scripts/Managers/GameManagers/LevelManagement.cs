using UnityEngine;

public class LevelManagement : Singleton<LevelManagement>
{
    [Header("Level Settings")]
    [SerializeField] protected Level[] levels;
    [SerializeField] protected Character characters;

    [HideInInspector] public Level currentLevel;
    private int levelIndex = 0;

    private void Start()
    {
        OnLoadLevel(levelIndex);
        OnInit();
    }

    public void OnInit()
    {
        characters.OnInit();
        if (currentLevel == null) return;
        characters.transform.position = currentLevel.GetStartPoint();
        characters.ChangeAnim(Constants.ANIM_IDLE);
    }

    public void OnReset()
    {
        if (currentLevel == null) return;
        currentLevel.ResetLevel();
        characters.ClearBricks();
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
        characters.ClearBricks();

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