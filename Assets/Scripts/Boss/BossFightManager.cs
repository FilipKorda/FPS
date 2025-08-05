using FPS.Enemy;
using UnityEngine;
using static SnakeBoss;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private SnakeBoss snakeBoss;
    [SerializeField] private SnakeBossHealth snakeBossHealth;
    [SerializeField] private BossRaycastHit bossRaycastHit;

    [ContextMenu(" -= Start Boss Fight =-")]
    public void StartBossFight()
    {
        snakeBoss.SetMove(true);
        bossRaycastHit.SetUseRaycast(true);
        snakeBoss.ChangeBossState(BossState.Attack, 1);
        snakeBossHealth.ShowAndSetupBossHealthSlider();
    }

}
