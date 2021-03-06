﻿using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject equipmentUI;
    
    EquipmentManager equipmentManager;

    EquipmentSlot[] slots;

    void Awake()
    {
        equipmentManager = EquipmentManager.instance;
        equipmentManager.onEquipmentChangedCallback += UpdateUI; //causes function to trigger whenever item is added/removed

        slots = itemsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Equipment"))
        {
            equipmentUI.SetActive(!equipmentUI.activeSelf);
        }
    }

    void UpdateUI (Equipment newItem, Equipment oldItem)
    {
        if(newItem != null) 
        {
            slots[(int)newItem.equipSlot].AddEquipment(newItem);
        }

        if (oldItem != null)
        {
            slots[(int)oldItem.equipSlot].ClearSlot();
        }
    }
}