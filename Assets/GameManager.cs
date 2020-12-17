using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public Text playerOneScoreText;
    public Text playerOneSign;
    public Text playerTwoScoreText;
    public Text playerTwoSign;
    public Text gameCommunicatesText;

    public StickController stick;

    private static List<Ball> gameBalls = new List<Ball>();

    private int playerTurn;
    private Ball.BallType playerOneBallType = Ball.BallType.None;

    private int solidTeamPoints = 0;
    private int stripeTeamPoints = 0;

    private bool changeTeamInvoked = false;

    public static void RegisterBall(Ball ball)
    {
        gameBalls.Add(ball);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(stick.mode == StickController.StickModes.Shooted)
        {
            //Wait till all balls stops movement
            foreach (var ball in gameBalls)
            {               
                if(ball.gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 0.001f)
                {
                    if (changeTeamInvoked)
                    {
                        changeTeamInvoked = false;
                        CancelInvoke("ChengeTurn");
                    }
                    return;                    
                }
            }
            if (!changeTeamInvoked)
            {
                changeTeamInvoked = true;
                Invoke("ChengeTurn", 1.0f);
            }
            
        }
    }

    private void ChengeTurn()
    {
        playerTurn = (playerTurn + 1) % 2;
        gameCommunicatesText.text = "Turn of player " + (playerTurn + 1);
        playerOneSign.text = playerTurn == 0 ? "X" : "";
        playerTwoSign.text = playerTurn == 1 ? "X" : "";
        stick.ShowStick();
        changeTeamInvoked = false;
    }

    private void UpdateScore()
    {
        if (playerOneBallType == Ball.BallType.None)
            return;

        playerOneScoreText.text = playerOneBallType == Ball.BallType.Solid ? $"Solid team: {solidTeamPoints} points" : $"Stripe team: {stripeTeamPoints} points";
        playerTwoScoreText.text = playerOneBallType == Ball.BallType.Stripe ? $"Solid team: {solidTeamPoints} points" : $"Stripe team: {stripeTeamPoints} points";
    }

    public void ScoreBall(Ball ball)
    {
        switch (ball.Type)
        {
            case Ball.BallType.Black:
                //end game
                break;
            case Ball.BallType.White:
                ball.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                ball.gameObject.transform.position = new Vector3(0, 10, 0);
                return;
            case Ball.BallType.Stripe:
                //First score
                if(playerOneBallType == Ball.BallType.None)
                {
                    playerOneBallType = playerTurn == 0 ? Ball.BallType.Stripe : Ball.BallType.Solid;
                    gameCommunicatesText.text = "Current player will play stripe balls";
                }
                else
                {
                    gameCommunicatesText.text = "Stripe player scores";
                }
                stripeTeamPoints += 1;
                break;
            case Ball.BallType.Solid:
                //First score
                if (playerOneBallType == Ball.BallType.None)
                {
                    playerOneBallType = playerTurn == 0 ? Ball.BallType.Solid : Ball.BallType.Stripe; 
                    gameCommunicatesText.text = "Solid player will play stripe balls";
                }
                else
                {
                    gameCommunicatesText.text = "Solid player scores";
                }
                solidTeamPoints += 1;
                break;
        }

        UpdateScore();
        gameBalls.Remove(ball);
        Destroy(ball.gameObject);
    }
}
