/*
    Copyright (c) 2021 SecureSoftworks. All rights reserved.
    Use of this source code is governed by a BSD-style
    license that can be found in the LICENSE file.
*/

using FlaxEngine;

namespace SecureSoftworks.AnyPrefs.frontend
{
    /// <summary>
    /// AnyPrefs Unity Asset
    /// Extensible, Indepenent BackendData Storage Class <br></br>
    /// with Player Prefs like API and multiple storage backends.
    /// </summary>
    public static class PlayerPrefs
    {
        private static bool _modified = false;

        private static backend.IBackend _backend;

        /// <summary>
        /// Sets the storage backend class that will be used with the PlayerPrefs Class.<br></br>
        /// Any Class can be used as a storage backend if it implements IBackend interface.
        /// </summary>
        /// <param name="backend"></param>
        public static void SetBackend(backend.IBackend backend)
        {
            if (backend == null) return;
            _backend = backend;
        }

        public static bool Modified
        {
            get { return _modified; }
            private set { _modified = value; }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool InitializePlayerPrefs()
        {
            return _backend.Initialize();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetString(string key, string value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetInt(string key, int value)
        {
            if (key == "") return;
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetFloat(string key, float value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetDouble(string key, double value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetLong(string key, long value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetShort(string key, short value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetUnsignedInt(string key, uint value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetUnsignedShort(string key, ushort value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetUnsignedLong(string key, ulong value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetBool(string key, bool value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetVector2(string key, Vector2 value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetVector3(string key, Vector3 value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetVector4(string key, Vector4 value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetQuaternion(string key, Quaternion value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetColor(string key, Color value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetByteArray(string key, byte[] value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void SetRect(string key, Rect value)
        {
            _backend.WriteData(key, value);
            Modified = true;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string GetString(string key)
        {
            return _backend.ReadString(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int GetInt(string key)
        {
            return _backend.ReadInt(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float GetFloat(string key)
        {
            return _backend.ReadFloat(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool GetBool(string key)
        {
            return _backend.ReadBool(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double GetDouble(string key)
        {
            return _backend.ReadDouble(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long GetLong(string key)
        {
            return _backend.ReadLong(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short GetShort(string key)
        {
            return _backend.ReadShort(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint GetUnisgnedInt(string key)
        {
            return _backend.ReadUint(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort GetUnsignedShort(string key)
        {
            return _backend.ReadUshort(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong GetUnsignedLong(string key)
        {
            return _backend.ReadUlong(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetByteArray(string key)
        {
            return _backend.ReadByteArray(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetVector2(string key)
        {
            return _backend.ReadVector2(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector3 GetVector3(string key)
        {
            return _backend.ReadVector3(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Vector4 GetVector4(string key)
        {
            return _backend.ReadVector4(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Rect GetRect(string key)
        {
            return _backend.ReadRect(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Quaternion GetQuaternion(string key)
        {
            return _backend.ReadQuaternion(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Color GetColor(string key)
        {
            return _backend.ReadColor(key);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void DeleteKey(string key)
        {
            _backend.DeleteKey(key);
        }

        public static void DeleteAll()
        {
            _backend.DeleteAll();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static void Save()
        {
            if (Modified == false)
            {
                return;
            }
            _backend.Save();
            Modified = false;
        }
    }
}
