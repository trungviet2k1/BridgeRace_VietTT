using Scriptable;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class Character : GameUnit
{
    [Header("Character Settings")]
    [SerializeField] protected float moveSpeed = 6f;
    [SerializeField] protected Animator animator;

    [Header("Skins")]
    [SerializeField] ColorData colorData;
    [SerializeField] Renderer meshRenderer;
    public ColorType color;

    [Header("Bricks")]
    [SerializeField] protected Transform brickHolder;
    [SerializeField] protected GameObject brickPrefab;

    [HideInInspector] public Stage stage;
    [HideInInspector] public LayerMask validLayerMask;
    protected List<GameObject> bricks = new();

    private string animName;
    private bool isOnValidSurface;
    private Vector3 lastPosition;
    private MeshRenderer cachedMeshRenderer;

    private void Start()
    {
        OnInit();
        ChangeColor(color);
        ChangeAnim(Constants.ANIM_IDLE);

        validLayerMask = LayerMask.GetMask("Ground", "Stair");
        lastPosition = TF.position;
        isOnValidSurface = IsOnValidSurface();
    }

    private void FixedUpdate()
    {
        if (HasMoved())
        {
            isOnValidSurface = IsOnValidSurface();
        }

        if (isOnValidSurface)
        {
            HandleMovement();
        }
        else
        {
            ChangeAnim(Constants.ANIM_IDLE);
        }
    }

    public virtual void OnInit() { }

    public virtual void OnDespawn() { }

    protected abstract void HandleMovement();

    protected float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void ChangeColor(ColorType colorType)
    {
        color = colorType;
        if (meshRenderer == null && colorData == null) return;
        meshRenderer.material = colorData.GetMat(colorType);
    }

    public void ChangeAnim(string animName)
    {
        if (string.IsNullOrEmpty(animName) || animator == null) return;

        if (this.animName != animName)
        {
            if (!string.IsNullOrEmpty(this.animName))
            {
                animator.ResetTrigger(this.animName);
            }
            this.animName = animName;
            animator.SetTrigger(this.animName);
        }
    }

    public void AddBrick()
    {
        if (brickPrefab == null || brickHolder == null) return;

        GameObject newBrick = Instantiate(brickPrefab, brickHolder);
        newBrick.transform.SetLocalPositionAndRotation(new Vector3(0, bricks.Count * 0.25f, 0), Quaternion.Euler(0, 90, 0));

        if (newBrick.TryGetComponent<Brick>(out var brickComponent))
        {
            brickComponent.ChangeColor(color);
            bricks.Add(newBrick);
        }
    }

    public int GetBrickCount() => bricks.Count;

    public void RemoveBrick()
    {
        if (bricks.Count > 0)
        {
            GameObject playerBrick = bricks[^1];
            bricks.RemoveAt(bricks.Count - 1);
            Destroy(playerBrick);
        }
    }

    public void ClearBricks()
    {
        foreach (Transform brick in brickHolder)
        {
            Destroy(brick.gameObject);
        }
        bricks.Clear();
    }

    public bool CanMove(Vector3 nextPoint)
    {
        bool canMove = true;
        if (Physics.Raycast(nextPoint + Vector3.up * 0.35f, Vector3.down, out RaycastHit hit, 1f, validLayerMask))
        {
            Stair stair = hit.collider.GetComponent<Stair>();
            if (stair != null && stair.stairColor != color && GetBrickCount() > 0)
            {
                stair.ChangeStairColor(color);
                RemoveBrick();
                stair.ActivateStair(this);
            }

            if (stair != null && stair.stairColor != color && GetBrickCount() == 0)
            {
                ChangeAnim(Constants.ANIM_IDLE);
                canMove = false;
            }
        }
        return canMove;
    }

    private bool IsOnValidSurface()
    {
        Ray ray = new(TF.position + Vector3.up * 0.35f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1f, validLayerMask))
        {
            if (cachedMeshRenderer == null || cachedMeshRenderer.gameObject != hitInfo.collider.gameObject)
            {
                cachedMeshRenderer = hitInfo.collider.GetComponent<MeshRenderer>();
            }

            return cachedMeshRenderer != null && cachedMeshRenderer.enabled;
        }
        return false;
    }

    private bool HasMoved()
    {
        float distanceMoved = (TF.position - lastPosition).sqrMagnitude;
        if (distanceMoved > 0.001f)
        {
            lastPosition = TF.position;
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_BRICK))
        {
            Brick brick = other.GetComponent<Brick>();
            if (brick != null && (brick.ColorType() == color || brick.ColorType() == ColorType.Gray))
            {
                AddBrick();
                brick.BrickDespawn();
                StartCoroutine(BricksReappeared(brick));
            }
        }
    }

    private IEnumerator BricksReappeared(Brick brick)
    {
        yield return new WaitForSeconds(10f);
        brick.BrickSpawn();
    }
}