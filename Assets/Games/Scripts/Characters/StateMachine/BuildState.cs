using UnityEngine;

public class BuildState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.MoveToFinishPoint();
    }

    public void OnExecute(Bot bot)
    {
        if (bot.navMeshAgent.pathPending || bot.navMeshAgent.remainingDistance > bot.navMeshAgent.stoppingDistance) return;
        if (bot.GetBrickCount() == 0)
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot) { }
}