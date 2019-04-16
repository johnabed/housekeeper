using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    //Creating Singleton of PlayerManager object (as only 1 would persist in game)
    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of PlayerManager found");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject player;

    public void KillPlayer()
    {
        StartCoroutine(PlayerDeath());
    }
    
    IEnumerator PlayerDeath()
    {   
        player.GetComponent<EnemyController>().enabled = false;
        Animator anim = player.GetComponentInChildren<Animator>();
        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveY", 0f);
        anim.SetBool("IsMoving", false);
        anim.SetBool("IsAttacking", false);
        anim.SetFloat("LastMoveX", 0f);
        anim.SetFloat("LastMoveY", 0f);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("IsDying", true);
        yield return new WaitForSeconds(1f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
