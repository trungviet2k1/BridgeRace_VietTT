using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform startPoint;
    [SerializeField] protected Transform finishPoint;
    [SerializeField] protected Stage[] floors;

    public Vector3 GetStartPoint()
    {
        return startPoint != null ? startPoint.position : Vector3.zero;
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
}