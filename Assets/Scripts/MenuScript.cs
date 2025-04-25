using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    //�������� �����
    private static AudioSource audioSrc; 

    // ������ � ������� ��� ������
    [SerializeField] Toggle soundToggle;
    [SerializeField] Slider soundSlider;

    // ������ � ������� ��� ������
    [SerializeField] Toggle musicToggle;
    [SerializeField] Slider musicSlider;

    // ���� � ���������� ����
    public GameDataScript gameData;

    //���� �����
    public GameObject menuPause;

   // public GameObject deadMenu;

    void Start()
    {
        //������������ �������� ��������� �����, ���������� �� gamedata ��� �������� ������ � ������
        audioSrc = Camera.main.GetComponent<AudioSource>();
        if (gameData.resetOnStart)
        {
            gameData.Load();
        }

        soundToggle.isOn = gameData.sound;
        soundSlider.value = gameData.soundVolume;
        musicToggle.isOn = gameData.music;
        musicSlider.value = gameData.musicVolume;
    }


    //������ Play, ��������� ���� (save ��������� ���������� �������� �����)
    public void Play()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //������ Quit, ��������� ���� � ��������� ������� ����
    public void Quit()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
    //������ Exit, ����� �� ����
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //������ Restart, ���������� � ��������� �������� �� ����� � ������������ �������
    public void Restart()
    {
        gameData.Reset();
        gameData.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //������ ����������, ��������� �������� ���� �����, ��������� ������ � ��������� �����
    public void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        // GameIsPaused = false;
    }
    //������ ���� �� ����� 
    public void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
       // GameIsPaused = true;
    }

   
    // ������������ �������� ��������� ����� ��� �������� ������
    public void SetSoundVolume()
    {
        gameData.soundVolume = (int)soundSlider.value;
    }

    // ������������ �������� ��������� ������  ��� �������� ������
    public void SetMusicVolume()
    {
        audioSrc.volume = musicSlider.value;
        gameData.musicVolume = audioSrc.volume;
    }


    //��������� �������� ������������� ������ � ������������� ��������� ��� ��������  ������
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


    //��������� �������� ������������� ������ � ������������� ��������� ��� �������� �����
    public void SetSoundToggle()
    {
        if (soundToggle.isOn)
            gameData.sound = true;
        else
            gameData.sound = false;
    }


}