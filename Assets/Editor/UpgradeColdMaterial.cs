
using UnityEditor;
using UnityEngine;

public class UpgradeColdMaterial
{
    [MenuItem("Tools/Upgrade Cold Material")]
    public static void Upgrade()
    {
        string path = "Assets/Week7/Prefabs/LowPolyTreePack/Materials/Cold.mat";

        AssetDatabase.ForceReserializeAssets(
            new string[] { path }
        );

        AssetDatabase.Refresh();

        Debug.Log("Cold.mat upgrade completed. Check the Console.");
    }
}
