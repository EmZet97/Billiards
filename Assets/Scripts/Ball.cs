using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallType
    {
        White, Black, Stripe, Solid, None
    }

    public BallType type;

    public BallType Type { 
        get {
            return type;
        }
    }

    private void Start()
    {
        GameManager.RegisterBall(this);
    }
}
