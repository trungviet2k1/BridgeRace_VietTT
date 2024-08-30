using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;

    [Header("Camera Settings")]
    [SerializeField] Vector3 offset = new(0, 5, -10);

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
    }
}