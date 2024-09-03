public class BuildState : IState<Bot>
{
    public void OnEnter(Bot bots)
    {
        //bot.ChangeAnim(Constants.ANIM_RUN);
    }

    public void OnExecute(Bot bots)
    {
        if (bots.GetBrickCount() == 0)
        {
            bots.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bots) { }
}