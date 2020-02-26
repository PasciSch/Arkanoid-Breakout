﻿using UnityEngine;
using System.Linq;

public class LightningBall : Collectable
{
    protected override void ApplyEffect()
    {
        foreach (var ball in BallsManager.Instance.Balls.ToList())
        {
            ball.StartLightningBall();
        }
    }
}
