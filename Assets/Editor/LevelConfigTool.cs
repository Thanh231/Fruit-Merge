using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class PlacedElement
{
    public bool isFruit;
    public int type;
    public Vector2 position;
    public float scale;
    public int innerFruitForFrozen = 0;
}

// --- APP EDITOR CHÍNH ---
public class SuikaLevelApp : EditorWindow
{
    private List<PlacedElement> placedElements = new List<PlacedElement>();
    private List<TargetData> levelTargets = new List<TargetData>();

    // Mảng lưu scale (Bạn đã custom ở đây)
    public float[] fruitScales = new float[10] { 0.35f, 0.4f, 0.55f, 0.65f, 0.8f, 0.9f, 1f, 1.2f, 1.3f, 1.45f };
    private float[] mechanicScales = new float[5] { 1f, 1f, 1f, 1f, 1f };
    private int frozenInnerFruitType = 0;

    // Trạng thái thao tác
    private bool isPlacing = false;
    private bool placingIsFruit = true;
    private int placingType = 0;

    private Rect playAreaRect;
    private float playAreaWidth = 280f; 
    private Vector2 scrollPos;
    private float baseSize = 40f; 

    [MenuItem("Tools/Suika Level App")]
    public static void ShowWindow()
    {
        SuikaLevelApp window = GetWindow<SuikaLevelApp>("Suika Level Editor");
        window.minSize = new Vector2(800, 600);
    }

    private void OnEnable()
    {
        wantsMouseMove = true; 
        // Đã xóa đoạn for loop tự cộng dồn scale cũ vì bạn đã gán mảng tùy ý ở trên
    }

    private void OnGUI()
    {
        HandleKeyboardShortcuts();

        GUILayout.BeginHorizontal();

        // ================= PANEL TRÁI =================
        DrawLeftPanel();

        // ================= PANEL PHẢI =================
        DrawPlayArea();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();

        HandleMouseEvents();
    }

    private void DrawLeftPanel()
    {
        GUILayout.BeginVertical("box", GUILayout.Width(300));
        scrollPos = GUILayout.BeginScrollView(scrollPos);

        GUILayout.Label("FRUITS (0 - 9)", EditorStyles.boldLabel);
        for (int i = 0; i < 10; i++)
        {
            GUILayout.BeginHorizontal();
            
            GUI.backgroundColor = (isPlacing && placingIsFruit && placingType == i) ? Color.green : Color.white;
            if (GUILayout.Button($"Quả {i}", GUILayout.Height(25), GUILayout.Width(100)))
            {
                isPlacing = true;
                placingIsFruit = true;
                placingType = i;
            }
            GUI.backgroundColor = Color.white;

            GUILayout.Label("Scale:", GUILayout.Width(40));
            fruitScales[i] = EditorGUILayout.FloatField(fruitScales[i], GUILayout.Width(50));
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);
        GUILayout.Label("MECHANICS", EditorStyles.boldLabel);
        
        DrawMechanicButton("1. Box", 1);
        DrawMechanicButton("2. Stone", 2);
        DrawMechanicButton("3. Special Box", 3);
        
        GUILayout.BeginHorizontal();
        DrawMechanicButton("4. Frozen", 4, 100);
        GUILayout.Label("Chứa quả:", GUILayout.Width(60));
        frozenInnerFruitType = EditorGUILayout.IntSlider(frozenInnerFruitType, 0, 9);
        GUILayout.EndHorizontal();

        GUILayout.Space(20);
        
        // --- LEVEL TARGETS ---
        GUILayout.Label("LEVEL TARGETS (Mục tiêu)", EditorStyles.boldLabel);
        if (GUILayout.Button("+ Thêm Mục Tiêu", GUILayout.Height(25)))
        {
            levelTargets.Add(new TargetData { isFruit = true, type = 0, count = 1 });
        }

        for (int i = 0; i < levelTargets.Count; i++)
        {
            GUILayout.BeginHorizontal();
            levelTargets[i].isFruit = EditorGUILayout.ToggleLeft(levelTargets[i].isFruit ? "Là Quả" : "Là Mech", levelTargets[i].isFruit, GUILayout.Width(65));
            GUILayout.Label("Loại:", GUILayout.Width(30));
            levelTargets[i].type = EditorGUILayout.IntField(levelTargets[i].type, GUILayout.Width(30));
            GUILayout.Label("SL:", GUILayout.Width(25));
            levelTargets[i].count = EditorGUILayout.IntField(levelTargets[i].count, GUILayout.Width(30));

            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                levelTargets.RemoveAt(i);
                i--;
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(30);

        // --- BUTTONS ---
        GUI.backgroundColor = new Color(0.1f, 0.5f, 0.9f); 
        if (GUILayout.Button("LOAD CONFIG JSON", GUILayout.Height(35)))
        {
            LoadJSON();
        }
        GUILayout.Space(5);

        GUI.backgroundColor = new Color(0.2f, 0.8f, 0.2f);
        if (GUILayout.Button("EXPORT JSON", GUILayout.Height(50)))
        {
            ExportJSON();
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(10);
        GUI.backgroundColor = new Color(0.8f, 0.2f, 0.2f);
        if (GUILayout.Button("CLEAR ALL", GUILayout.Height(30)))
        {
            placedElements.Clear();
            levelTargets.Clear();
            isPlacing = false;
        }
        GUI.backgroundColor = Color.white;

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawMechanicButton(string label, int type, float width = -1)
    {
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = (isPlacing && !placingIsFruit && placingType == type) ? Color.green : Color.white;
        
        GUILayoutOption widthOpt = width > 0 ? GUILayout.Width(width) : GUILayout.ExpandWidth(true);
        if (GUILayout.Button(label, GUILayout.Height(30), widthOpt))
        {
            isPlacing = true;
            placingIsFruit = false;
            placingType = type;
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Label("Scale:", GUILayout.Width(40));
        mechanicScales[type] = EditorGUILayout.FloatField(mechanicScales[type], GUILayout.Width(50));
        GUILayout.EndHorizontal();
    }

    private void DrawPlayArea()
    {
        playAreaRect = GUILayoutUtility.GetRect(playAreaWidth, 0, GUILayout.Width(playAreaWidth), GUILayout.ExpandHeight(true));
        
        EditorGUI.DrawRect(playAreaRect, new Color(0.15f, 0.15f, 0.15f));
        GUI.Box(playAreaRect, "PLAY AREA (Chuột phải để hủy cầm | Ctrl + Z để hoàn tác)");

        // 1. Vẽ các object ĐÃ ĐẶT (lấy Scale trực tiếp từ biến hiện tại để cập nhật Real-time)
        foreach (var el in placedElements)
        {
            Vector2 absPos = el.position + playAreaRect.position; 
            float liveScale = el.isFruit ? fruitScales[el.type] : mechanicScales[el.type];
            DrawElementGraphic(el.isFruit, el.type, absPos, liveScale, 1.0f);
        }

        // 2. Vẽ PREVIEW CỦA QUẢ ĐANG CẦM TRÊN TAY
        Event e = Event.current;
        if (isPlacing && playAreaRect.Contains(e.mousePosition))
        {
            Vector2 absPos = e.mousePosition; 
            float currentScale = placingIsFruit ? fruitScales[placingType] : mechanicScales[placingType];
            float size = baseSize * currentScale;
            float halfSize = size / 2f;
            
            DrawElementGraphic(placingIsFruit, placingType, absPos, currentScale, 0.4f);

            Handles.color = Color.white;
            if (placingIsFruit || placingType == 2) 
            {
                Handles.DrawWireDisc(absPos, Vector3.forward, halfSize);
                Handles.DrawWireDisc(absPos, Vector3.forward, halfSize + 0.5f);
            }
            else 
            {
                Rect outlineRect = new Rect(absPos.x - halfSize, absPos.y - halfSize, size, size);
                Handles.DrawSolidRectangleWithOutline(outlineRect, Color.clear, Color.white);
            }
        }
    }

    private void DrawElementGraphic(bool isFruit, int type, Vector2 absPos, float scale, float alpha)
    {
        float size = baseSize * scale;

        if (isFruit)
        {
            Color c = Color.HSVToRGB(type / 10f, 0.7f, 0.9f);
            c.a = alpha;
            Handles.color = c;
            Handles.DrawSolidDisc(absPos, Vector3.forward, size / 2f);
            
            GUIStyle style = new GUIStyle(); style.normal.textColor = Color.black; style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(absPos.x - 10, absPos.y - 10, 20, 20), type.ToString(), style);
        }
        else
        {
            Color c = type == 1 ? new Color(0.6f, 0.4f, 0.2f) : 
                      type == 2 ? Color.gray :                  
                      type == 3 ? Color.yellow :                
                                  Color.cyan;                   
            c.a = alpha;

            if (type == 2) 
            {
                Handles.color = c;
                Handles.DrawSolidDisc(absPos, Vector3.forward, size / 2f);
            }
            else 
            {
                EditorGUI.DrawRect(new Rect(absPos.x - size / 2f, absPos.y - size / 2f, size, size), c);
            }
        }
    }

    private void HandleKeyboardShortcuts()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Z && (e.control || e.command))
        {
            if (placedElements.Count > 0)
            {
                placedElements.RemoveAt(placedElements.Count - 1);
                e.Use();
                Repaint(); 
            }
        }
    }

    private void HandleMouseEvents()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseMove && playAreaRect.Contains(e.mousePosition))
        {
            Repaint();
        }

        if (e.type == EventType.MouseDown && e.button == 0 && isPlacing)
        {
            if (playAreaRect.Contains(e.mousePosition))
            {
                Vector2 localPos = e.mousePosition - playAreaRect.position;

                placedElements.Add(new PlacedElement
                {
                    isFruit = placingIsFruit,
                    type = placingType,
                    position = localPos, 
                    innerFruitForFrozen = (!placingIsFruit && placingType == 4) ? frozenInnerFruitType : 0
                });

                Repaint(); 
                e.Use(); 
            }
        }

        if (e.type == EventType.MouseDown && e.button == 1 && isPlacing)
        {
            isPlacing = false;
            Repaint();
        }
    }

    private void LoadJSON()
    {
        string path = EditorUtility.OpenFilePanel("Chọn file Level Config", Application.dataPath, "json");
        
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                LevelConfig config = JsonUtility.FromJson<LevelConfig>(json);

                if (config != null)
                {
                    placedElements.Clear();
                    levelTargets.Clear();

                    // Phục hồi lại mảng Scale từ file JSON
                    if (config.globalFruitScales != null && config.globalFruitScales.Count == 10)
                    {
                        for (int i = 0; i < 10; i++) fruitScales[i] = config.globalFruitScales[i];
                    }
                    if (config.globalMechanicScales != null && config.globalMechanicScales.Count == 5)
                    {
                        for (int i = 0; i < 5; i++) mechanicScales[i] = config.globalMechanicScales[i];
                    }

                    foreach (var f in config.fruits)
                    {
                        placedElements.Add(new PlacedElement {
                            isFruit = true,
                            type = f.type,
                            position = f.position
                        });
                    }

                    foreach (var m in config.mechanics)
                    {
                        placedElements.Add(new PlacedElement {
                            isFruit = false,
                            type = m.mechanicType,
                            position = m.position,
                            innerFruitForFrozen = m.innerFruitType
                        });
                    }

                    if (config.targets != null && config.targets.Count > 0)
                    {
                        levelTargets.AddRange(config.targets);
                    }

                    Debug.Log($"<color=cyan>Nạp Level thành công: {Path.GetFileName(path)}</color>");
                    Repaint(); 
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Lỗi khi đọc file JSON: " + e.Message);
            }
        }
    }

    private void ExportJSON()
    {
        LevelConfig config = new LevelConfig();

        // Ghi lại mảng Scale hiện tại vào file JSON để dùng trong Game
        config.globalFruitScales = new List<float>(fruitScales);
        config.globalMechanicScales = new List<float>(mechanicScales);

        foreach (var el in placedElements)
        {
            if (el.isFruit)
            {
                config.fruits.Add(new FruitData
                {
                    type = el.type,
                    position = el.position
                });
            }
            else
            {
                config.mechanics.Add(new MechanicData
                {
                    mechanicType = el.type,
                    position = el.position,
                    innerFruitType = el.innerFruitForFrozen
                });
            }
        }
        
        config.targets.AddRange(levelTargets);

        string path = EditorUtility.SaveFilePanel("Lưu Config JSON", Application.dataPath, "Level_New", "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, JsonUtility.ToJson(config, true));
            Debug.Log($"<color=green>Đã xuất JSON thành công tại: {path}</color>");
            AssetDatabase.Refresh();
        }
    }
}