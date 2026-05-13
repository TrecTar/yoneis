using UnityEngine;

public class YoneisLoader : MonoBehaviour
{
    private static GameObject obj;

    public static void Init()
    {
        if (obj != null) return;

        obj = new GameObject("Yoneis");
        Object.DontDestroyOnLoad(obj);
        obj.AddComponent<YoneisLoader>();
    }

    void Update()
    {
        Optimizer.Update();
    }

    void OnGUI()
    {
        OptimizerUI.Draw();
    }
}