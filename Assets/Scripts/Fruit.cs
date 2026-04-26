using UnityEngine;

public class Fruit : MonoBehaviour
{
    [Header("Fruit Settings")]
    public int fruitLevel;
    public GameObject nextFruitPrefab;
    public int scoreValue = 10;
    private bool isMerged = false; 
    private bool canMerged = false; 
    private float timer = 0;

    private void OnCollisionStay2D(Collision2D collision)
    {
        // 1. Kiểm tra nếu vật va chạm cũng có Component Fruit
        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

            if (otherFruit != null && 
                otherFruit.fruitLevel == this.fruitLevel && 
                !isMerged && !otherFruit.isMerged && 
                nextFruitPrefab != null && canMerged)
            {
                if (transform.position.y < collision.transform.position.y || 
                   (transform.position.y == collision.transform.position.y && transform.position.x > collision.transform.position.x))
                {
                    DoMerge(otherFruit);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Kiểm tra nếu vật va chạm cũng có Component Fruit
        if (collision.gameObject.CompareTag("Fruit"))
        {
            Fruit otherFruit = collision.gameObject.GetComponent<Fruit>();

            if (otherFruit != null && 
                otherFruit.fruitLevel == this.fruitLevel && 
                !isMerged && !otherFruit.isMerged && 
                nextFruitPrefab != null && canMerged)
            {
                if (transform.position.y < collision.transform.position.y || 
                   (transform.position.y == collision.transform.position.y && transform.position.x < collision.transform.position.x))
                {
                    DoMerge(otherFruit);
                }
            }
        }
    }

    private void DoMerge(Fruit other)
    {
        isMerged = true;
        other.isMerged = true;

        // Xác định vị trí trung tâm giữa 2 quả để sinh ra quả mới
        Vector3 spawnPos = (transform.position + other.transform.position) / 2f;
        // Vector3 spawnPos = other.transform.position; // Sinh ra quả mới tại vị trí của quả có y cao hơn (để tránh chồng lên nhau)

        // Tạo quả tiếp theo
        GameObject newFruit = Instantiate(nextFruitPrefab, spawnPos, Quaternion.identity);
        
        // Thêm hiệu ứng nổ nhỏ hoặc âm thanh tại đây
        // PlayMergeEffect(spawnPos);

        // Cộng điểm thông qua Singleton ScoreManager
        // ScoreManager.Ins.AddScore(scoreValue);

        // Xóa 2 quả cũ
        Destroy(other.gameObject);
        Destroy(this.gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.3f)
        {
            canMerged = true;
        }
    }
}