using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienEnamy : MonoBehaviour
{
    public List<BodyPart> bodyParts = new List<BodyPart>();

    private void Start()
    {
        foreach (BodyPart bodyPart in bodyParts)
        {
            bodyPart.currentHealth = bodyPart.maxHealth;
        }
    }

    public void TakeDamage(string bodyPartName, int damage)
    {
        BodyPart targetBodyPart = bodyParts.Find(part => part.name == bodyPartName);

        if (targetBodyPart != null)
        {
            targetBodyPart.TakeDamage(damage);
        }
    }
}
