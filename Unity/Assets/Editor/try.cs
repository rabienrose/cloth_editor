#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class ChamoTry
{
    [MenuItem("Chamo/Test Cross")]
    public static void TestCross()
    {
        Vector3 a=Vector3.Cross(new Vector3(-0.4f,0.0f,0f), Vector3.back).normalized;
        Debug.Log(a.normalized);
    }
}
#endif
