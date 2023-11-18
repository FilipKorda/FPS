using UnityEngine;

[System.Serializable]
public class BodyPart
{
    public string name;
    public int maxHealth;
    public int currentHealth;

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Mo¿esz dodaæ tu kod obs³uguj¹cy obra¿enia dla konkretnej czêœci cia³a

        if (currentHealth <= 0)
        {
            Debug.Log("!");
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
