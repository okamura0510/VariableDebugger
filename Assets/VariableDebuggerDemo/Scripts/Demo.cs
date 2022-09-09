using UnityEngine;
using System.Collections;

namespace VariableDebuggers
{
    /// <summary>
    /// デモシーン。ゲームデータ、セーブデータ、変数デバッガ、などゲーム全般に関わるものを管理。
    /// </summary>
    public class Demo : MonoBehaviour
    {
        [SerializeField] GameData gameData;
        [SerializeField] SaveData saveData;
        bool canTouch = true;
        
        public GameData GameData => gameData;
        public bool CanTouch     => canTouch;

        void Awake()
        {
            saveData.Init(gameData);

            var variableDebugger = Instantiate(Resources.Load<VariableDebugger>("Prefabs/VariableDebugger"));
            variableDebugger.Init(gameData);
            variableDebugger.OnOpen.AddListener(OnVariableDebuggerOpen);
            variableDebugger.OnClose.AddListener(OnVariableDebuggerClose);
        }

        void OnVariableDebuggerOpen()
        {
            // タッチ操作無効
            canTouch = false;
        }

        void OnVariableDebuggerClose()
        {
            // アプリを再起動しても値を保持するためにセーブ
            saveData.Save();
            StartCoroutine(ReleaseTouch());
        }

        IEnumerator ReleaseTouch()
        {
            // 1フレ待たないと、GodControllerのUpdateが不正に処理されて、攻撃モーションが誤って呼ばれてしまう
            yield return null;

            // タッチ操作有効
            canTouch = true;
        }
    }
}