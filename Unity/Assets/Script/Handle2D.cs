using UnityEngine;

public class Handle2D : MonoBehaviour {
    public delegate void NotifyMovement();
    public event NotifyMovement moved;
    private void Awake() {
        CreateArrowObj("up_mesh", Quaternion.identity, new Vector3(0f,0.1f,0f), 0.2f, new Color(0f,1f,0f,1f));
        CreateArrowObj("right_mesh", Quaternion.AngleAxis(-90f, Vector3.forward), new Vector3(0.1f,0f,0f), 0.2f, new Color(1f,0f,0f,1f));
    }

    public void CreateArrowObj(string name, Quaternion rot, Vector3 posi, float scale, Color color){
        GameObject tmp_obj = new GameObject(name);
        MeshFilter mesh_up=tmp_obj.AddComponent<MeshFilter>();
        tmp_obj.transform.SetParent(transform);
        mesh_up.sharedMesh = CreateCone(new Color(1f,1f,1f,1f), 1f);
        MeshRenderer render_up = tmp_obj.AddComponent<MeshRenderer>();
        tmp_obj.transform.rotation=rot;
        tmp_obj.transform.position=posi;
        tmp_obj.transform.localScale=new Vector3(scale,scale,scale);
        Material material = new Material(Shader.Find("Shader/PolyChamo"));
        material.SetColor("_Color",  color);
        tmp_obj.GetComponent<Renderer>().material = material;
    }
    public static Mesh CreateCone(Color color, float scale)
    {
        int segmentsCount = 12;
        float size = 1.0f / 5;
        size *= scale;

        Vector3[] vertices = new Vector3[segmentsCount * 3 + 1];
        int[] triangles = new int[segmentsCount * 6];
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < colors.Length; ++i)
        {
            colors[i] = color;
        }

        float radius = size / 2.6f;
        float height = size;
        float deltaAngle = Mathf.PI * 2.0f / segmentsCount;

        float y = -height;

        vertices[vertices.Length - 1] = new Vector3(0, -height, 0);
        for (int i = 0; i < segmentsCount; i++)
        {
            float angle = i * deltaAngle;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            vertices[i] = new Vector3(x, y, z);
            vertices[segmentsCount + i] = new Vector3(0, 0.01f, 0);
            vertices[2 * segmentsCount + i] = vertices[i];
        }

        for (int i = 0; i < segmentsCount; i++)
        {
            triangles[i * 6] = i;
            triangles[i * 6 + 1] = segmentsCount + i;
            triangles[i * 6 + 2] = (i + 1) % segmentsCount;

            triangles[i * 6 + 3] = vertices.Length - 1;
            triangles[i * 6 + 4] = 2 * segmentsCount + i;
            triangles[i * 6 + 5] = 2 * segmentsCount + (i + 1) % segmentsCount;
        }

        Mesh cone = new Mesh();
        cone.name = "Cone";
        cone.vertices = vertices;
        cone.triangles = triangles;
        cone.colors = colors;

        return cone;
    }
}