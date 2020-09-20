using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomLogger;
using GameDevWare.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.ModelManagement
{
	public class NewtonsoftSerilizer: ISerialiser
	{
		public void Serialize<T>(string fullPath, T sourceData)
		{
			//Debug.Log("[[[Serialize]]]] fullPath:" + fullPath);

			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented
			};
			var str = JsonConvert.SerializeObject(sourceData, settings);
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

		public T DeSerialize<T>(string fullPath)
		{
			var returnObj = default(T);

			var fileExists = File.Exists(fullPath);
			if (!fileExists)
			{
				return returnObj;
			}
			
			var sourceStr = File.ReadAllText(fullPath);
			
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			};

            returnObj = JsonConvert.DeserializeObject<T>(sourceStr, settings);

			return returnObj;
		}
	}
}