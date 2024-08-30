using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] Transform startPoint;
    //[SerializeField] Transform finishPoint;

    public Vector3 GetStartPoint()
    {
        if (startPoint == null) return Vector3.zero;
        return startPoint.position;
    }

    public void OnInit()
    {
        //Setup something here
    }
}