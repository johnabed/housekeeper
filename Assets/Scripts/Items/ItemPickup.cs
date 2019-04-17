using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public Texture2D cursorInteract;
    
    public override void Interact()
    {
        base.Interact();

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Pickup();
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.name);
        //Add to inventory
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorInteract, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
