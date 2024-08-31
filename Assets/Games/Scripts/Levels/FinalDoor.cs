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
        if (level == null) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character player) && level != null && isBridgeComplete)
        {
            StartCoroutine(HandlePlayerArrival(player));
        }
    }

    private IEnumerator HandlePlayerArrival(Character player)
    {
        yield return new WaitForSeconds(1f);
        Vector3 finishPoint = level.GetFinishPoint();
        player.transform.position = finishPoint;
        player.ChangeAnim(Constants.ANIM_DANCE);
        player.transform.eulerAngles = Vector3.up * 180;
        player.OnInit();

        yield return new WaitForSeconds(5f);
        LevelManagement.Ins.LoadNextLevel();
    }

    public void SetBridgeComplete()
    {
        isBridgeComplete = true;
    }
}