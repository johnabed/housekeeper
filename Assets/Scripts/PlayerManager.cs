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

    //todo: note that player respawns with no clothing... like the EquipDefault is not running on scene load?
    public void KillPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
