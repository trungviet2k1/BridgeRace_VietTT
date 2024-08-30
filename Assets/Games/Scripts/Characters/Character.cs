using Scriptable;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected Animator anim;

    [Header("Skins")]
    [SerializeField] ColorData colorData;
    [SerializeField] Renderer meshRenderer;
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