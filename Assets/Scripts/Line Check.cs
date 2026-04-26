using UnityEngine;

public class LineCheck : MonoBehaviour
{
    public float loseTimer = 3f; // Thời gian chờ trước khi xử lý thua
    private float timer = 0f;
    private bool isTouching = false;

    void Update()
    {
        if (GameManager.Ins.currentState != GameState.StartGame) return;

        if (isTouching)
        {
            timer += Time.deltaTime;
            if (timer >= loseTimer)
            {
                GameManager.Ins.GameOver(); // Gọi hàm GameOver từ GameManager
            }
        }
        else
        {
            timer = 0f; // Reset nếu quả đã rơi xuống dưới varchy
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Chỉ kiểm tra những quả đã được thả xuống (không phải quả đang trên tay)
        if (collision.CompareTag("Fruit"))
        {
            var position = collision.transform.position;
            var scale = position.y * collision.transform.localScale.y;
            var distance1 = Vector2.Distance(position, scale * Vector2.up);
            var distance2 = Vector2.Distance(position, transform.position);

            bool isOutLine = distance2 > distance1;

            // Kiểm tra vận tốc gần bằng 0 để tránh việc quả vừa bay ngang qua đã tính thời gian
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null && rb.linearVelocity.y < 0.1f && isOutLine) 
            {
                isTouching = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Fruit"))
        {
            isTouching = false;
        }
    }
}