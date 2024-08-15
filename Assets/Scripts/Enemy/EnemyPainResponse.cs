using UnityEngine;

namespace FPS.Enemy
{
    [DisallowMultipleComponent]
    public class EnemyPainResponse : MonoBehaviour
    {
        [SerializeField] private GameObject wholeEnemy;
        [SerializeField] private PartHealth Health;

        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathArmLeft;
        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathArmRight;
        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathBodyPart;
        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathHeadPart;
        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathLegLeft;
        [SerializeField] private DoAfterEnemyDeath doAfterEnemyDeathLegRight;

        [SerializeField] private PartHealth armLeft;
        [SerializeField] private PartHealth armRight;
        [SerializeField] private PartHealth bodyPart;
        [SerializeField] private PartHealth headPart;
        [SerializeField] private PartHealth legLeft;
        [SerializeField] private PartHealth legRight;


        public void HandlePain(int Damage)
        {
            if (Health.CurrentHealth != 0)
            {
                Debug.Log("Enemy Get Hit: " + gameObject.name + "" + Damage);
            }
        }

        public void HandleDeath()
        {
            Debug.Log("Destroy");
            gameObject.SetActive(false);
        }

        public void HandleAllPartDeath()
        {
            if (wholeEnemy != null)
            {
                if (armLeft.isActiveAndEnabled)
                {
                    doAfterEnemyDeathArmLeft.SpawnBodyPart(transform.position);
                }
                if (armRight.isActiveAndEnabled)
                {
                    doAfterEnemyDeathArmRight.SpawnBodyPart(transform.position);
                }               
                if (bodyPart.isActiveAndEnabled)
                {
                    doAfterEnemyDeathBodyPart.SpawnBodyPart(transform.position);
                }
                if (headPart.isActiveAndEnabled)
                {
                    doAfterEnemyDeathHeadPart.SpawnBodyPart(transform.position);
                }
                if (legLeft.isActiveAndEnabled)
                {
                    doAfterEnemyDeathLegLeft.SpawnBodyPart(transform.position);
                }
                if (legRight.isActiveAndEnabled)
                {
                    doAfterEnemyDeathLegRight.SpawnBodyPart(transform.position);
                }

                Destroy(wholeEnemy);
            }
        }

    }
}