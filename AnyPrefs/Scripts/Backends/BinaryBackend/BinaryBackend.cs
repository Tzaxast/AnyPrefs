/*
    Copyright (c) 2021 SecureSoftworks. All rights reserved.
    Use of this source code is governed by a BSD-style
    license that can be found in the LICENSE file.
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SecureSoftworks.AnyPrefs.backend.binary
{
    /// <summary>Storage Backend for Binary files.</summary>
    public class BinaryBackend : IBackend
    {
        private const char Pref_header_start = '<';
        private const char Pref_header_end = '>';

        private static bool _initialised = false;

        private static StreamWriter DataWriter = null;
        private static StreamReader DataReader = null;

        private static Dictionary<string, Dictionary<string, string>> BackendData = null;

        private static string _name = "";

        public static string Name { get => _name; }

        public static Dictionary<string, Dictionary<string, string>> PlayerPref { get => BackendData; }

        public static bool Initialised
        {
            get
            {
                return _initialised;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool HasKey(string key)
        {
            if (!_initialised) Initialize();
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            else
            {
                return PlayerPref[_name].ContainsKey(key);
            }
        }

        /// <summary>
        /// Opens or Creates a BinaryBackend.
        /// </summary>
        /// <returns>True on Success/False on Failure</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public bool Initialize()
        {
            if (_initialised) return true;
            if (File.Exists(Application.persistentDataPath + "/" + "PlayerPrefs.dat"))
            {
                using (DataReader = new StreamReader(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)))
                {
                    if (DataReader == null) { throw new ArgumentNullException(); }
                    BackendData = new()
                    {
                        { GeneratePrefDataName(), new Dictionary<string, string>() }
                    };
                    Load();
                    _name = GeneratePrefDataName();
                }
            }
            else
            {
                BackendData = new()
                {
                    { GeneratePrefDataName(), new Dictionary<string, string>() }
                };
                _name = GeneratePrefDataName();
            }
            DataWriter = new StreamWriter(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite));
            Application.quitting += Dispose;
            _initialised = true;
            return true;
        }

        public void Load()
        {
            InternalLoad();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void InternalLoad()
        {
            string DataboxName;
            int index = 0;

            while (DataReader.Peek() != -1)
            {
                string data = DataReader.ReadLine().Trim();

                if (data != string.Empty)
                {
                    DataboxName = data.Substring(1, data.Length - 2).Trim();
                    if (string.IsNullOrEmpty(DataboxName))
                    {
                        DataboxName = GeneratePrefDataName();
                        _name = DataboxName;
                    }
                    else _name = DataboxName;

                    if (index < (data.Length - 1))
                    {
                        string PlayerPrefName = data.Substring(0, index).TrimEnd();
                        if (PlayerPrefName == "") { return; }
                        string PlayerPrefData = data.Substring(index + 1, data.Length - index - 1).TrimStart();
                        if (BackendData[DataboxName].ContainsKey(PlayerPrefName))
                        {
                            BackendData[DataboxName][PlayerPrefName] = PlayerPrefData;
                        }
                        else
                            BackendData[DataboxName].Add(PlayerPrefName, PlayerPrefData);
                    }
                }
            }
        }

        /// <summary>Saves the current state of BackendData to a file.</summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Save()
        {
            InternalSave();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void DeleteAll()
        {
            PlayerPref[Name].Clear();
            if (DataWriter != null) return;
            File.Delete(Application.persistentDataPath + "/" + "PlayerPrefs.dat");
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void InternalSave()
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> Backend in BackendData)
            {
                DataWriter.WriteLine(Pref_header_start + Backend.Key + Pref_header_end);
                foreach (KeyValuePair<string, string> databox_data in Backend.Value)
                {
                    //PlayerPref Name = PlayerPref BackendData
                    DataWriter.WriteLine(databox_data.Key + "=" + databox_data.Value);
                }
                DataWriter.Flush();
                DataWriter.BaseStream.Position = 0;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static string GeneratePrefDataName()
        {
            return (Application.companyName + Application.productName).GetHashCode().ToString();
        }

        private static void Dispose()
        {
            DataWriter?.Dispose();
            DataWriter = null;
        }
        //Interface Implementations
        #region WriteBackend Data
        public void WriteData(string key, string value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value;
            }
            else
            {
                PlayerPref[Name].Add(key, value);
            }
        }

        public void WriteData(string key, int value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, float value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString("F");
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString("F"));
            }
        }

        public void WriteData(string key, double value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, long value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, short value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, uint value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, ushort value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, ulong value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, bool value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.ToString());
            }
        }

        public void WriteData(string key, Vector2 value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.x.ToString() + "|" + value.y.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.x.ToString() + "|" + value.y.ToString());
            }
        }

        public void WriteData(string key, Vector3 value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString());
            }
        }

        public void WriteData(string key, Vector4 value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString() + "|" + value.w.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString() + "|" + value.w.ToString());
            }
        }

        public void WriteData(string key, Quaternion value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString() + "|" + value.w.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.x.ToString() + "|" + value.y.ToString() + "|" + value.z.ToString() + "|" + value.w.ToString());
            }
        }

        public void WriteData(string key, Color value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = value.r.ToString() + "|" + value.g.ToString() + "|" + value.b.ToString() + "|" + value.a.ToString();
            }
            else
            {
                PlayerPref[Name].Add(key, value.r.ToString() + "|" + value.g.ToString() + "|" + value.b.ToString() + "|" + value.a.ToString());
            }
        }

        public void WriteData(string key, byte[] value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = Convert.ToBase64String(value);
            }
            else
            {
                PlayerPref[Name].Add(key, Convert.ToBase64String(value));
            }
        }

        public void WriteData(string key, Rect value)
        {
            if (HasKey(key))
            {
                PlayerPref[Name][key] = (value.x.ToString() + "|" + value.y.ToString() + "|" + value.width.ToString() + "|" + value.height.ToString());
            }
            else
            {
                PlayerPref[Name].Add(key, value.x.ToString() + "|" + value.y.ToString() + "|" + value.width.ToString() + "|" + value.height.ToString());
            }
        }
        #endregion

        #region ReadBackend Data

        public string ReadString(string key)
        {
            if (HasKey(key))
            {
                return PlayerPref[Name][key];

            }
            else
            {
                return "";
            }
        }

        public int ReadInt(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToInt32(PlayerPref[Name][key]);
            }
            return 0;
        }

        public float ReadFloat(string key)
        {
            if (HasKey(key))
            {

                return Convert.ToSingle(PlayerPref[Name][key]);
            }
            return 0.0f;
        }

        public bool ReadBool(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToBoolean(PlayerPref[Name][key]);
            }
            return false;
        }

        public double ReadDouble(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToDouble(PlayerPref[Name][key]);
            }

            return 0;
        }

        public long ReadLong(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToInt64(PlayerPref[Name][key]);
            }

            return 0L;
        }

        public short ReadShort(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToInt16(PlayerPref[Name][key]);
            }

            return 0;
        }

        public uint ReadUint(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToUInt32(PlayerPref[Name][key]);
            }

            return 0;
        }

        public ushort ReadUshort(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToUInt16(PlayerPref[Name][key]);
            }

            return 0;
        }

        public ulong ReadUlong(string key)
        {
            if (HasKey(key))
            {
                return Convert.ToUInt64(PlayerPref[Name][key]);
            }

            return 0;
        }

        public byte[] ReadByteArray(string key)
        {
            if (HasKey(key))
            {
                return Convert.FromBase64String(PlayerPref[Name][key]);
            }
            return new byte[0];
        }

        public Vector2 ReadVector2(string key)
        {
            if (HasKey(key))
            {
                string[] vectorValues = PlayerPref[Name][key].Split('|');
                return new Vector2(Convert.ToSingle(vectorValues[0]), Convert.ToSingle(vectorValues[1]));
            }
            else
            {
                return Vector2.zero;
            }
        }

        public Vector3 ReadVector3(string key)
        {
            if (HasKey(key))
            {
                string[] vectorValues = PlayerPref[Name][key].Split('|');
                return new Vector3(Convert.ToSingle(vectorValues[0]), Convert.ToSingle(vectorValues[1]), Convert.ToSingle(vectorValues[2]));
            }
            else
            {
                return Vector3.zero;
            }
        }

        public Vector4 ReadVector4(string key)
        {
            if (HasKey(key))
            {
                string[] vectorValues = PlayerPref[Name][key].Split('|');
                return new Vector4(Convert.ToSingle(vectorValues[0]), Convert.ToSingle(vectorValues[1]), Convert.ToSingle(vectorValues[2]), Convert.ToSingle(vectorValues[3]));
            }
            else
            {
                return Vector4.zero;
            }
        }

        public Rect ReadRect(string key)
        {
            if (HasKey(key))
            {
                string[] RectValues = PlayerPref[Name][key].Split('|');
                return new Rect(Convert.ToSingle(RectValues[0]), Convert.ToSingle(RectValues[1]), Convert.ToSingle(RectValues[2]), Convert.ToSingle(RectValues[3]));
            }
            return Rect.zero;
        }

        public Quaternion ReadQuaternion(string key)
        {
            if (HasKey(key))
            {
                string[] vectorValues = PlayerPref[Name][key].Split('|');
                return new Quaternion(Convert.ToSingle(vectorValues[0]), Convert.ToSingle(vectorValues[1]), Convert.ToSingle(vectorValues[2]), Convert.ToSingle(vectorValues[3]));
            }
            else
            {
                return new Quaternion(0, 0, 0, 0);
            }
        }

        public Color ReadColor(string key)
        {
            if (HasKey(key))
            {
                string[] vectorValues = PlayerPref[Name][key].Split('|');
                return new Color(Convert.ToSingle(vectorValues[0]), Convert.ToSingle(vectorValues[1]), Convert.ToSingle(vectorValues[2]), Convert.ToSingle(vectorValues[3]));
            }
            else
            {
                return new Color(0, 0, 0, 0);
            }
        }

        public void DeleteKey(string key)
        {
            PlayerPref[Name].Remove(key);
            InternalSave();
        }

        #endregion
    }
}
