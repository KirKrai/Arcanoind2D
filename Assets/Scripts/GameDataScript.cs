using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game Data", order = 51)]
public class GameDataScript : ScriptableObject
{
    public bool resetOnStart;
    public int level = 1;
    public int balls = 6;
    public int points;
    public int pointsToBall;
    public bool music = true;
    public bool sound = true;
    public int soundVolume = 0; //начальное значение громкости звуков
    public float musicVolume = 0f;//начальное значение громкости музыки

    public void Reset()
    {
        level = 1;
        balls = 6;
        points = 0;
        pointsToBall = 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("balls", balls);
        PlayerPrefs.SetInt("points", points);
        PlayerPrefs.SetInt("pointsToBall", pointsToBall);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.SetInt("sound", sound ? 1 : 0);
        PlayerPrefs.SetInt("soundVolume", soundVolume); //сохранение громкости звука и музыки
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("level", 1);
        balls = PlayerPrefs.GetInt("balls", 6);
        points = PlayerPrefs.GetInt("points", 0);
        pointsToBall = PlayerPrefs.GetInt("pointsToBall", 0);
        music = PlayerPrefs.GetInt("music", 1) == 1;
        sound = PlayerPrefs.GetInt("sound", 1) == 1;
        soundVolume = PlayerPrefs.GetInt("soundVolume", 1);//загрузка значение громкости музыки и звука
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.2f);
    }

    public static int[] GetProbes()
    {
        int[] probes = { 35, 35, 30 };
        return probes;
    }
}