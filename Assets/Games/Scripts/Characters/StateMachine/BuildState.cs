using UnityEngine;

public class BuildState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.navMeshAgent.SetDestination(LevelManagement.Ins.currentLevel.GetFinishPoint());
    }

    public void OnExecute(Bot bot)
    {
        if (Physics.Raycast(bot.TF.position + Vector3.up * 0.35f, Vector3.down, out RaycastHit hit, 1f, bot.validLayerMask))
        {
            Stair stair = hit.collider.GetComponent<Stair>();

            if (stair != null && !stair.isActive && stair.stairColor != bot.color && bot.GetBrickCount() > 0)
            {
                stair.ChangeStairColor(bot.color);
                bot.RemoveBrick();
                stair.ActivateStair(bot);
            }

            if (stair != null && !stair.isActive && stair.stairColor != bot.color && bot.GetBrickCount() == 0)
            {
                bot.StopMove();
                bot.ChangeState(new PatrolState());
            }
        }
    }

    public void OnExit(Bot bot) { }
}