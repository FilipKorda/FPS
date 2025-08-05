using FPS.Enemy;
using UnityEngine;
using static SnakeBoss;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private SnakeBoss snakeBoss;
    [SerializeField] private SnakeBossHealth snakeBossHealth;
    [SerializeField] private BossRaycastHit bossRaycastHit;
    [SerializeField] private NpcMovement[] npcMovements;

    [ContextMenu(" -= Start Boss Fight =-")]
    public void StartBossFight()
    {
        snakeBoss.SetMove(true);
        bossRaycastHit.SetUseRaycast(true);
        snakeBoss.ChangeBossState(BossState.Attack, 1);
        snakeBossHealth.ShowAndSetupBossHealthSlider();
        SetNpcHide();
    }

    public void EndBossFight()
    {
        snakeBoss.SetMove(false);
        bossRaycastHit.SetUseRaycast(false); 
        snakeBoss.ChangeBossState(BossState.Idle, 1);
        snakeBossHealth.HideBossHealthSlider();
        SetNpcStandUp();
    }

    private void SetNpcHide()
    {
        foreach (var npc in npcMovements)
        {
            npc.HideNpcWhenBossEntrance();
        }
    }

    private void SetNpcStandUp()
    {
        foreach (var npc in npcMovements)
        {
            npc.StandUpNpcAfterBossDead();
        }
    }



}
