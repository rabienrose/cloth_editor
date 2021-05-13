using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class InputMgr : MonoBehaviour {
    private Text m_Text;
    private Camera cam_2d;
    private RawClothMgr raw_cloth_mgr;
    private Vector2 mouse_down_pos;
    private Vector2 last_pos;
    private bool in_drag_mouse=false;
    private Vector2 prevPos=new Vector2(0,0);
    private Button grid_btn;
    public GameObject grid_obj;
    private Button add_new_btn;

    private void Awake() {
        m_Text=GameObject.Find("TestText").GetComponent<Text>();
        cam_2d=GameObject.Find("Cam2d").GetComponent<Camera>();
        raw_cloth_mgr=RawClothMgr.Instance;
        grid_btn=GameObject.Find("grid_btn").GetComponent<Button>();
        grid_btn.onClick.AddListener(toggle_grid);
        // add_new_btn=GameObject.Find("add_new_btn").GetComponent<Button>();
        // add_new_btn.onClick.AddListener(add_new_polyline);
    }

    private void toggle_grid(){
        Debug.Log(grid_obj.activeSelf);
        if (grid_obj.activeSelf){
            grid_obj.SetActive(false);
        }else{
            grid_obj.SetActive(true);
        }
    }

    private void add_new_polyline(){
        //new raw cloth
    }


    class ClickInfo{
        public string type;
        public int obj_id;
        public int node_id;
        public Vector3 pos;
    }

    private ClickInfo CheckClick(Vector2 click_pos){
        ClickInfo re=null;
        Vector2 w_click_pos = Util.Screen2World2D(click_pos, cam_2d);
        float min_dist=-1;
        int min_obj_id=-1;
        int min_node_id=-1;
        for (int i=0; i<raw_cloth_mgr.raw_clothes.Count; i++){
            RawCloth cloth=raw_cloth_mgr.raw_clothes[i];
            for (int j=0; j<cloth.node_list.Count; j++){
                Vector3 tmp_pos=cloth.node_list[j];
                Vector2 tmp_v2=new Vector2(tmp_pos.x, tmp_pos.y);
                float dist=Vector2.Distance(tmp_v2, w_click_pos);
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
            re.pos=raw_cloth_mgr.raw_clothes[min_obj_id].node_list[min_node_id];
        }
        return re;
    }

    private void ZoomNotifyRawCloth(){
        for (int i=0; i<raw_cloth_mgr.raw_clothes.Count; i++){
            RawCloth cloth=raw_cloth_mgr.raw_clothes[i];
            cloth.OnZoom();
        }
    }

    private void Update() {
        bool buttonDown = Input.GetMouseButtonDown(0);
        if (buttonDown){
            mouse_down_pos=Input.mousePosition;
            last_pos=mouse_down_pos;
            in_drag_mouse=true;
        }
        
        float zoom_data=Input.mouseScrollDelta.y;
        if (Mathf.Abs(zoom_data)>0.1){
            if (zoom_data>0){
                cam_2d.orthographicSize=cam_2d.orthographicSize*1.05f;
            }else{
                cam_2d.orthographicSize=cam_2d.orthographicSize*0.95f;
            }
            ZoomNotifyRawCloth();
        }
        bool buttonUp = Input.GetMouseButtonUp(0);
        if (buttonUp && in_drag_mouse){
            Vector2 up_pos=Input.mousePosition;
            float dist=Vector2.Distance(up_pos, mouse_down_pos);
            if (dist>10){ //drag
            }else{ //click
                ClickInfo re= CheckClick(up_pos);
                if (re!=null){
                    // m_Text.text = re.type+" "+re.node_id+" "+re.pos;
                }
            }
            in_drag_mouse=false;
        }
        if (Input.touchCount == 2){
            in_drag_mouse=false;
        }
        if (Input.touchCount == 2 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) 
		{
            Vector2 p1_p_c=Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
            Vector2 p2_p_c=Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;
            Vector2 p1_c_c=Input.GetTouch(0).position;
            Vector2 p2_c_c=Input.GetTouch(1).position;
			float curDist = (p1_c_c-p2_c_c).magnitude;
			float prevDist = (p1_p_c-p2_p_c).magnitude;
			float touchDelta = prevDist/curDist;
			cam_2d.orthographicSize=cam_2d.orthographicSize*touchDelta;
            float screen_half_h=Screen.height/2f;
            float screen_half_w=Screen.width/2f;
            Vector2 p1_p_c_s=(p1_p_c-new Vector2(screen_half_w, screen_half_h))/touchDelta;
            p1_p_c_s=p1_p_c_s+new Vector2(screen_half_w, screen_half_h);
            Vector2 p1_p_w_s=Util.Screen2World2D(p1_p_c_s, cam_2d);
            Vector2 p1_c_w=Util.Screen2World2D(p1_c_c, cam_2d);
            Vector2 tmp_t=p1_p_w_s-p1_c_w;
            cam_2d.transform.position=cam_2d.transform.position+new Vector3(tmp_t.x, tmp_t.y, 0);
		}
        if (in_drag_mouse){
            Vector2 mouse_pos=Input.mousePosition;
            m_Text.text = mouse_pos+"";
            float dist=Vector2.Distance(mouse_pos, mouse_down_pos);
            if (dist>10){
                Vector2 p1_p_c=last_pos;
                Vector2 p1_c_c=mouse_pos;
                Vector2 p1_p_w=Util.Screen2World2D(p1_p_c, cam_2d);
                Vector2 p1_c_w=Util.Screen2World2D(p1_c_c, cam_2d);
                Vector2 tmp_t=p1_c_w-p1_p_w;
                cam_2d.transform.position=cam_2d.transform.position-new Vector3(tmp_t.x, tmp_t.y, 0);
                ZoomNotifyRawCloth();
            }
            last_pos=mouse_pos;
        }
    }
}