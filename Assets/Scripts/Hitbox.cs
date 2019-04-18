using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hitbox : MonoBehaviour
{
    public float radius = 1f; //size of hitbox
    
    public bool WithinRadius (Transform attacker)
    {
        float distance = Vector2.Distance(attacker.position, transform.position);
        return (distance <= radius);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
   
}
