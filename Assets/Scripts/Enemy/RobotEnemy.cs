using UnityEngine;

namespace FPS.Enemy
{
    public class RobotEnemy : MainEnemyBehaviour
    {
        [SerializeField] private ParticleSystem armLeftPS;
        [SerializeField] private ParticleSystem armRightPS;
        [SerializeField] private ParticleSystem legLeftPS;
        [SerializeField] private ParticleSystem legRightPS;



        public void ActiveFlyParticels()
        {
            legLeftPS.gameObject.SetActive(true);
            legRightPS.gameObject.SetActive(true);
        }

        public void DisableFlyParticels()
        {
            legLeftPS.gameObject.SetActive(false);
            legRightPS.gameObject.SetActive(false);
        }


        public void ActiveAttackParticels()
        {
            armLeftPS.gameObject.SetActive(true);
            armRightPS.gameObject.SetActive(true);
        }

        public void DisableAttackParticels()
        {
            armLeftPS.gameObject.SetActive(false);
            armRightPS.gameObject.SetActive(false);
        }
    }
}