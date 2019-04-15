using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; //not Stat as we don't plan to add modifiers
    public int currentHealth { get; private set; }
   
    public Stat damage;
    public Stat armor;

    void Awake ()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage (int damage)
    {
        damage -= armor.GetValue(); //mitigate damage with armor protection
        damage = Mathf.Clamp(damage, 0, int.MaxValue); //ensure value is positive

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die in some way
        //This is method is meant to be overridden
        Debug.Log(transform.name + " died.");
    }
}
