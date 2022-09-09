using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Reflection;
using G2U4S;

namespace VariableDebuggers
{
    /// <summary>
    /// 変数デバッガ。ゲームデータをゲームプレイ中に確認したり変更したり出来る。
    /// </summary>
    public class VariableDebugger : MonoBehaviour
    {
        const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        [SerializeField] ScrollRect scrollView;
        [SerializeField] UnityEvent onOpen;
        [SerializeField] UnityEvent onClose;
        object gameData;
        Dictionary<FieldInfo, InputField> variables = new ();

        public bool IsShowing     => scrollView.gameObject.activeSelf;
        public UnityEvent OnOpen  => onOpen;
        public UnityEvent OnClose => onClose;
        
        public void Init(object gameData)
        {
            name          = "VariableDebugger";
            this.gameData = gameData;
            
            var fields = this.gameData.GetType().GetFields(Flags);
            foreach(var field in fields)
            {
                var fieldName  = field.Name;
                var go         = Instantiate(Resources.Load<GameObject>("Prefabs/Variable"), scrollView.content);
                var text       = go.GetComponentInChildren<Text>();
                var inputField = go.GetComponentInChildren<InputField>();
                go.name        = fieldName;
                text.text      = fieldName[0].ToString().ToUpper() + fieldName.Substring(1); // パスカルケース(見た目分かりやすいように)
                variables.Add(field, inputField);
                go.SetActive(true);
            }
        }
        
        public void Open()
        {
            onOpen?.Invoke();
            
            foreach(var variable in variables)
            {
                var field       = variable.Key;
                var inputField  = variable.Value;
                var value       = field.GetValue(gameData);
                inputField.text = G2U4SUtil.ParseString(value);
            }

            scrollView.gameObject.SetActive(true);
        }

        public void Close()
        {
            foreach(var variable in variables)
            {
                var field      = variable.Key;
                var inputField = variable.Value;
                var type       = field.FieldType;
                var value      = inputField.text;
                var setValue   = G2U4SUtil.Parse(type, value);
                field.SetValue(gameData, setValue);
            }

            scrollView.gameObject.SetActive(false);
            onClose?.Invoke();
        }
    }
}