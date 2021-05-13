using UnityEngine;

public class GirdBG : MonoBehaviour {
    private Mesh m_gridMesh;
    private Material m_gridMaterial;
    private float m_gridSize = 0.01f;
    void Start(){
        Init();
    }

    void OnDestroy(){
        Cleanup();
    }

    private void Update(){
        Graphics.DrawMesh(m_gridMesh, transform.position, transform.rotation, m_gridMaterial, 0, null, 0, null, false, false, false);
    }

    private void Init(){
        Cleanup();
        m_gridMesh = CreateGridMesh();
        m_gridMaterial = CreateGridMaterial();
    }

    private void Cleanup(){
        if (m_gridMaterial != null){
            Destroy(m_gridMaterial);
        }
        if (m_gridMesh != null){
            Destroy(m_gridMesh);
        }
    }

    public Mesh CreateGridMesh(){
        float spacing=m_gridSize;
        int count = 100;
        Mesh mesh = new Mesh();
        mesh.name = "Grid " + spacing;
        int index = 0;
        int[] indices = new int[count * 8];
        Vector3[] vertices = new Vector3[count * 8];
        for(int i = -count; i < count; ++i){
            // for(int j = -count; j < count+1; ++j){

            // }
            vertices[index] = new Vector3(i * spacing, -count * spacing, 0);
            vertices[index + 1] = new Vector3(i * spacing, count * spacing, 0);
            vertices[index + 2] = new Vector3(-count * spacing, i * spacing, 0);
            vertices[index + 3] = new Vector3(count * spacing, i * spacing, 0);
            indices[index] = index;
            indices[index + 1] = index + 1;
            indices[index + 2] = index + 2;
            indices[index + 3] = index + 3;
            index += 4;
        }
        mesh.vertices = vertices;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
        return mesh;
    }

    private Material CreateGridMaterial(){
        Shader shader =  Shader.Find("Shader/GridChamo");
        Material material = new Material(shader);
        Color color=new Color(0.1f, 0.1f, 0.1f, 1.0f);
        material.SetColor("_GridColor", color);
        return material;
    }
}