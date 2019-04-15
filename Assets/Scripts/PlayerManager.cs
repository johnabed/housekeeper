using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
