using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace G2U4S
{
	[Serializable]
	public class SerializableDateTime : ISerializationCallbackReceiver
	{
		const string Format = "yyyy-MM-dd HH:mm:ss";

		public DateTime Value;

		[SerializeField] string text;

		public SerializableDateTime() : this(new DateTime()) { }

		public SerializableDateTime(DateTime value) => Set(value);

		public static implicit operator SerializableDateTime(DateTime value) => new SerializableDateTime(value);

		public void Set(long ticks)
        {
            Value = new DateTime(ticks);
			text  = ToString();
        }

		public void Set(DateTime value)
		{
			Value = value;
			text  = ToString();
		}

		public void OnAfterDeserialize() 
		{
			if(DateTime.TryParse(text, out var newValue))
			{
				Set(newValue);
			}
			else
			{   
				Set(Value);
			}
		}

		public void OnBeforeSerialize() { }

		public override string ToString() => Value.ToString(Format);
	}

	#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(SerializableDateTime))]
	class SerializableDateTimeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField(position, property.FindPropertyRelative("text"), label);
		}
	}
	#endif
}