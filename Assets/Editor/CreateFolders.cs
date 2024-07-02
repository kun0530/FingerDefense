using UnityEngine;
using UnityEditor;

public class CreateFolders : MonoBehaviour
{
    [MenuItem("Tools/Create Default Folders")]
    private static void CreateDefaultFolders()
    {
        CreateFolder("Assets", "01Scene");
        CreateFolder("Assets", "02Script");
        CreateFolder("Assets", "03Prefab");
        CreateFolder("Assets", "04Image");
        CreateFolder("Assets", "05Audio");
        CreateFolder("Assets", "06Animation");
        CreateFolder("Assets", "07Material");
        CreateFolder("Assets", "08Particle");
        CreateFolder("Assets", "09Font");
        CreateFolder("Assets", "10Shader");

        AssetDatabase.Refresh();
    }
    
    private static void CreateFolder(string parent, string newFolder)
    {
        string guid = AssetDatabase.CreateFolder(parent, newFolder);
        string newFolderPath = AssetDatabase.GUIDToAssetPath(guid);
        Debug.Log("Created folder: " + newFolderPath);
    }
}

