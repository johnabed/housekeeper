using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public override void Die()
    {
        base.Die();

        //Handle animation

        //Destory enemy
        StartCoroutine(EnemyDeath());
    }
    
    IEnumerator EnemyDeath()
    {
        gameObject.GetComponent<EnemyController>().enabled = false;
        Animator anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveY", 0f);
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsAttacking", false);
        anim.SetFloat("LastMoveX", 0f);
        anim.SetFloat("LastMoveY", 0f);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("IsDying", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
