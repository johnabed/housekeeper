using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocket : MonoBehaviour
{

    public Animator MyAnimator { get; set; }

    private SpriteRenderer spriteRenderer;

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private bool playerMoving;
    private Vector2 lastMove;

    [SerializeField]
    private AnimationClip[] animationClips;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentAnimator = GetComponentInParent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    private void Update()
    {
        playerMoving = false;

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            playerMoving = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            playerMoving = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }

        if(Input.GetKeyDown("space"))
        {
            print("space key pressed");
            Equip(animationClips);
        }

        if (Input.GetKeyDown("x"))
        {
            print("x key pressed");
            Unequip();
        }

        MyAnimator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        MyAnimator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        MyAnimator.SetBool("PlayerMoving", playerMoving);
        MyAnimator.SetFloat("LastMoveX", lastMove.x);
        MyAnimator.SetFloat("LastMoveY", lastMove.y);
    }

    public void Equip(AnimationClip[] animations)
    {
        spriteRenderer.color = Color.white;

        animatorOverrideController["Player_Idle_Down"] = animations[0];
        animatorOverrideController["Player_Idle_Left"] = animations[1];
        animatorOverrideController["Player_Idle_Right"] = animations[2];
        animatorOverrideController["Player_Idle_Up"] = animations[3];

        animatorOverrideController["Player_Move_Down"] = animations[4];
        animatorOverrideController["Player_Move_Left"] = animations[5];
        animatorOverrideController["Player_Move_Right"] = animations[6];
        animatorOverrideController["Player_Move_Up"] = animations[7];
    }

    public void Unequip()
    {
        animatorOverrideController["Player_Idle_Down"] = null;
        animatorOverrideController["Player_Idle_Left"] = null;
        animatorOverrideController["Player_Idle_Right"] = null;
        animatorOverrideController["Player_Idle_Up"] = null;

        animatorOverrideController["Player_Move_Down"] = null;
        animatorOverrideController["Player_Move_Left"] = null;
        animatorOverrideController["Player_Move_Right"] = null;
        animatorOverrideController["Player_Move_Up"] = null;

        Color c = spriteRenderer.color;
        c.a = 0;
        spriteRenderer.color = c;
    }
}
