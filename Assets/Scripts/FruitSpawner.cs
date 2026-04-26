using Unity.VisualScripting;
using UnityEngine;

public class FruitSpawner : Singleton<FruitSpawner>
{
    [Header("Settings")]
    public GameObject[] fruitPrefabs; // Danh sách các quả nhỏ có thể spawn (level 1-3)
    public Transform spawnPoint;      // Vị trí quả xuất hiện trên thanh ngang
    public float moveSpeed = 5f;

    private GameObject currentFruit;
    private bool canSpawn = true;

    private void OnEnable() {
        EventManager.OnGameStart += StartSpawning;
        EventManager.OnGameOver += StopSpawning;
    }

    private void OnDisable() {
        EventManager.OnGameStart -= StartSpawning;
        EventManager.OnGameOver -= StopSpawning;
    }

    private void StopSpawning(int obj)
    {
        canSpawn = false;
    }

    private void StartSpawning()
    {
        canSpawn = true;
        SpawnNewFruit();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (currentFruit == null && canSpawn)
        {
            SpawnNewFruit();
            return;
        }

        if (currentFruit != null)
        {
            // Di chuyển quả theo chuột/ngón tay
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mousePos.x, spawnPoint.position.y, 0);
            
            // Giới hạn không cho quả văng ra ngoài thùng
            targetPos.x = Mathf.Clamp(targetPos.x, -2.36f + currentFruit.transform.localScale.x, 2.36f - currentFruit.transform.localScale.x); 
            currentFruit.transform.position = targetPos;

            // Thả quả khi nhấc chuột/tay
            if (Input.GetMouseButtonUp(0))
            {
                DropFruit();
            }
        }
    }

    void SpawnNewFruit()
    {
        int randomIndex = Random.Range(0, 4); // Chỉ lấy các quả nhỏ (ví dụ level 0, 1, 2)
        currentFruit = Instantiate(fruitPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
        
        currentFruit.GetComponent<Rigidbody2D>().simulated = false;
    }

    void DropFruit()
    {
        currentFruit.GetComponent<Rigidbody2D>().simulated = true;
        currentFruit = null;
        
        // Tạo khoảng chờ nhỏ trước khi cho quả tiếp theo xuất hiện
        canSpawn = false;
        Invoke("ResetSpawn", 1.0f);
    }

    void ResetSpawn() { canSpawn = true; }
}