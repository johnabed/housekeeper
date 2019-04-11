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

    Equipment[] currentEquipment;
    GameObject[] currentSockets;
    public GameObject targetSocket;
    public GameObject equipmentSocket; 

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length; //string array of the elements inside the Enum
        currentEquipment = new Equipment[numSlots];
        currentSockets = new GameObject[numSlots];
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

        Equipment oldItem = null;
        if(currentEquipment[slotIndex] != null )
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if(onEquipmentChangedCallback != null)
        {
            onEquipmentChangedCallback.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;

        GameObject newSocket = Instantiate<GameObject>(equipmentSocket, targetSocket.transform);
        newSocket.GetComponent<GearSocketController>().Equip(newItem.animationClips);
        currentSockets[slotIndex] = newSocket;
    }

    public void Unequip (int slotIndex)
    {
        if(currentSockets[slotIndex] != null)
        {
            Destroy(currentSockets[slotIndex].gameObject);
        }

        if(currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChangedCallback != null)
            {
                onEquipmentChangedCallback.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll ()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }

}
