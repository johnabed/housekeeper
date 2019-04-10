using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f; // how close to interact
    public Transform interactionTransform; //where the player can interact with the Interactable (for chests etc)

    bool isFocus = false;
    Transform player;

    bool hasInteracted = false;

    public virtual void Interact ()
    {
        // This method is meant to be overwritten
        Debug.Log("Interacting with " + transform.name);
    }

    void Update()
    {
        if (isFocus && !hasInteracted)
        {
            float distance = Vector2.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        GetComponent<SpriteRenderer>().color = Color.red; //todo: remove
        isFocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OnDefocused()
    {
        GetComponent<SpriteRenderer>().color = Color.white; //todo: remove
        isFocus = false;
        player = null;
        hasInteracted = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
