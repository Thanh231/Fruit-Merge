using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : Singleton<FruitSpawner>
{
    [Header("Settings")]
    public GameObject[] fruitPrefabs;
    public Transform spawnPoint;
    public float moveSpeed = 5f;

    [Header("Trajectory Settings")]
    public GameObject dirObject;
    private float trajectorySpacing = 0.08f;
    private List<GameObject> dirObjectsList = new List<GameObject>();

    private GameObject currentFruit;
    private bool canSpawn = true;

    [Header("Thông số Tool (Ước lượng từ JSON)")]

    [Header("Thông số Game World")]
    public Vector2 gameBoxScale = new Vector2(4.3f, 4.1f);
    public Vector3 spawnBasePosition = new Vector3(0, -2.02f, 0);
    public Vector2 toolPlayAreaSize = new Vector2(280f, 600f);

    public float[] fruitScales = new float[] { 0.35f, 0.4f, 0.55f, 0.65f, 0.8f, 0.9f, 1f, 1.2f, 1.3f, 1.45f };
    private void OnEnable()
    {
        EventManager.OnGameStart += StartSpawning;
        EventManager.OnGameOver += StopSpawning;

        var dirGroup = new GameObject("TrajectoryDots");
        for (int i = 0; i < 50; i++)
        {
            GameObject dirObj = Instantiate(dirObject, Vector3.zero, Quaternion.identity);
            dirObj.SetActive(false);
            dirObjectsList.Add(dirObj);
            dirObj.transform.SetParent(dirGroup.transform);
        }
    }

    private void OnDisable()
    {
        EventManager.OnGameStart -= StartSpawning;
        EventManager.OnGameOver -= StopSpawning;
    }

    private void StopSpawning(int obj)
    {
        canSpawn = false;
        HideTrajectory(); // Ẩn đường kẻ khi game over
    }

    public void SpawnLevel(LevelConfig config)
    {
        float minWorldX = spawnBasePosition.x - (gameBoxScale.x / 2f);
        float maxWorldX = spawnBasePosition.x + (gameBoxScale.x / 2f);

        float minWorldY = spawnBasePosition.y - (gameBoxScale.y / 2f);
        float maxWorldY = spawnBasePosition.y + (gameBoxScale.y / 2f);

        foreach (var fruitData in config.fruits)
        {
            float normalizedX = fruitData.position.x / toolPlayAreaSize.x;
            float normalizedY = 1.0f - (fruitData.position.y / toolPlayAreaSize.y);
            float worldX = Mathf.Lerp(minWorldX, maxWorldX, normalizedX);
            float worldY = Mathf.Lerp(minWorldY, maxWorldY, normalizedY);

            Vector3 finalSpawnPos = new Vector3(worldX, worldY, 0);

            var fruit = FruitConfigIO.Instance.GetFruitPrefab(fruitData.type);
            GameObject fruitObj = Instantiate(fruit, finalSpawnPos, Quaternion.identity);

            // fruitObj.transform.localScale = Vector3.one * fruitData.scale;

        }
    }

    public static async Cysharp.Threading.Tasks.UniTask<LevelConfig> LoadLevelData(int levelNumber)
    {
        var filename = $"level_{levelNumber}.json";
        var filetext = await StaticUtils.GetStreamingFileText(filename);
        var currentLevel = JsonUtility.FromJson<LevelConfig>(filetext);
        return currentLevel;
    }
    private async void StartSpawning()
    {
        if (!GameManager.Ins.isAdventureMode)
        {
            return;
        }
        Debug.Log("congthanh");
        // canSpawn = true;
        int currentLevel = LevelController.GetCurrentLevel();
        LevelConfig config = await LoadLevelData(currentLevel);
        // Debug.Log(config);
        if (config != null)
        {
            SpawnLevel(config);
        }
        // SpawnNewFruit();
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
            // Cập nhật vị trí quả theo chuột
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mousePos.x, spawnPoint.position.y, 0);

            targetPos.x = Mathf.Clamp(targetPos.x, -2.36f + currentFruit.transform.localScale.x, 2.36f - currentFruit.transform.localScale.x);
            currentFruit.transform.position = targetPos;

            // Cập nhật đường hiển thị quỹ đạo rơi
            UpdateTrajectory();

            if (Input.GetMouseButtonUp(0))
            {
                DropFruit();
            }
        }
    }

    void SpawnNewFruit()
    {
        int randomIndex = Random.Range(0, 4);
        currentFruit = Instantiate(fruitPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

        currentFruit.GetComponent<Rigidbody2D>().simulated = false;
    }

    void DropFruit()
    {
        currentFruit.GetComponent<Rigidbody2D>().simulated = true;
        currentFruit = null;

        HideTrajectory();

        canSpawn = false;
        Invoke("ResetSpawn", 1.0f);
    }

    void ResetSpawn()
    {
        canSpawn = true;
    }


    void UpdateTrajectory()
    {
        if (currentFruit == null) return;

        float startY = currentFruit.transform.position.y - (currentFruit.transform.localScale.y / 2f) - 0.2f;

        for (int i = 0; i < dirObjectsList.Count; i++)
        {
            GameObject dot = dirObjectsList[i];
            dot.SetActive(true);

            float dotY = startY - (i * trajectorySpacing);
            dot.transform.position = new Vector3(currentFruit.transform.position.x, dotY, 0);
        }
    }

    void HideTrajectory()
    {
        foreach (GameObject dot in dirObjectsList)
        {
            if (dot != null)
            {
                dot.SetActive(false);
            }
        }
    }
}