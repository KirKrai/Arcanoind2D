using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    //источник звука
    private static AudioSource audioSrc; 

    // кнопка и слайдер для звуков
    [SerializeField] Toggle soundToggle;
    [SerializeField] Slider soundSlider;

    // кнопка и слайдер для музыки
    [SerializeField] Toggle musicToggle;
    [SerializeField] Slider musicSlider;

    // файл с информации игры
    public GameDataScript gameData;

    //меню паузы
    public GameObject menuPause;

   // public GameObject deadMenu;

    void Start()
    {
        //подхватываем значения источника звука, информацию из gamedata для значение звуков и музыки
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


    //Кнопка Play, запускает игру (save сохраняет измененные значение звука)
    public void Play()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Кнопка Quit, сохраняет игру и запускает главное меню
    public void Quit()
    {
        gameData.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }
    //Кнопка Exit, выход из игры
    public void Exit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //Кнопка Restart, сбрасывает и сохраняет значение на сцене и перезаускает уровень
    public void Restart()
    {
        gameData.Reset();
        gameData.Save();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //Кнопка продолжить, выключает активное меню паузы, отключает курсор и отключает паузу
    public void Resume()
    {
        menuPause.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        // GameIsPaused = false;
    }
    //Ставит игру на паузу 
    public void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0f;
       // GameIsPaused = true;
    }

   
    // подхватываем значение громкости звука для слайдера звуков
    public void SetSoundVolume()
    {
        gameData.soundVolume = (int)soundSlider.value;
    }

    // подхватываем значение громкости музыки  для слайдера музыки
    public void SetMusicVolume()
    {
        audioSrc.volume = musicSlider.value;
        gameData.musicVolume = audioSrc.volume;
    }


    //Проверяем значение переключателя музыки и соотвестненно отключаем или включаем  музыки
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


    //Проверяем значение переключателя звуков и соотвестненно отключаем или включаем звуки
    public void SetSoundToggle()
    {
        if (soundToggle.isOn)
            gameData.sound = true;
        else
            gameData.sound = false;
    }


}