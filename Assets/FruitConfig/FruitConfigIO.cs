using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitConfigIO", menuName = "Config/FruitConfigIO", order = 1)]
public class FruitConfigIO : ScriptableObject
{
    [System.Serializable]
    public class FruitConfig
    {
        public int type;
        public GameObject prefab;
    }

    public List<FruitConfig> fruitConfigs = new List<FruitConfig>();
    public GameObject GetFruitPrefab(int type)
    {
        foreach (var config in fruitConfigs)
        {
            if (config.type == type)
            {
                return config.prefab;
            }
        }
        Debug.LogWarning($"No prefab found for fruit type {type}");
        return null;
    }
    public static FruitConfigIO Instance => Resources.Load<FruitConfigIO>("FruitConfigIO");
}
