using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    //Creating Singleton of Inventory object (as only 1 would persist in game)
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }
        instance = this;
    }
    #endregion

    #region Delegate
    //used to notify changes in inventory from add / remove
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    #endregion

    public int space = 20; //inventory space

    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        if(!item.isDefaultItem) 
        { 
            if(items.Count >= space)
            {
                Debug.Log("Not enough room.");
                return false;
            }
            items.Add(item);

            //delegate call
            if (onItemChangedCallback != null) 
            { 
                onItemChangedCallback.Invoke();
            }

        }
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);

        //delegate call
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }
}
