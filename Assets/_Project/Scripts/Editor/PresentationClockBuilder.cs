using UnityEditor;
using UnityEngine;
using TMPro;

public static class PresentationClockBuilder
{
    private const string PrefabPath = "Assets/_Project/Prefabs/UI/PresentationClock.prefab";

    [MenuItem("VPS/Create Presentation Clock Prefab")]
    public static void Build()
    {
        // --- root ---
        var root = new GameObject("PresentationClock");

        var shader = Shader.Find("VPS/UnlitColor");

        // --- black border circle (back) ---
        var border = new GameObject("ClockBorder");
        border.transform.SetParent(root.transform, false);
        border.transform.localPosition = new Vector3(0f, 0f, 0.002f);
        border.transform.localScale = Vector3.one;
        border.AddComponent<MeshFilter>().mesh = CreateCircleMesh(64, 0.28f);
        var borderMR = border.AddComponent<MeshRenderer>();
        borderMR.sharedMaterial = new Material(shader) { color = Color.black };

        // --- white face circle (in front of border) ---
        var face = new GameObject("ClockFace");
        face.transform.SetParent(root.transform, false);
        face.transform.localPosition = new Vector3(0f, 0f, 0f);
        face.transform.localScale = Vector3.one;
        face.AddComponent<MeshFilter>().mesh = CreateCircleMesh(64, 0.25f);
        var faceMR = face.AddComponent<MeshRenderer>();
        faceMR.sharedMaterial = new Material(shader) { color = Color.white };

        // --- timer text (in front of everything) ---
        var textObj = new GameObject("TimerText");
        textObj.transform.SetParent(root.transform, false);
        textObj.transform.localPosition = new Vector3(0f, 0f, -0.004f);
        textObj.transform.localScale = new Vector3(0.12f, 0.12f, 0.12f);

        var tmp = textObj.AddComponent<TextMeshPro>();
        tmp.text = "05:00";
        tmp.fontSize = 36;
        tmp.fontStyle = FontStyles.Bold;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;
        tmp.rectTransform.sizeDelta = new Vector2(8f, 4f);

        // --- PresentationTimer on root ---
        var timer = root.AddComponent<PresentationTimer>();
        timer.timerText = tmp;
        timer.totalMinutes = 5f;

        // --- save as prefab ---
        var prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
        Object.DestroyImmediate(root);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[VPS] PresentationClock prefab saved to {PrefabPath}");
        Selection.activeObject = prefab;
        EditorGUIUtility.PingObject(prefab);
    }

    private static Mesh CreateCircleMesh(int segments, float radius)
    {
        var mesh = new Mesh();
        var verts = new Vector3[segments + 1];
        var tris  = new int[segments * 3];

        verts[0] = Vector3.zero;
        for (int i = 0; i < segments; i++)
        {
            float a = 2f * Mathf.PI * i / segments;
            verts[i + 1] = new Vector3(Mathf.Cos(a) * radius, Mathf.Sin(a) * radius, 0f);
        }

        for (int i = 0; i < segments; i++)
        {
            tris[i * 3]     = 0;
            tris[i * 3 + 1] = i + 1;
            tris[i * 3 + 2] = (i % segments) + 1 == segments ? 1 : i + 2;
        }

        mesh.vertices  = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        return mesh;
    }
}
