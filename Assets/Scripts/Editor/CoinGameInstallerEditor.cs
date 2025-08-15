using Runtime.Datas;
using Runtime.Managers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CoinGameInstaller))]
    public sealed class CoinGameInstallerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(10);
            GUI.backgroundColor = Color.red;

            if (GUILayout.Button("Clear Save Data"))
            {
                if (EditorUtility.DisplayDialog(
                        "Confirm Clear",
                        "Are you sure you want to reset all coin data?",
                        "Yes", "No"))
                {
                    var saveManager = new JsonSaveManager();
                    var repository = new JsonCoinRepository(saveManager);
                    var installer = (CoinGameInstaller)target;
                    var (coins, income) = installer.startingValues;

                    if (Application.isPlaying)
                    {
                        _ = installer.ClearDataAsync();
                    }
                    else
                    {
                        repository.Delete();
                    }

                    Debug.Log("Coin save file reset.");
                }
            }

            GUI.backgroundColor = Color.white;
        }
    }
}