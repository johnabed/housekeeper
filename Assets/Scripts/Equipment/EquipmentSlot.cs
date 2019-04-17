using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Equipment equipment;
    public Image icon;
    
    public void AddEquipment (Equipment newItem)
    {
        if(!newItem.isDefaultItem) {
            equipment = newItem;

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
}