using Scriptable;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Animator anim;

    [Header("Skins")]
    [SerializeField] protected ColorData colorData;
    [SerializeField] protected Renderer meshRenderer;
    public ColorType color;

    private string animName;

    private void Start()
    {
        OnInit();
        ChangeAnim(Constants.ANIM_IDLE);
    }

    private void Update()
    {
        HandleMovement();
    }

    public virtual void OnInit() { }

    protected abstract void HandleMovement();

    protected abstract void AddBrick();

    protected abstract void RemoveBrick();

    protected abstract void ClearBricks();

    protected float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void ChangeColor(ColorType colorType)
    {
        color = colorType;
        meshRenderer.material = colorData.GetMat(colorType);
    }

    public void ChangeAnim(string animName)
    {
        if (string.IsNullOrEmpty(animName)) return;

        if (this.animName != animName)
        {
            if (!string.IsNullOrEmpty(this.animName))
            {
                anim.ResetTrigger(this.animName);
            }
            this.animName = animName;
            anim.SetTrigger(this.animName);
        }
    }
}