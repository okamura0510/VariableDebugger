using UnityEngine;
using System.IO;
using System.Text;

namespace VariableDebuggers
{
    /// <summary>
    /// セーブデータ。ゲームデータのロード・セーブを行う。
    /// </summary>
    public class SaveData : MonoBehaviour
    {
        GameData gameData;
        string saveFilePath;
        
        public void Init(GameData gameData)
        {
            this.gameData = gameData;

            // セーブファイルはルート直下に作ると管理しづらいのでディレクトリ配下にしてる
            var saveDir = $"{Application.persistentDataPath}/VariableDebugger/";
            if(!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
            saveFilePath = $"{saveDir}saveData.txt";

            Load();
        }

        public void Load()
        {
            if(Application.isEditor) return; // ScriptableObjectはエディタ上で確認出来るのでセーブ不要

            if(File.Exists(saveFilePath))
            {
                var json = File.ReadAllText(saveFilePath, Encoding.UTF8);
                JsonUtility.FromJsonOverwrite(json, gameData);
            }
            else
            {
                Save();
            }
        }

        public void Save()
        {
            if(Application.isEditor) return; // ScriptableObjectはエディタ上で確認出来るのでセーブ不要

            var json = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveFilePath, json, Encoding.UTF8);
        }
    }
}