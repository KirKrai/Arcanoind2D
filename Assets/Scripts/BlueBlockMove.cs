using UnityEngine;

public class BlueBlockMove : MonoBehaviour
{
    public float speedVec;

    void Start()
    {
        // Случайная скорость
        speedVec = Random.Range(2, 6);
        var random = new System.Random();
        var dir = new[] { -1, 1 };
        // Случайное направление движения (вперёд или назад)
        speedVec *= dir[random.Next(0, dir.Length)];
    }

    void Update()
    {
        // Движение блока
        transform.Translate(speedVec * Time.deltaTime, 0, 0);
        // Если блок приблизлися к границам игрового поля, то меняем направление движения
        if (transform.position.x > 5.5f || transform.position.x < -5.5f)
        {
            // Меняем направление лишь один раз (чтобы блок не дёргался на месте)
            speedVec = -Mathf.Sign(transform.position.x) * Mathf.Abs(speedVec);
        }
    }
}