public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot bots)
    {
        bots.SeekBrick();
    }

    public void OnExecute(Bot bots)
    {
        bots.ChangeAnim(Constants.ANIM_RUN);
        if (bots.GetBrickCount() >= 10)
        {
            bots.ChangeState(new BuildState());
        }
        else
        {
            bots.MoveToNextBrick();
        }
    }

    public void OnExit(Bot bots) { }
}