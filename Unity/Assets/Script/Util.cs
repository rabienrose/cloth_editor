using UnityEngine;
using System.Collections.Generic;
static class Util{
    public static float WorldDist2Screen(float world_dist, Camera cam){
        float cam_half_h=cam.orthographicSize;
        float screen_half_h=Screen.height/2f;
        return world_dist*screen_half_h/cam_half_h;
    }

    public static float Screen2WorldDist(int screendist, Camera cam){
        float cam_half_h=cam.orthographicSize;
        float screen_half_h=Screen.height/2f;
        return screendist*cam_half_h/screen_half_h;
    }

    public static Vector2 Screen2World2D(Vector2 pt, Camera cam){
        float cam_half_h=cam.orthographicSize;
        float cam_half_w=cam_half_h*cam.aspect;
        float screen_half_h=Screen.height/2f;
        float screen_half_w=Screen.width/2f;
        float scale = cam_half_h/screen_half_h;
        Vector3 cam_pos=cam.gameObject.transform.position;
        Vector2 re_v=new Vector2();
        re_v.x=cam_pos.x+(pt.x-screen_half_w)*scale;
        re_v.y=cam_pos.y+(pt.y-screen_half_h)*scale;
        return re_v;
    }
    
    public static Vector2 Screen2World2D3(Vector3 pt, Camera cam){
        Vector2 pt2=new Vector2();
        pt2.x=pt.x;
        pt2.y=pt.y;
        return Screen2World2D(pt2, cam);
    }
}