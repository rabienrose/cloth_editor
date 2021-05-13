using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RawCloth : MonoBehaviour {
    public List<Vector3> node_list=new List<Vector3>();
    private Mesh m_polyMesh;
    private Material m_polyMaterial;
    private int line_width=2;
    private RawClothMgr raw_cloth_mgr;
    Camera cam_2d;

    private InputMgr input_mgr;
    private void Awake() {
        node_list.Add(new Vector3(-0.2f,-0.4f,-5f));
        node_list.Add(new Vector3(0.2f,-0.4f,-5f));
        node_list.Add(new Vector3(0.2f,0.4f,-5f));
        node_list.Add(new Vector3(-0.2f,0.4f,-5f));
        cam_2d=GameObject.Find("Cam2d").GetComponent<Camera>();
        m_polyMesh=new Mesh();
        GenerateMesh();
        GenerateMaterial();
        input_mgr = GameObject.Find("InputMgr").GetComponent<InputMgr>();
        raw_cloth_mgr=RawClothMgr.Instance;
        raw_cloth_mgr.raw_clothes.Add(this);
    }

    Vector3[] GetLineMesh(Vector3 p1, Vector3 p2){
        Vector3 line_dir=p2-p1;
        line_dir.z=0;
        Vector3 d1 = Vector3.Cross(line_dir, Vector3.back).normalized;
        float line_width_w=Util.Screen2WorldDist(line_width, cam_2d);
        d1=d1*line_width_w;
        Vector3 d2=-d1;
        Vector3 w1=d1+p1;
        Vector3 w2=d1+p2;
        Vector3 w3=d2+p2;
        Vector3 w4=d2+p1;
        Vector3[] vertices=new Vector3[]{
            w1,
            w2,
            w4,
            w2,
            w3,
            w4
        };
        return vertices;
    }

    void GenerateMaterial(){
        m_polyMaterial = new Material(Shader.Find("Shader/PolyChamo"));
        m_polyMaterial.SetColor("_Color",  Color.red);
    }

    void GenerateMesh(){
        Vector3[] vertices=new Vector3[(node_list.Count)*6];
        int[] indices=new int[(node_list.Count)*6];
        for (int i=0; i<indices.Length; i++){
            indices[i]=i;
        }
        
        for (int i=0;i<node_list.Count-1; i++){
            Vector3[] vs = GetLineMesh(node_list[i], node_list[i+1]);
            for (int j=0; j<6; j++){
                vertices[i*6+j]=vs[j];
            }
        }
        Vector3[] vs1 = GetLineMesh(node_list[node_list.Count-1], node_list[0]);
        for (int j=0; j<6; j++){
            vertices[(node_list.Count-1)*6+j]=vs1[j];
        }
        m_polyMesh.Clear();
        m_polyMesh.SetVertices(vertices);
        m_polyMesh.SetIndices(indices,MeshTopology.Triangles,0);
    }

    public void OnZoom(){
        GenerateMesh();
    }

    private void Update() {
        Graphics.DrawMesh(m_polyMesh, transform.position, transform.rotation, m_polyMaterial, 0, null, 0, null, false, false, false);
        // Graphics.DrawMesh(m_polyMesh, Vector3.zero, Quaternion.identity, m_polyMaterial, 0, null, 0, null, false, false, false);
    }
}