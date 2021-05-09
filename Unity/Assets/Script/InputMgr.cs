using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class InputMgr : MonoBehaviour {
    public List<RawCloth> raw_clothes;
    private Text m_Text;
    private Camera cam_2d;

    private void Awake() {
        m_Text=GameObject.Find("TestText").GetComponent<Text>();
        cam_2d=GameObject.Find("Cam2d").GetComponent<Camera>();
    }

    class ClickInfo{
        public string type;
        public int obj_id;
        public int node_id;
        public Vector3 pos;
    }

    private ClickInfo CheckClick(Vector3 click_pos){
        ClickInfo re=null;
        Vector2 w_click_pos = Util.Screen2World2D(Input.mousePosition, cam_2d);
        float min_dist=-1;
        int min_obj_id=-1;
        int min_node_id=-1;
        Debug.Log(w_click_pos);
        for (int i=0; i<raw_clothes.Count; i++){
            RawCloth cloth=raw_clothes[i];
            for (int j=0; j<cloth.node_list.Count; j++){
                Vector3 tmp_pos=cloth.node_list[j];
                Vector2 tmp_v2=new Vector2(tmp_pos.x, tmp_pos.y);
                float dist=Vector2.Distance(tmp_v2, w_click_pos);
                Debug.Log(dist+"   "+tmp_v2);
                if (min_dist==-1 || min_dist>dist){
                    min_dist=dist;
                    min_obj_id=i;
                    min_node_id=j;
                }
            }
        }
        float screen_dist = Util.WorldDist2Screen(min_dist, cam_2d);
        if (screen_dist<20 && screen_dist>=0){
            re=new ClickInfo();
            re.type="node";
            re.obj_id=min_obj_id;
            re.node_id=min_node_id;
            re.pos=raw_clothes[min_obj_id].node_list[min_node_id];
        }
        return re;
    }
    private void Update() {
        bool buttonDown = Input.GetMouseButtonDown(0);
        if (buttonDown){
            ClickInfo re= CheckClick(Input.mousePosition);
            if (re!=null){
                m_Text.text = re.type+" "+re.node_id+" "+re.pos;
            }
            
        }
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            m_Text.text = "Touch Position : " + touch.position;
        }
    }
}