using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] protected Transform startPoint;
    [SerializeField] protected Transform finishPoint;
    [SerializeField] protected Stage[] floors;

    public Vector3 GetStartPoint()
    {
        if (startPoint == null) return Vector3.zero;
        return startPoint.position;
    }

    public Vector3 GetFinishPoint()
    {
        if (finishPoint == null) return Vector3.zero;
        return finishPoint.position;
    }

    public void OnInit()
    {
        //Setup something here
    }
}