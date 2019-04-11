using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSocketController : MonoBehaviour
{
    public Animator MyAnimator { get; set; }

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    //NOTES FOR SELF:
    //MyAnimator is creating an Override Controller so the GearSocketAnimator is not needed
    //I think I can safely delete the GearSocketAnimator and default to Null.
    //I should use the EquipmentManager delegate to listen for changes
    private void Awake()
    {
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
    }

    public void Equip(AnimationClip[] animations)
    {
        animatorOverrideController["Player_Idle_Down"] = animations[0];
        animatorOverrideController["Player_Idle_Left"] = animations[1];
        animatorOverrideController["Player_Idle_Right"] = animations[2];
        animatorOverrideController["Player_Idle_Up"] = animations[3];

        animatorOverrideController["Player_Move_Down"] = animations[4];
        animatorOverrideController["Player_Move_Left"] = animations[5];
        animatorOverrideController["Player_Move_Right"] = animations[6];
        animatorOverrideController["Player_Move_Up"] = animations[7];
    }
}
