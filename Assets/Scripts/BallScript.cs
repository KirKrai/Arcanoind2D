using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector2 ballInitialForce;
    private Rigidbody2D rb;
    private GameObject playerObj;
    private float deltaX;
    private AudioSource audioSrc;
    public AudioClip hitSound;
    public AudioClip loseSound;

    public GameDataScript gameData;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
        deltaX = transform.position.x;
        audioSrc = Camera.main.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (rb.isKinematic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                rb.isKinematic = false;
                rb.AddForce(ballInitialForce);
            }
            else
            {
                var transform1 = transform;
                var pos = transform1.position;
                pos.x = playerObj.transform.position.x + deltaX;
                transform1.position = pos;
            }
        }

        if (rb.isKinematic || !Input.GetKeyDown(KeyCode.J)) return;
        var v = rb.linearVelocity;
        if (Random.Range(0, 2) == 0)
        {
            v.Set(v.x - 0.1f, v.y + 0.1f);
        }
        else
        {
            v.Set(v.x + 0.1f, v.y - 0.1f);
        }

        rb.linearVelocity = v;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(hitSound, gameData.soundVolume * (1 / audioSrc.volume));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Wall")) return;
        if (gameData.sound)
        {
            audioSrc.PlayOneShot(loseSound, gameData.soundVolume * (1 / audioSrc.volume));
        }

        Destroy(gameObject);
        playerObj.GetComponent<PlayerScript>().BallDestroyed();
    }
}