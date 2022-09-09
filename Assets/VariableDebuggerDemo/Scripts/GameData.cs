using UnityEngine;
using System;
using GodControllers;
using G2U4S;

namespace VariableDebuggers
{
    /// <summary>
    /// ゲームデータ。変数デバッガで値を操作するため一元管理してる。
    /// </summary>
    public class GameData : ScriptableObject
    {
        public float CameraSpeed              = 0.01f;
        public float WalkSpeed                = 0.1f;
        public float RunSpeed                 = 0.2f;
        public float JumpTime                 = 0.6f;
        public float JumpSpeed                = 0.2f;
        public float JumpHeight               = 2;
        public GodDir EnumValue               = GodDir.Up;
        public sbyte SbyteValue               = 1;
        public byte ByteValue                 = 1;
        public short ShortValue               = 1;
        public ushort UshortValue             = 1;
        public int IntValue                   = 1;
        public uint UintValue                 = 1;
        public long LongValue                 = 1;
        public ulong UlongValue               = 1;
        public char CharValue                 = 'p';
        public float FloatValue               = 0.1f;
        public double DoubleValue             = 0.1;
        public bool BoolValue                 = true;
        public string StringValue             = "ピー";
        public SerializableDateTime TimeValue = DateTime.Parse("2022-02-22 22:22");
        public Vector2 Vector2Value           = new Vector2(0.1f, 0.1f);
        public Vector3 Vector3Value           = new Vector3(0.1f, 0.1f, 0.1f);
        public Vector4 Vector4Value           = new Vector4(0.1f, 0.1f, 0.1f, 0.1f);
        public Rect RectValue                 = new Rect(0.1f, 0.1f, 0.1f, 0.1f);
        public Vector2Int Vector2IntValue     = new Vector2Int(1, 1);
        public Vector3Int Vector3IntValue     = new Vector3Int(1, 1, 1);
        public RectInt RectIntValue           = new RectInt(1, 1, 1, 1);
        public Quaternion QuaternionValue     = Quaternion.identity;
        public Color ColorValue               = new Color(0, 0, 0);
        public Color32 Color32Value           = new Color32(255, 255, 255, 255);
        public string[] ArrayValues           = new string[] { "てつ", "クロ", "ピー" };
        public Vector2[] StructArrayValues    = new Vector2[] { new Vector2(0.1f, 0.1f), new Vector2(0.2f, 0.2f) };
    }
}