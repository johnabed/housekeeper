using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    //Creating Singleton of EquipmentManager object (as only 1 would persist in game)
    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found");
            return;
        }
        instance = this;
    }
    #endregion

    #region Delegate
    //used to notify changes in inventory from add / remove
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChangedCallback;
    #endregion

    public Equipment[] defaultEquipment; //What is initially worn by the player
    Equipment[] currentEquipment; //equipment worn by player
    GameObject[] currentSockets; //reference to gameobjects holding equipment worn by player
    public GameObject targetSocket; //parent object (i.e. Player Graphics holding Anim & SpriteRenderer)

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; //string array of the elements inside the Enum
        currentEquipment = new Equipment[numSlots];
        currentSockets = new GameObject[numSlots];

        EquipDefaultItems();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public void Equip (Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot; //get the index of the enum (i.e. Chest=1)

        Equipment oldItem = Unequip(slotIndex); //removes any items currently equipped in this slot

        if (onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke(newItem, null); //todo: make sure changing oldItem to null works
        }

        currentEquipment[slotIndex] = newItem;

        //create new EquipmentSocket prefab gameobject in scene as child of Player Graphic
        GameObject newSocket = Instantiate<GameObject>(newItem.equipmentSocket, targetSocket.transform);
        newSocket.GetComponent<EquipmentSocketController>().Equip(newItem.animationClips);
        currentSockets[slotIndex] = newSocket;
    }

    public Equipment Unequip (int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            if (currentSockets[slotIndex] != null)
            {
                Destroy(currentSockets[slotIndex].gameObject);
            }

            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChangedCallback != null)
            {
                onEquipmentChangedCallback.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll ()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }

        EquipDefaultItems();
    }

    public void EquipDefaultItems ()
    {
        foreach (Equipment item in defaultEquipment)
        {
            Equip(item);
        }
    }

}
