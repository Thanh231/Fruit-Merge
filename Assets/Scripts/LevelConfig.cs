using System.Collections.Generic;
using UnityEngine;
// Thêm class này vào
[System.Serializable]
public class TargetData
{
    public bool isFruit; // true: Mục tiêu là Quả, false: Mục tiêu là Mechanic
    public int type;     // Loại quả (0-9) hoặc Loại Mechanic (1-4)
    public int count;    // Số lượng cần đạt
}

// Cập nhật class LevelConfig hiện tại của bạn thành thế này:
[System.Serializable]
public class LevelConfig
{
    // Bảng cấu hình kích thước chung cho cả level
    public List<float> globalFruitScales = new List<float>();
    public List<float> globalMechanicScales = new List<float>();

    public List<FruitData> fruits = new List<FruitData>();
    public List<MechanicData> mechanics = new List<MechanicData>();
    public List<TargetData> targets = new List<TargetData>();
}

[System.Serializable]
public class FruitData
{
    public int type;
    public Vector2 position;
    // ĐÃ XÓA: Biến scale ở đây
}
[System.Serializable]
public class MechanicData
{
    public int mechanicType; 
    public Vector2 position;
    public float scale;
    public int innerFruitType;
}