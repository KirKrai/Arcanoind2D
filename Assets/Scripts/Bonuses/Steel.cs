using UnityEngine;

public class Steel : BonusBase
{
    PlayerScript playerObj;

    public override void BonusActivate()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerObj.force = 40;

        var balls = GameObject.FindGameObjectsWithTag("Ball");

        foreach (var ball in balls)
        {
            var BallSprite = ball.GetComponent<SpriteRenderer>();
            BallSprite.color = Color.gray;
        }
    }
}
