using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScoreArena : MonoBehaviour
{
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var ball = other.gameObject.GetComponent<Ball>();
        if (ball && gameManager)
        {
            gameManager.ScoreBall(ball);
        }
    }
}
