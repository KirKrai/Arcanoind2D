using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    const int maxLevel = 30;
    [Range(1, maxLevel)] public int level = 1;
    public int force = 1;
    public float ballVelocityMult = 0.02f;
    public GameObject bluePrefab;
    public GameObject redPrefab;
    public GameObject greenPrefab;
    public GameObject yellowPrefab;
    public GameObject ballPrefab;
    public GameObject bonusPrefab;


    static Collider2D[] colliders = new Collider2D[50];
    static ContactFilter2D contactFilter;

    public GameDataScript gameData;

    static bool gameStarted;

    public static bool gameFinished;

    private static AudioSource audioSrc;
    public AudioClip pointSound;

    private static bool GameIsPaused;
    //������������� ������ � �����
    [SerializeField] Toggle soundToggle;
    [SerializeField] Toggle musicToggle;

    //���� ����� � ����� ����
    public GameObject pauseMenuUI;
    public GameObject deadMenu;

    //���� ��� ��������� � ������������ ������ � ���������� ��� �������
    public Text textHint;


    static void CreateBlocks(GameObject prefab, float xMax, float yMax, int count, int maxCount)
    {
        if (count > maxCount)
            count = maxCount;
        for (var i = 0; i < count; i++)
        for (var k = 0; k < 20; k++)
        {
            var obj = Instantiate(prefab,
                new Vector3((Random.value * 2 - 1) * xMax,
                    Random.value * yMax, 0),
                Quaternion.identity);
            if (obj.GetComponent<Collider2D>()
                    .Overlap(contactFilter.NoFilter(), colliders) == 0)
                break;
            Destroy(obj);
        }
    }

    private void CreateBalls()
    {
        var count = 2;
        if (gameData.balls == 1)
            count = 1;

        for (var i = 0; i < count; i++)
        {
            var obj = Instantiate(ballPrefab);
            var ball = obj.GetComponent<BallScript>();
            ball.ballInitialForce += new Vector2(10 * i, 0);
            ball.ballInitialForce *= 1 + level * ballVelocityMult;
        }
    }

    void SetBackground()
    {
        var bg = GameObject.Find("Background").GetComponent<SpriteRenderer>();
        bg.sprite = Resources.Load(level.ToString("d2"),
            typeof(Sprite)) as Sprite;
    }

    private IEnumerator BlockDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Block").Length == 0)
        {
            switch (level)
            {
                case < maxLevel:
                    gameData.level++;
                    gameData.Save();
                    SceneManager.LoadScene("MainScene"); // ������� �� ������� �����
                    break;
                case maxLevel:
                    //������� ��� ���� ��������, ��������� ����� ����� ���� � ������ ������� ������
                    gameFinished = true;
                    gameData.Save();
                    deadMenu.SetActive(true);
                    Cursor.visible = true;
                    Time.timeScale = 0f;
                    //SceneManager.LoadScene("Menu");
                    break;
            }
        }
    }

    IEnumerator BallDestroyedCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (GameObject.FindGameObjectsWithTag("Ball").Length != 0) yield break;
        if (gameData.balls > 0)
            CreateBalls();
        else
        {
            //���������� � ���������� �������� ���� � �������� ���� ����� ���� � ������ ������� ������
            gameData.Reset();
            gameData.Save();
            deadMenu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    public void BallDestroyed()
    {
        gameData.balls--;
        StartCoroutine(BallDestroyedCoroutine());
    }

    IEnumerator BlockDestroyedCoroutine2()
    {
        for (var i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.2f);
            audioSrc.PlayOneShot(pointSound, gameData.soundVolume * (1 / audioSrc.volume));
        }
    }

    public void BlockDestroyed(int points, string name, Vector3 pos)
    {
        gameData.points += points;
        if (gameData.sound)
            audioSrc.PlayOneShot(pointSound, gameData.soundVolume * (1 / audioSrc.volume));//�������� �������� ��������� ��������
        gameData.pointsToBall += points;
        if (gameData.pointsToBall >= RequiredPointsToBall)
        {
            gameData.balls++;
            gameData.pointsToBall -= RequiredPointsToBall;
            if (gameData.sound)
                StartCoroutine(BlockDestroyedCoroutine2());
        }

        StartCoroutine(BlockDestroyedCoroutine());
        if (name != "Green Block(Clone)") return;
        var probes = GameDataScript.GetProbes();
        CreateBonus(probes, pos);
    }

    private void CreateBonus(int[] probes, Vector3 pos)
    {
        var rand = Random.Range(1, 100);
        var probab = new int[3];
        probab[0] = probes[0];
        for (int i = 1; i < 3; i++)
        {
            probab[i] = probab[i - 1] + probes[i];
        }

        var obj = Instantiate(bonusPrefab, pos, Quaternion.identity);
        if (rand < probab[0])
        {
            obj.AddComponent<Fire>().gameData = gameData;
            obj.GetComponent<Fire>().textObject =
                obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<Fire>().text = "Fire";
            obj.GetComponent<Fire>().bonusColor = Color.red;
            obj.GetComponent<Fire>().textColor = Color.black;
            return;
        }

        if (rand < probab[1])
        {
            obj.AddComponent<Steel>().gameData = gameData;
            obj.GetComponent<Steel>().textObject =
                obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<Steel>().text = "Steel";
            obj.GetComponent<Steel>().bonusColor = Color.gray;
            obj.GetComponent<Steel>().textColor = Color.black;
            return;
        }

        if (rand < probab[2])
        {
            obj.AddComponent<Norm>().gameData = gameData;
            obj.GetComponent<Norm>().textObject =
                obj.transform.Find("Canvas").gameObject.transform.Find("Bonus Text").gameObject;
            obj.GetComponent<Norm>().text = "Norm";
            obj.GetComponent<Norm>().bonusColor = Color.white;
            obj.GetComponent<Norm>().textColor = Color.black;
        }
    }

    //�������� �������� ������
    private void SetMusic()
    {
        audioSrc.volume = gameData.musicVolume;
        if (gameData.music)
        {
            //musicToggle.isOn = true;
            audioSrc.Play();
        }
        else
        {
            //musicToggle.isOn = false;
            audioSrc.Stop();
        }
    }


    void StartLevel()
    {
        force = 1;
        SetBackground();
        var main = Camera.main;
        var orthographicSize = main.orthographicSize;
        var yMax = orthographicSize * 0.5f;
        var xMax = orthographicSize * main.aspect * 0.5f;
        CreateBlocks(bluePrefab, xMax, yMax, level, 8);
        CreateBlocks(redPrefab, xMax, yMax, 1 + level, 10);
        CreateBlocks(greenPrefab, xMax, yMax, 1 + level, 12);
        CreateBlocks(yellowPrefab, xMax, yMax, 2 + level, 15);
        CreateBalls();
    }

    private int RequiredPointsToBall => 400 + (level - 1) * 20;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 1f;
        audioSrc = Camera.main.GetComponent<AudioSource>();
        Cursor.visible = false;
        if (!gameStarted)
        {
            gameStarted = true;
            if (gameData.resetOnStart)
                gameData.Load();
        }


        //musicToggle.isOn = gameData.music;
        SetMusic();

        level = gameData.level;


        StartLevel();
        GameIsPaused = false;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var transform1 = transform;
            var pos = transform1.position;
            pos.x = mousePos.x;
            transform1.position = pos;
        }
        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            gameData.music = !gameData.music;
            musicToggle.isOn = gameData.music;
            SetMusic();
        }

        if (Input.GetKeyDown(KeyCode.S))

            gameData.sound = !gameData.sound;
        soundToggle.isOn = gameData.sound;

        if (Input.GetKeyDown(KeyCode.N))
            Restart();
        */
        //������ Esc ������ ���� �� ����� � �������������� ��������� �����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    /*
    public void Restart()
    {
        gameData.Reset();
        gameData.Save();
        Time.timeScale = 1f;
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    */

    // ��������� �������� ���� �����, ��������� ������ � ��������� �����
    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    //������ ���� �� �����, �������� ���� ����� � ��������� �������
    void Pause()
    {
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }


    void OnApplicationQuit()
    {
        gameData.Save();
    }

    static string OnOff(bool boolVal)
    {
        return boolVal ? "on" : "off";
    }

    void OnGUI()
    {
        //�����, ������� ������� ��������� � ������ ����� � ���������� ���������� �������, ��� ������� � ����
        textHint.text = "Esc-pause"+"  "+"Level:" + gameData.level + "  " + "Balls" + gameData.balls+"  "+"Score:"+ gameData.points;

        /*
        GUI.Label(new Rect(300, 0, Screen.width - 10, 100),
            $"<color=yellow><size=15>Level <b>{gameData.level}</b> Balls <b>{gameData.balls}</b>" +
            $" Score <b>{gameData.points}</b></size></color>");
        */
    }

    /*
    public void SetMusicToggle()
    {
        if (musicToggle.isOn)
        {
            gameData.music = true;
            audioSrc.Play();
        }
        else
        {
            gameData.music = false;
            audioSrc.Stop();
        }
    }


    public void SetSoundToggle()
    {
        if (soundToggle.isOn)
            gameData.sound = true;
        else
            gameData.sound = false;
    }
    */
}