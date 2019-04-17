using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Equipment equipment;
    public Image icon;
    public Text equipmentName;
    
    public void AddEquipment (Equipment newItem)
    {
        equipment = newItem;
        equipmentName.text = equipment.equipSlot.ToString();
        if(!newItem.isDefaultItem) {    
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
    }

    public void Unequip()
    {
        if (equipment != null)
        {
            EquipmentManager.instance.Unequip((int)equipment.equipSlot);
        }
    }
}