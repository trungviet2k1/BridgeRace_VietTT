using System.Drawing;
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
            if (stair == null || stair.stairColor == bot.color || stair.parentBridge.IsBridgeComplete()) return;

            if (!stair.parentBridge.IsBridgeComplete())
            {
                if (bot.GetBrickCount() > 0)
                {
                    stair.ActivateStair(bot);
                    stair.ChangeStairColor(bot.color);
                    bot.RemoveBrick();
                }
                else
                {
                    bot.StopMove();
                    bot.ChangeState(new PatrolState());
                }
            }
            else
            {
                bot.ChangeState(new PatrolState());
            }
        }
    }

    public void OnExit(Bot bot) { }
}