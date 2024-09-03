﻿using UnityEngine;

public class PatrolState : IState<Bot>
{
    private int targetBrick;

    public void OnEnter(Bot bot)
    {
        targetBrick = Random.Range(3, 8);
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
                bot.MoveToBrickPosition();
            }
            else
            {
                bot.SeekBrick();
            }
        }
    }

    public void OnExit(Bot bot) { }
}