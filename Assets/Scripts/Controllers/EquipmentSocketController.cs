using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSocketController : MonoBehaviour
{
    public EquipmentSlot equipSlot;

    public Animator MyAnimator { get; set; }

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;
    private List<KeyValuePair<AnimationClip, AnimationClip>> overrides;

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
        if (newItem != null && newItem.equipSlot == equipSlot 
                            && newItem.animationClips.Length == animatorOverrideController.overridesCount)
        {
            overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(overrides);
            for (int i = 0; i < overrides.Count; ++i) {
                Debug.Log(overrides[i].Key.name + " = " + newItem.animationClips[i].name);
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, newItem.animationClips[i]);
            }
            animatorOverrideController.ApplyOverrides(overrides);
        }
    }
}
