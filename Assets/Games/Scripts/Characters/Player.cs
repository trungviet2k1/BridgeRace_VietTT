using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Joystick Settings")]
    [SerializeField] protected Joystick joystick;

    [Header("Movement Settings")]
    [SerializeField] protected Rigidbody rb;

    private readonly float rotationSpeed = 700f;
    private readonly float movementThreshold = 0.001f;

    private List<Brick> collectedBricks = new();

    public override void OnInit()
    {
        base.OnInit();
        ChangeColor(color);
    }

    protected override void HandleMovement()
    {
        if (joystick == null || rb == null) return;

        float moveInputX = joystick.Horizontal;
        float moveInputZ = joystick.Vertical;

        Vector3 movement = GetMoveSpeed() * Time.deltaTime * new Vector3(moveInputX, 0, moveInputZ).normalized;

        rb.MovePosition(rb.position + movement);

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

    protected override void AddBrick()
    {
        throw new System.NotImplementedException();
    }

    protected override void RemoveBrick()
    {
        throw new System.NotImplementedException();
    }

    protected override void ClearBricks()
    {
        throw new System.NotImplementedException();
    }
}