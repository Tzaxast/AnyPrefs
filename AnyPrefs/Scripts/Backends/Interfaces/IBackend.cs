/*
    Copyright (c) 2021 SecureSoftworks. All rights reserved.
    Use of this source code is governed by a BSD-style
    license that can be found in the LICENSE file.
*/

using UnityEngine;

namespace SecureSoftworks.AnyPrefs.backend
{
    public interface IBackend
    {
        bool Initialize();
        bool HasKey(string key);
        void Load();
        void Save();
        void DeleteAll();
        void WriteData(string key, string value);
        void WriteData(string key, int value);
        void WriteData(string key, float value);
        void WriteData(string key, double value);
        void WriteData(string key, long value);
        void WriteData(string key, short value);
        void WriteData(string key, uint value);
        void WriteData(string key, ushort value);
        void WriteData(string key, ulong value);
        void WriteData(string key, bool value);
        void WriteData(string key, Vector2 value);
        void WriteData(string key, Vector3 value);
        void WriteData(string key, Vector4 value);
        void WriteData(string key, Quaternion value);
        void WriteData(string key, Color value);
        void WriteData(string key, byte[] value);
        void WriteData(string key, Rect value);
        string ReadString(string key);
        int ReadInt(string key);
        float ReadFloat(string key);
        bool ReadBool(string key);
        double ReadDouble(string key);
        long ReadLong(string key);
        short ReadShort(string key);
        uint ReadUint(string key);
        ushort ReadUshort(string key);
        ulong ReadUlong(string key);
        byte[] ReadByteArray(string key);
        Vector2 ReadVector2(string key);
        Rect ReadRect(string key);
        Vector3 ReadVector3(string key);
        Vector4 ReadVector4(string key);
        Quaternion ReadQuaternion(string key);
        Color ReadColor(string key);
        void DeleteKey(string key);
    }
}
