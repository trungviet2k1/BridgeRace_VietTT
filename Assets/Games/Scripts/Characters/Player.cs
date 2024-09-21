using UnityEngine;

public class Player : Character
{
    [Header("Joystick Settings")]
    [SerializeField] protected Joystick joystick;
    [SerializeField] protected RectTransform rectTransform;
    [SerializeField] protected CanvasGroup canvasGroup;

    [Header("Rigidbody")]
    [SerializeField] protected Rigidbody rb;

    private readonly float rotationSpeed = 700f;
    private readonly float movementThreshold = 0.001f;

    public override void OnInit()
    {
        base.OnInit();
        if (joystick == null && canvasGroup == null) return;
        canvasGroup.alpha = 0f;
        TF.position = Vector3.zero;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        StopMovement();
    }

    protected override void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            JoystickPosition(Input.mousePosition);
        }
        HandleJoystickMovement();
    }

    private void HandleJoystickMovement()
    {
        if (joystick == null || rb == null) return;
        float moveInputX = joystick.Horizontal;
        float moveInputZ = joystick.Vertical;

        Vector3 movement = GetMoveSpeed() * Time.deltaTime * new Vector3(moveInputX, 0, moveInputZ).normalized;
        Vector3 nextPosition = rb.position + movement;
        bool isMoving = Mathf.Abs(movement.magnitude) > movementThreshold;

        if (CanMove(nextPosition))
        {
            rb.MovePosition(nextPosition);
            if (isMoving)
            {
                ChangeAnim(Constants.ANIM_RUN);
                Quaternion targetRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
            }
        }

        if (!isMoving || !CanMove(nextPosition))
        {
            StopMovement();
        }
    }

    private void JoystickPosition(Vector2 position)
    {
        rectTransform.position = position;
    }

    private void StopMovement()
    {
        ChangeAnim(Constants.ANIM_IDLE);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}