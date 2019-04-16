using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSocketController : MonoBehaviour
{
    public EquipmentSlot equipSlot;

    public Animator MyAnimator { get; set; }

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;

    private void Awake()
    {
        EquipmentManager.instance.onEquipmentChangedCallback += OnEquipmentChanged;
        
        parentAnimator = transform.parent.GetComponent<Animator>();
        MyAnimator = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(MyAnimator.runtimeAnimatorController);
        MyAnimator.runtimeAnimatorController = animatorOverrideController;
    }

    private void Update()
    {
        MyAnimator.SetFloat("LastMoveX", parentAnimator.GetFloat("LastMoveX"));
        MyAnimator.SetFloat("LastMoveY", parentAnimator.GetFloat("LastMoveY"));
        MyAnimator.SetFloat("MoveX", parentAnimator.GetFloat("MoveX"));
        MyAnimator.SetFloat("MoveY", parentAnimator.GetFloat("MoveY"));
        MyAnimator.SetBool("IsMoving", parentAnimator.GetBool("IsMoving"));
        MyAnimator.SetBool("IsAttacking", parentAnimator.GetBool("IsAttacking"));
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null && newItem.equipSlot == equipSlot)
        {
            animatorOverrideController["Player_Idle_Down"] = newItem.animationClips[0];
            animatorOverrideController["Player_Idle_Left"] = newItem.animationClips[1];
            animatorOverrideController["Player_Idle_Right"] = newItem.animationClips[2];
            animatorOverrideController["Player_Idle_Up"] = newItem.animationClips[3];

            animatorOverrideController["Player_Move_Down"] = newItem.animationClips[4];
            animatorOverrideController["Player_Move_Left"] = newItem.animationClips[5];
            animatorOverrideController["Player_Move_Right"] = newItem.animationClips[6];
            animatorOverrideController["Player_Move_Up"] = newItem.animationClips[7];
        }
    }
}
