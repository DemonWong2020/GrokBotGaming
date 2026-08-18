using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    static bool started;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoStart()
    {
        if (started) return;
        var existing = FindObjectOfType<GameBootstrap>();
        if (existing != null)
        {
            existing.Build();
            return;
        }

        var go = new GameObject("Game");
        go.AddComponent<GameBootstrap>().Build();
    }

    public void Build()
    {
        if (started) return;
        started = true;

        if (Object.FindObjectOfType<GameState>() == null)
            gameObject.AddComponent<GameState>();

        CreateLight();
        CreateArena();
        var player = CreatePlayer(new Vector3(0f, 2f, 0f));
        CreateCamera(player.transform);
        CreateCoins();
    }

    void CreateLight()
    {
        if (Object.FindObjectOfType<Light>() != null) return;
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    void CreateArena()
    {
        CreateBox("Floor", new Vector3(0f, -0.5f, 0f), new Vector3(24f, 1f, 24f), new Color(0.35f, 0.55f, 0.35f));
        CreateBox("PlatA", new Vector3(-6f, 1.5f, 4f), new Vector3(4f, 0.5f, 4f), new Color(0.55f, 0.45f, 0.3f));
        CreateBox("PlatB", new Vector3(6f, 2.5f, -3f), new Vector3(4f, 0.5f, 4f), new Color(0.55f, 0.45f, 0.3f));
        CreateBox("PlatC", new Vector3(0f, 3.5f, 7f), new Vector3(5f, 0.5f, 3f), new Color(0.55f, 0.45f, 0.3f));
    }

    static void CreateBox(string name, Vector3 pos, Vector3 scale, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = scale;
        var renderer = go.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;
    }

    GameObject CreatePlayer(Vector3 pos)
    {
        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = pos;
        Object.Destroy(player.GetComponent<Collider>());
        var cc = player.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = 0.5f;
        cc.center = Vector3.zero;
        var renderer = player.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = new Color(0.2f, 0.6f, 0.95f);
        player.AddComponent<PlayerController>().spawnPoint = pos;
        return player;
    }

    void CreateCamera(Transform target)
    {
        var cam = Camera.main;
        if (cam == null)
        {
            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            cam = camGo.AddComponent<Camera>();
            camGo.AddComponent<AudioListener>();
        }
        var follow = cam.GetComponent<FollowCamera>();
        if (follow == null) follow = cam.gameObject.AddComponent<FollowCamera>();
        follow.target = target;
        cam.transform.position = target.position + new Vector3(0f, 6f, -10f);
        cam.transform.LookAt(target.position + Vector3.up);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.45f, 0.7f, 0.95f);
    }

    void CreateCoins()
    {
        Vector3[] spots =
        {
            new Vector3(-6f, 2.4f, 4f),
            new Vector3(6f, 3.4f, -3f),
            new Vector3(0f, 4.4f, 7f),
            new Vector3(3f, 1.2f, 2f),
            new Vector3(-4f, 1.2f, -5f)
        };
        var state = Object.FindObjectOfType<GameState>();
        state.RegisterTotal(spots.Length);
        for (int i = 0; i < spots.Length; i++)
        {
            var coin = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            coin.name = "Coin_" + i;
            coin.transform.position = spots[i];
            coin.transform.localScale = new Vector3(0.7f, 0.08f, 0.7f);
            coin.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            var col = coin.GetComponent<CapsuleCollider>();
            if (col != null) col.isTrigger = true;
            var sphere = coin.AddComponent<SphereCollider>();
            sphere.isTrigger = true;
            sphere.radius = 0.8f;
            var renderer = coin.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(1f, 0.84f, 0.2f);
            coin.AddComponent<Coin>();
        }
    }
}
