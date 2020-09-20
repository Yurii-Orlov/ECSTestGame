using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.ModelManagement
{
	public class ProtobuffSerilizer: ISerialiser
	{
		public void Serialize<T>(string fullPath, T sourceData)
		{
			using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
			{
				return;
				ProtoBuf.Serializer.Serialize(fileStream, sourceData);
			}
		}

		public T DeSerialize<T>(string fullPath)
		{
			T returnObj;

			using (var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate))
			{
				returnObj = ProtoBuf.Serializer.Deserialize<T>(fileStream);
			}

			return returnObj;
		}
	}
}