using FPS.Enemy;
using System.Collections;
using UnityEngine;
using static SnakeBoss;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private SnakeBoss snakeBoss;
    [SerializeField] private SnakeBossHealth snakeBossHealth;
    [SerializeField] private BossRaycastHit bossRaycastHit;
    [SerializeField] private NpcMovement[] npcMovements;
    [SerializeField] private WinGame winGame;

    [ContextMenu(" -= Set Boss To Patrol =-")]
    public void SetBossToPatrol()
    {
        snakeBoss.SetMove(true);
        bossRaycastHit.SetUseRaycast(false);
        snakeBoss.ChangeBossState(BossState.Patrol, 1);
    }

    [ContextMenu(" -= Start Boss Fight =-")]
    public void StartBossFight()
    {
        bossRaycastHit.SetUseRaycast(true);
        snakeBoss.ChangeBossState(BossState.Attack, 1);
        snakeBossHealth.ShowAndSetupBossHealthSlider();
        SetNpcHide();
    }

    [ContextMenu(" -= End Boss Fight =-")]
    public void EndBossFight()
    {
        snakeBoss.SetMove(false);
        bossRaycastHit.SetUseRaycast(false);
        snakeBoss.ChangeBossState(BossState.Idle, 1);
        snakeBossHealth.HideBossHealthSlider();
        SetNpcStandUp();
        StartCoroutine(ActivateWinGamePanel());
    }

    private IEnumerator ActivateWinGamePanel()
    {
        yield return new WaitForSeconds(5f);
        winGame.ActivateWinGamePanel();
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
