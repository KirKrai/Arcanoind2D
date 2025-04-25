using UnityEngine;

public class Norm : BonusBase
{
    PlayerScript playerObj;

    public override void BonusActivate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerObj.force = 1;

        var balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (var ball in balls)
        {
            var BallSprite = ball.GetComponent<SpriteRenderer>();
            BallSprite.color = Color.white;
        }
    }
}
