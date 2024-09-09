using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public GameObject bars;
        
    public int hp = 1;
    private int score;
    
    void Start()
    {
        if (hp > 1)
        {
            bars.SetActive(true);
        }

        score = hp * 10;
    }

    void Update()
    {
        
    }

    public void BallHit()
    {
        hp -= 1;
        switch (hp)
        {
            case <= 0:
                GameController.instance.score += score;
                GameController.instance.bricks -= 1;
                Destroy(gameObject);
                break;
            case 1:
                bars.SetActive(false);
                break;
        }
    }
}
