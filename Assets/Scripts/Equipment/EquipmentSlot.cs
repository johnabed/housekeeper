using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Equipment equipment;
    public EquipmentType slot;
    public Image icon;
    public Text equipmentName;
    public Button equipmentButton;
    
    public void AddEquipment (Equipment newItem)
    {
        equipment = newItem;
        equipmentName.text = slot.ToString();
        if(!newItem.isDefaultItem)
        {
            equipmentButton.interactable = true;
            equipmentName.text = equipment.name;
            icon.sprite = equipment.icon;
            icon.enabled = true;
            icon.preserveAspect = true;
        }
    }

    public void ClearSlot()
    {
        equipment = null;

        icon.sprite = null;
        icon.enabled = false;

        equipmentButton.interactable = false;
    }

    public void Unequip()
    {
        if (equipment != null)
        {
            EquipmentManager.instance.Unequip((int)equipment.equipSlot);
        }
    }
}