/*
    Copyright (c) 2021 SecureSoftworks. All rights reserved.
    Use of this source code is governed by a BSD-style
    license that can be found in the LICENSE file.
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SecureSoftworks.AnyPrefs.backend.json
{
    #region JSON Data Classes
    /// <summary>
    /// JSON Serialization Helper BackendData Classes
    /// </summary>
    public class Data
    {
        public string name = ""; //BackendData Name(String Key)
        public string data = "";//BackendData Value (Dictionary<string, string>) = PlayerPref name(String Key) + PlayerPref data(String Value).
    }
    #endregion

    /// <summary>Storage Backend for JSON files.</summary>
    public class JSONBackend : IBackend
    {
        private static bool _initialised = false;

        private static StreamWriter DataWriter = null;
        private static StreamReader DataReader = null;

        private static Dictionary<string, Dictionary<string, string>> BackendData = null;

        private static string _name;

        public static string Name { get => _name; }

        public static Dictionary<string, Dictionary<string, string>> PlayerPref { get => BackendData; }

        public static bool Initialised
        {
            get
            {
                return _initialised;
            }
        }


        public bool HasKey(string key)
        {
            if (!_initialised) Initialize();
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            else
            {
                return BackendData[_name].ContainsKey(key);
            }
        }

        /// <summary>
        /// Opens or Creates a BinaryBackend with a default filename(PlayerPrefs.dat).
        /// </summary>
        /// <returns>True on Success/False on failure</returns>

        public bool Initialize()
        {
            if (_initialised) return true;
            if (File.Exists(Application.persistentDataPath + "/" + "PlayerPrefs.dat"))
            {
                DataReader = new StreamReader(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete));
                if (DataReader == null) { throw new ArgumentNullException(); }
                DataWriter = new StreamWriter(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete));
                BackendData = new()
                    {
                        { GenerateBackendName(), new Dictionary<string, string>() }
                    };
                Load();
            }
            else
            {
                BackendData = new()
                {
                    { GenerateBackendName(), new Dictionary<string, string>() }
                };
                _name = GenerateBackendName();
                DataReader = new StreamReader(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete));
                DataWriter = new StreamWriter(File.Open(Application.persistentDataPath + "/" + "PlayerPrefs.dat", FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read | FileShare.Write | FileShare.Delete));
            }
            _initialised = true;
            return true;
        }

        public void Load()
        {
            InternalLoad();
        }


        private static bool InternalLoad()
        {
            string data = DataReader.ReadToEnd();
            string[] databoxes;
            string dataSep = "(&)";
            char dataKeySep = '@';
            char dataValueSep = '|';
            string databox_data;
            string[] KeyValuePairs;

            if (data.Contains(dataSep))
            {
                databoxes = data.Split(dataSep, StringSplitOptions.RemoveEmptyEntries);

            }

            Data deserialize_player_prefs = JsonUtility.FromJson<Data>(data);
            if (deserialize_player_prefs != null)
            {
                if (string.IsNullOrEmpty(deserialize_player_prefs.name))
                {
                    _name = GenerateBackendName();
                }
                BackendData.Add(_name, new Dictionary<string, string>());
                databox_data = string.Concat(deserialize_player_prefs.data.Split(dataValueSep));
                KeyValuePairs = databox_data.Split(dataKeySep);
                for (int i = 0; i < KeyValuePairs.Length; i += 2)
                {
                    BackendData[_name].Add(KeyValuePairs[i], KeyValuePairs[i + 1]);
                }
                return true;
            }
            return false;
        }

        /// <summary>Saves the current state of databoxes and values to a file.</summary>
        /// <param name="path">Path of a file to save the data.</param>

        public void Save()
        {
            InternalSave();
        }

        public void DeleteAll()
        {
            File.Delete(Application.persistentDataPath + "/" + "PlayerPrefs.dat");
        }


        private static void InternalSave()
        {
            int i = 0;
            if (BackendData.Count > 1)
            {
                Data[] BackendArray = new Data[BackendData.Count];
                foreach (KeyValuePair<string, Dictionary<string, string>> Backend in BackendData)
                {
                    BackendArray[i].name = Backend.Key;

                    foreach (KeyValuePair<string, string> databox_data in Backend.Value)
                    {
                        BackendArray[i].data += databox_data.Key + "@" + databox_data.Value + "|";
                    }
                    i++;
                }
                foreach (var Backend in BackendArray)
                {
                    DataWriter.Write(JsonUtility.ToJson(Backend) + "(&)");
                }
                DataWriter.Flush();
            }
            else
            {
                Data serialized_player_prefs = new()
                {
                    name = _name
                };
                i = 0;
                foreach (KeyValuePair<string, string> playerPref_data in BackendData[_name])
                {
                    serialized_player_prefs.data += playerPref_data.Key + '@' + playerPref_data.Value + '|';
                    i++;
                }
                DataWriter.Write(JsonUtility.ToJson(serialized_player_prefs));
                DataWriter.Flush();
            }

        }

        private static string GenerateBackendName()
        {
            return (Application.companyName + Application.buildGUID + Application.productName).GetHashCode().ToString();
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