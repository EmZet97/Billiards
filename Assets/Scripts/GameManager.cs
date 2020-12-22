using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool gameInProgress = false;

    public menu menu;

    private static List<Ball> gameBalls = new List<Ball>();

    private Ball.BallType[] playersBalls = new Ball.BallType[] { Ball.BallType.None, Ball.BallType.None };

    private int playerTurn = -1;
    public Ball.BallType TurnBallType { get
        {
            return playersBalls[playerTurn];
        } 
    }

    private List<List<Ball.BallType>> scoredBallsPerTurn = new List<List<Ball.BallType>>();

    private int turn = 0;

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
        gameBalls = gameBalls.Where(x => x != null).ToList();
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
                        CancelInvoke("ChangeTurn");
                    }
                    return;                    
                }
            }
            if (!changeTeamInvoked)
            {
                changeTeamInvoked = true;
                Invoke("ChangeTurn", 1.0f);
            }
            
        }
    }

    //End of turn
    private void ChangeTurn()
    {
        //If player having turn didn't score - change player
        if(scoredBallsPerTurn.Count == 0 || scoredBallsPerTurn[scoredBallsPerTurn.Count - 1].FindAll(x => x.Equals(TurnBallType)).Count == 0 || scoredBallsPerTurn[scoredBallsPerTurn.Count - 1].FindAll(x => x.Equals(Ball.BallType.White)).Count != 0)
        {
            playerTurn = (playerTurn + 1) % 2;
            //Update UI
            gameCommunicatesText.text = "Turn of player " + (playerTurn + 1);
            playerOneSign.text = playerTurn == 0 ? "X" : "";
            playerTwoSign.text = playerTurn == 1 ? "X" : "";
        }
        else
        {
            gameCommunicatesText.text = "Player " + (playerTurn + 1) + " continue turn";
        }        

        stick.ShowStick();
        changeTeamInvoked = false;

        // Add new turn ball container
        scoredBallsPerTurn.Add(new List<Ball.BallType>());
        turn++;
    }

    private void UpdateScore()
    {
        if (playersBalls[0] == Ball.BallType.None)
            return;

        playerOneScoreText.text = playersBalls[0] == Ball.BallType.Solid ? $"Solid team: {solidTeamPoints} points" : $"Stripe team: {stripeTeamPoints} points";
        playerTwoScoreText.text = playersBalls[0] == Ball.BallType.Stripe ? $"Solid team: {solidTeamPoints} points" : $"Stripe team: {stripeTeamPoints} points";
    }

    private void AssignBallToPlayer(Ball.BallType ballType)
    {
        playersBalls[playerTurn] = ballType;
        playersBalls[(playerTurn + 1)%2] = ballType == Ball.BallType.Solid ? Ball.BallType.Stripe : Ball.BallType.Solid;
    }

    public void ScoreBall(Ball ball)
    {
        //Add scored ball to turn container
        scoredBallsPerTurn[scoredBallsPerTurn.Count - 1].Add(ball.Type);

        switch (ball.Type)
        {
            case Ball.BallType.Black:
                if (playersBalls[playerTurn] == Ball.BallType.Solid)
                {
                    if (solidTeamPoints == 7)
                    {
                        gameInProgress = false;
                        menu.EndGame($"Player {playerTurn + 1}");
                    }
                    gameInProgress = false;
                    menu.EndGame($"Player {(playerTurn + 1) % 2 + 1}");
                }
                else if (playersBalls[playerTurn] == Ball.BallType.Stripe)
                {
                    if (stripeTeamPoints == 7)
                    {
                        gameInProgress = false;
                        menu.EndGame($"Player {playerTurn + 1}");
                    }
                    gameInProgress = false;
                    menu.EndGame($"Player {(playerTurn + 1) % 2 + 1}");
                }
                break;
            case Ball.BallType.White:
                ball.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                ball.gameObject.transform.position = new Vector3(0, 10, 0);
                return;
            case Ball.BallType.Stripe:
                //First score
                if(playersBalls[0] == Ball.BallType.None)
                {
                    AssignBallToPlayer(ball.Type);
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
                if (playersBalls[0] == Ball.BallType.None)
                {
                    AssignBallToPlayer(ball.Type);
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
