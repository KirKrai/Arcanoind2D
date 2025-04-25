using UnityEngine;
using UnityEngine.UI;

public class BonusBase : MonoBehaviour
{
    public GameObject textObject;
    Text bonusName;
    public string text;
    public GameDataScript gameData;
    public Color bonusColor;
    public Color textColor;
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = bonusColor;
        if (textObject != null)
        {
            bonusName = textObject.GetComponent<Text>();
            bonusName.color = textColor;
            bonusName.text = text;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BonusActivate();
        }

        if (other.gameObject.name == "Bottom Wall" || other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public virtual void BonusActivate()
    {
        gameData.points += 100;
    }
}