using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSocketController : MonoBehaviour
{
    public Animator MyAnimator { get; set; }

    private Animator parentAnimator;

    private AnimatorOverrideController animatorOverrideController;
    private List<KeyValuePair<AnimationClip, AnimationClip>> overrides;

    private void Awake()
    {
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
        MyAnimator.SetBool("IsDying", parentAnimator.GetBool("IsDying"));
    }

    public void AddEquipment(Equipment newItem)
    {
        if (newItem != null && newItem.animationClips.Length > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            
            //convert array of clips into dict
            var animationClipsDict = newItem.animationClips.ToDictionary(item => item.name,
                item => item);
            
            overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(animatorOverrideController.overridesCount);          
            animatorOverrideController.GetOverrides(overrides);
            for (int i = 0; i < overrides.Count; ++i) {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, animationClipsDict[overrides[i].Key.name]);
            }
            animatorOverrideController.ApplyOverrides(overrides);
        }
    }

    public void RemoveEquipment()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}
