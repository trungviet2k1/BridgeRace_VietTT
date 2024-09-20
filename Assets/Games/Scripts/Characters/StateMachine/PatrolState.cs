using UnityEngine;

public class PatrolState : IState<Bot>
{
    private int targetBrick;

    public void OnEnter(Bot bot)
    {
        targetBrick = Random.Range(7, 10);
        bot.SeekBrick();
    }

    public void OnExecute(Bot bot)
    {
        bot.ChangeAnim(Constants.ANIM_RUN);

        if (bot.GetBrickCount() >= targetBrick)
        {
            bot.ChangeState(new BuildState());
        }
        else
        {
            if (bot.brickPositions.Count > 0)
            {
                bot.BotMovement();
            }
            else
            {
                bot.SeekBrick();
            }
        }
    }

    public void OnExit(Bot bot) { }
}