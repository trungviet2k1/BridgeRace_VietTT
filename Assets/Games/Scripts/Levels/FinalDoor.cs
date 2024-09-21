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
        if (other.TryGetComponent(out Character character) && level != null && isBridgeComplete)
        {
            StartCoroutine(HandleCharacterArrival(character));
        }
    }

    private IEnumerator HandleCharacterArrival(Character character)
    {
        yield return new WaitForSeconds(0f);
        Vector3 finishPoint = level.GetFinishPoint();
        character.TF.position = finishPoint;
        character.TF.eulerAngles = Vector3.up * 180;
        character.ChangeAnim(Constants.ANIM_DANCE);
        level.EndLevel();

        yield return new WaitForSeconds(5f);
        LevelManagement.Ins.LoadNextLevel();
    }

    public void SetBridgeComplete()
    {
        isBridgeComplete = true;
    }
}