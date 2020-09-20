using GameDevWare.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.ModelManagement
{

    public class GameDevWareSerializer : ISerialiser
    {
        public T DeSerialize<T>(string fullPath)
        {
            var returnObj = default(T);

            var fileExists = File.Exists(fullPath);
            if (!fileExists)
            {
                return returnObj;
            }

            var sourceStr = File.ReadAllText(fullPath);

            returnObj = Json.Deserialize<T>(sourceStr, SerializationOptions.SuppressTypeInformation);

            return returnObj;
        }

        public void Serialize<T>(string fullPath, T sourceData)
        {
            var str = Json.SerializeToString(sourceData, SerializationOptions.SuppressTypeInformation);
            //Debug.Log("str:" + str);

            if (!File.Exists(fullPath))
            {
                var file = File.Create(fullPath);
                file.Close();
            }

            StreamWriter streamWriter = null;

            try
            {
                streamWriter = new StreamWriter(fullPath);
                streamWriter.WriteLine(str);
            }
            catch (Exception e)
            {
                Debug.LogError("WRITE ERROR:" + e.Message);
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
        }
    }

}