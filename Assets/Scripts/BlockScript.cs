using UnityEngine;
using UnityEngine.UI;

// using TMPro;

public class BlockScript : MonoBehaviour
{
    private PlayerScript playerScript;

    public GameObject textObject;
    private Text textComponent;
    public int hitsToDestroy;
    public int points;

    void Start()
    {
        if (textObject != null)
        {
            textComponent = textObject.GetComponent<Text>();
            textComponent.text = hitsToDestroy.ToString();
        }

        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitsToDestroy -= playerScript.force;
        if (hitsToDestroy <= 0)
        {
            Destroy(gameObject);
            var o = gameObject;
            playerScript.BlockDestroyed(points, o.name, o.transform.position);
        }
        else if (textComponent != null)
        {
            textComponent.text = hitsToDestroy.ToString();
        }
    }
}