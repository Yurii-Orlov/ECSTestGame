using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ModelManagement
{
	public interface ISerialiser
	{
		void Serialize<T>(string fullPath, T sourceData);
		T DeSerialize<T>(string fullPath);
	}
}