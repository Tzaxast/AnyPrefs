/*
    Copyright (c) 2021 SecureSoftworks. All rights reserved.
    Use of this source code is governed by a BSD-style
    license that can be found in the LICENSE file.
*/

//Use AnyPrefs Playerprefs with Binary Backend
using SecureSoftworks.AnyPrefs.backend.binary;
//Use AnyPrefs Playerprefs with JSON Backend
//using SecureSoftworks.AnyPrefs.backend.json;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

//Need this using because it collides with Unity Player Prefs.
using AnyPrefs_PlayerPrefs = SecureSoftworks.AnyPrefs.frontend.PlayerPrefs;

//Use Unity Player Prefs.
using Unity_PlayerPrefs = UnityEngine.PlayerPrefs;

namespace SecureSoftworks.AnyPrefs.Examples
{
    public class BenchmarkPrefs : MonoBehaviour
    {
        private const int prefsIterations = 10000;
        private int i;
        private readonly byte[] testdata = new System.Text.UTF8Encoding(true).GetBytes("this is a test.");
        private readonly Vector2 test_data_vector2 = new Vector2(3f, 12f);
        private readonly Vector3 test_data_vector3 = new Vector3(3f, 4f, 5f);
        private readonly Vector4 test_data_vector4 = new Vector4(3f, 5f, 7f, 8f);

        private readonly Stopwatch sw = new();

        //Use AnyPrefs Playerprefs with Binary Backend
        private BinaryBackend binaryBackend;

        //Use AnyPrefs Playerprefs with JSON Backend
        //private JSONBackend jsonBackend;

        #region test variables
        private int testint;
        private float testfloat;
        private string teststring;
        private bool testbool;
        private byte[] test_byte_array;
        private Vector2 test_vector2;
        private Vector3 test_vector3;
        private Vector4 test_vector4;
        #endregion

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void Start()
        {
            //Initialize the storage Backend class(Binary Backend here)
            binaryBackend = new BinaryBackend();
            //Set the storage backend that will be used.
            AnyPrefs_PlayerPrefs.SetBackend(binaryBackend);
            StartTest();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void SetAnyPlayerPrefs()
        {
            AnyPrefs_PlayerPrefs.SetInt("__a", 1);
            AnyPrefs_PlayerPrefs.SetFloat("__b", 2f);
            AnyPrefs_PlayerPrefs.SetString("__c", "3");
            AnyPrefs_PlayerPrefs.SetBool("__d", true);
            AnyPrefs_PlayerPrefs.SetByteArray("__e", testdata);
            AnyPrefs_PlayerPrefs.SetVector2("__f", test_data_vector2);
            AnyPrefs_PlayerPrefs.SetVector3("__g", test_data_vector3);
            AnyPrefs_PlayerPrefs.SetVector4("__i", test_data_vector4);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void GetAnyPlayerPrefs()
        {
            testint = AnyPrefs_PlayerPrefs.GetInt("__a");
            testfloat = AnyPrefs_PlayerPrefs.GetFloat("__b");
            teststring = AnyPrefs_PlayerPrefs.GetString("__c");
            testbool = AnyPrefs_PlayerPrefs.GetBool("__d");
            test_byte_array = AnyPrefs_PlayerPrefs.GetByteArray("__e");
            test_vector2 = AnyPrefs_PlayerPrefs.GetVector2("__f");
            test_vector3 = AnyPrefs_PlayerPrefs.GetVector3("__g");
            test_vector4 = AnyPrefs_PlayerPrefs.GetVector4("__i");
        }

        private void SetBuildInPlayerPrefs()
        {
            Unity_PlayerPrefs.SetInt("__test1", 1);
            Unity_PlayerPrefs.SetFloat("__test2", 2f);
            Unity_PlayerPrefs.SetString("__test2", "3");
        }

        private void GetBuildInPlayerPrefs()
        {
            testint = Unity_PlayerPrefs.GetInt("__test1");
            testfloat = Unity_PlayerPrefs.GetFloat("__test2");
            teststring = Unity_PlayerPrefs.GetString("__test3");
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void StartTest()
        {
            Debug.Log("AnyPrefs vs Built-in Unity_PlayerPrefs, " + prefsIterations + " iterations for read and write.");

            Debug.Log("AnyPrefs Benchmark Start...");

            sw.Reset();
            sw.Start();

            for (i = 0; i < prefsIterations; ++i)
            {
                SetAnyPlayerPrefs();
                AnyPrefs_PlayerPrefs.Save();
                GetAnyPlayerPrefs();
            }

            sw.Stop();

            Debug.Log("AnyPrefs:" + sw.ElapsedMilliseconds + " ms");

            Debug.Log("AnyPrefs Benchmark End.");

            testint = 0;
            testfloat = 0;
            teststring = "";
            test_vector2 = Vector2.zero;
            test_vector3 = Vector3.zero;
            test_vector4 = Vector4.zero;



            Debug.Log("Built-in PlayerPrefs Benchmark Start...");

            sw.Reset();
            sw.Start();

            for (i = 0; i < prefsIterations; ++i)
            {
                SetBuildInPlayerPrefs();
                Unity_PlayerPrefs.Save();
                GetBuildInPlayerPrefs();
            }

            sw.Stop();

            Debug.Log("Built-in PlayerPrefs:" + sw.ElapsedMilliseconds + " ms");

            Debug.Log("Built-in PlayerPrefs Benchmark End.");

            Unity_PlayerPrefs.DeleteKey("__a");
            Unity_PlayerPrefs.DeleteKey("__b");
            Unity_PlayerPrefs.DeleteKey("__c");

            Debug.Log("Benchmark End.");
        }
    }
}