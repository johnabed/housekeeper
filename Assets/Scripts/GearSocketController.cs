using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSocketController : MonoBehaviour
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
        parentAnimator = transform.parent.GetComponent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    private void Update()
    {
        MyAnimator.SetFloat("MoveX", parentAnimator.GetFloat("MoveX"));
        MyAnimator.SetFloat("MoveY", parentAnimator.GetFloat("MoveY"));
        MyAnimator.SetBool("PlayerMoving", parentAnimator.GetBool("PlayerMoving"));
        MyAnimator.SetFloat("LastMoveX", parentAnimator.GetFloat("LastMoveX"));
        MyAnimator.SetFloat("LastMoveY", parentAnimator.GetFloat("LastMoveY"));

        if (Input.GetKeyDown("space"))
        {
            print("space key pressed, should equip");
            Equip(animationClips);
        }

        if (Input.GetKeyDown("x"))
        {
            print("x key pressed, should unequip");
            Unequip();
        }
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
