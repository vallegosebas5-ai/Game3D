#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Construye automáticamente MainScene, Player.prefab y los materiales.
// Se ejecuta solo la primera vez que se abre el proyecto (si MainScene no existe)
// y también manualmente desde el menú: Tools > Escape Room > Construir escena.
[InitializeOnLoad]
public static class EscapeRoomSceneBuilder
{
    private const string ScenePath = "Assets/Scenes/MainScene.unity";
    private const string PrefabPath = "Assets/Prefabs/Player.prefab";
    private const string MatFolder = "Assets/Materials";

    static EscapeRoomSceneBuilder()
    {
        EditorApplication.delayCall += () =>
        {
            if (!File.Exists(ScenePath) && !EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Build();
            }
        };
    }

    [MenuItem("Tools/Escape Room/Construir escena")]
    public static void Build()
    {
        EnsureFolder("Assets", "Scenes");
        EnsureFolder("Assets", "Prefabs");
        EnsureFolder("Assets", "Materials");

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Material floorMat = Mat("Floor", new Color(0.35f, 0.27f, 0.2f));
        Material wallMat = Mat("Wall", new Color(0.75f, 0.72f, 0.65f));
        Material woodMat = Mat("Wood", new Color(0.5f, 0.3f, 0.12f));
        Material doorMat = Mat("Door", new Color(0.3f, 0.18f, 0.08f));
        Material goldMat = Mat("Gold", new Color(1f, 0.8f, 0.1f));
        Material panelMat = Mat("Panel", new Color(0.15f, 0.2f, 0.25f));
        Material paperMat = Mat("Paper", new Color(0.95f, 0.92f, 0.8f));

        // --- Iluminación ---
        var sun = new GameObject("Directional Light");
        var sl = sun.AddComponent<Light>();
        sl.type = LightType.Directional;
        sl.intensity = 0.6f;
        sun.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var lamp = new GameObject("Room Light");
        var ll = lamp.AddComponent<Light>();
        ll.type = LightType.Point;
        ll.range = 14f;
        ll.intensity = 1.4f;
        ll.color = new Color(1f, 0.92f, 0.8f);
        lamp.transform.position = new Vector3(0f, 3f, 0f);

        // --- Habitación (10 x 10, alto 3.5) ---
        var room = new GameObject("Room");
        Box("Floor", room.transform, new Vector3(0, -0.05f, 0), new Vector3(10, 0.1f, 10), floorMat);
        Box("Ceiling", room.transform, new Vector3(0, 3.55f, 0), new Vector3(10, 0.1f, 10), wallMat);
        Box("Wall_South", room.transform, new Vector3(0, 1.75f, -5f), new Vector3(10, 3.5f, 0.2f), wallMat);
        Box("Wall_East", room.transform, new Vector3(5f, 1.75f, 0), new Vector3(0.2f, 3.5f, 10), wallMat);
        Box("Wall_West", room.transform, new Vector3(-5f, 1.75f, 0), new Vector3(0.2f, 3.5f, 10), wallMat);
        // Pared norte con hueco para la puerta (ancho 1.2, alto 2.2)
        Box("Wall_North_L", room.transform, new Vector3(-2.8f, 1.75f, 5f), new Vector3(4.4f, 3.5f, 0.2f), wallMat);
        Box("Wall_North_R", room.transform, new Vector3(2.8f, 1.75f, 5f), new Vector3(4.4f, 3.5f, 0.2f), wallMat);
        Box("Wall_North_Top", room.transform, new Vector3(0f, 2.85f, 5f), new Vector3(1.2f, 1.3f, 0.2f), wallMat);
        // Exterior (para que se vea algo al abrir la puerta)
        Box("Outside_Floor", room.transform, new Vector3(0, -0.05f, 8f), new Vector3(6, 0.1f, 6), Mat("Grass", new Color(0.25f, 0.55f, 0.25f)));

        // --- GameManager ---
        var gmGo = new GameObject("GameManager");
        gmGo.AddComponent<GameManager>();

        // --- Cofre ---
        var chest = new GameObject("Chest");
        chest.transform.position = new Vector3(3f, 0f, 2.5f);
        Box("Chest_Base", chest.transform, new Vector3(0, 0.3f, 0), new Vector3(1f, 0.6f, 0.6f), woodMat, local: true);
        var lidPivot = new GameObject("Lid_Pivot");
        lidPivot.transform.SetParent(chest.transform, false);
        lidPivot.transform.localPosition = new Vector3(0, 0.6f, 0.3f);
        Box("Chest_Lid", lidPivot.transform, new Vector3(0, 0.1f, -0.3f), new Vector3(1.02f, 0.2f, 0.62f), woodMat, local: true);
        var chestCtrl = chest.AddComponent<ChestController>();

        // Llave dentro del cofre (oculta hasta abrirlo)
        var key = new GameObject("Key");
        key.transform.SetParent(chest.transform, false);
        key.transform.localPosition = new Vector3(0, 0.95f, 0);
        Box("Key_Shaft", key.transform, new Vector3(0, 0, 0), new Vector3(0.4f, 0.06f, 0.06f), goldMat, local: true);
        Box("Key_Head", key.transform, new Vector3(-0.22f, 0, 0), new Vector3(0.14f, 0.14f, 0.06f), goldMat, local: true);
        Box("Key_Tooth", key.transform, new Vector3(0.15f, -0.06f, 0), new Vector3(0.05f, 0.08f, 0.06f), goldMat, local: true);
        var keyCol = key.AddComponent<BoxCollider>();
        keyCol.size = new Vector3(0.6f, 0.3f, 0.3f);
        key.AddComponent<KeyItem>();
        key.SetActive(false);

        Set(chestCtrl, "lid", lidPivot.transform);
        Set(chestCtrl, "keyInside", key);

        // --- Puzzle ---
        var puzzleGo = new GameObject("PuzzleManager");
        var puzzle = puzzleGo.AddComponent<PuzzleManager>();
        Set(puzzle, "chest", chestCtrl);

        var terminal = Box("PuzzleTerminal", null, new Vector3(4.85f, 1.3f, -1f), new Vector3(0.1f, 0.6f, 0.45f), panelMat);
        var term = terminal.AddComponent<PuzzleTerminal>();
        Set(term, "puzzleManager", puzzle);

        var note = Box("ClueNote", null, new Vector3(-4.88f, 1.5f, 0.5f), new Vector3(0.03f, 0.4f, 0.3f), paperMat);
        note.AddComponent<ClueNote>();

        // --- Puerta ---
        var door = new GameObject("Door");
        door.transform.position = new Vector3(0f, 0f, 5f);
        var hinge = new GameObject("Door_Hinge");
        hinge.transform.SetParent(door.transform, false);
        hinge.transform.localPosition = new Vector3(-0.6f, 0f, 0f);
        Box("Door_Panel", hinge.transform, new Vector3(0.6f, 1.1f, 0f), new Vector3(1.2f, 2.2f, 0.1f), doorMat, local: true);
        Box("Door_Handle", hinge.transform, new Vector3(1.05f, 1.05f, -0.08f), new Vector3(0.1f, 0.05f, 0.05f), goldMat, local: true);
        var doorCtrl = door.AddComponent<DoorController>();
        Set(doorCtrl, "hinge", hinge.transform);

        // --- Player ---
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, -3f);
        var rb = player.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        var camGo = new GameObject("PlayerCamera");
        camGo.tag = "MainCamera";
        camGo.transform.SetParent(player.transform, false);
        camGo.transform.localPosition = new Vector3(0f, 0.6f, 0f);
        var cam = camGo.AddComponent<Camera>();
        cam.nearClipPlane = 0.05f;
        camGo.AddComponent<AudioListener>();

        player.AddComponent<PlayerController>();
        var look = player.AddComponent<PlayerLook>();
        Set(look, "playerCamera", camGo.transform);
        var interaction = player.AddComponent<PlayerInteraction>();
        Set(interaction, "playerCamera", cam);
        Set(interaction, "interactionDistance", 2.5f);

        PrefabUtility.SaveAsPrefabAssetAndConnect(player, PrefabPath, InteractionMode.AutomatedAction);

        // --- UI ---
        var ui = new GameObject("UI");
        var hud = ui.AddComponent<GameHUD>();
        Set(hud, "playerInteraction", interaction);

        RenderSettings.ambientLight = new Color(0.35f, 0.35f, 0.4f);

        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();

        Debug.Log("Escape Room: MainScene y Player.prefab creados. Presiona Play.");
    }

    // ----------------- helpers -----------------

    private static GameObject Box(string name, Transform parent, Vector3 pos, Vector3 scale, Material mat, bool local = false)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        if (parent != null) go.transform.SetParent(parent, false);
        if (local) go.transform.localPosition = pos; else go.transform.position = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    private static Material Mat(string name, Color color)
    {
        string path = $"{MatFolder}/{name}.mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) return existing;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null || UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline == null)
            shader = Shader.Find("Standard");

        var m = new Material(shader) { color = color };
        if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", color);
        AssetDatabase.CreateAsset(m, path);
        return m;
    }

    private static void Set(Object target, string field, Object value)
    {
        var so = new SerializedObject(target);
        so.FindProperty(field).objectReferenceValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void Set(Object target, string field, float value)
    {
        var so = new SerializedObject(target);
        so.FindProperty(field).floatValue = value;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    private static void EnsureFolder(string parent, string name)
    {
        if (!AssetDatabase.IsValidFolder($"{parent}/{name}"))
            AssetDatabase.CreateFolder(parent, name);
    }
}
#endif
