using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public class RawClothMgr
{
    private static RawClothMgr instance = null;
    public List<RawCloth> raw_clothes;
    
    RawClothMgr(){
    }

    public static RawClothMgr Instance{
        get{
            if (instance == null){
                instance = new RawClothMgr();
                instance.raw_clothes=new List<RawCloth>();
            }
            return instance;
        }
    }
}