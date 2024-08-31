using System.Collections;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private BoxCollider finalDoor;
    private Level level;
    private bool isBridgeComplete = false;

    private void Start()
    {
        if (finalDoor == null) return;
        level = GetComponentInParent<Level>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && level != null && isBridgeComplete)
            {
                StartCoroutine(HandlePlayerArrival(player));
            }
        }
    }

    private IEnumerator HandlePlayerArrival(Player player)
    {
        yield return new WaitForSeconds(1f);

        if (player != null && level != null)
        {
            Vector3 finishPoint = level.GetFinishPoint();
            player.transform.position = finishPoint;
        }
    }

    public void SetBridgeComplete()
    {
        isBridgeComplete = true;
    }
}