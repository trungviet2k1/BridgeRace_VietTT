using UnityEngine;

public class Player : Character
{
    [Header("Joystick Settings")]
    [SerializeField] protected Joystick joystick;

    [Header("Rigidbody")]
    [SerializeField] protected Rigidbody rb;

    private readonly float rotationSpeed = 700f;
    private readonly float movementThreshold = 0.001f;

    public override void OnInit()
    {
        base.OnInit();
    }

    protected override void HandleMovement()
    {
        if (joystick == null || rb == null) return;

        float moveInputX = joystick.Horizontal;
        float moveInputZ = joystick.Vertical;

        Vector3 movement = GetMoveSpeed() * Time.deltaTime * new Vector3(moveInputX, 0, moveInputZ).normalized;
        Vector3 nextPosition = rb.position + movement;

        if (CanMove(nextPosition))
        {
            rb.MovePosition(nextPosition);

            if (movement.magnitude > movementThreshold)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
                ChangeAnim(Constants.ANIM_RUN);
            }
            else
            {
                ChangeAnim(Constants.ANIM_IDLE);
            }
        }
        else
        {
            ChangeAnim(Constants.ANIM_IDLE);
        }
    }
}