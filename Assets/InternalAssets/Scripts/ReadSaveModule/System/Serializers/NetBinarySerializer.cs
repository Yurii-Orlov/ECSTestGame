using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Core.ModelManagement;

namespace InternalAssets.Scripts.Engine.ReadSaveModule.System.Serializers
{
	public class NetBinarySerializer: ISerialiser
	{
		public void Serialize<T>(string fullPath, T sourceData)
		{
			var fs = new FileStream(fullPath, FileMode.Create);

			// Construct a BinaryFormatter and use it to serialize the data to the stream.
			var formatter = new BinaryFormatter();
			try 
			{
				formatter.Serialize(fs, sourceData);
			}
			catch (SerializationException e) 
			{
				Console.WriteLine("Failed to serialize. Reason: " + e.Message);
				throw;
			}
			finally 
			{
				fs.Close();
			}
		}

		public T DeSerialize<T>(string fullPath)
		{
			var fs = new FileStream(fullPath, FileMode.Open);
			T result;
			try 
			{
				var formatter = new BinaryFormatter();
				result = (T) formatter.Deserialize(fs);
			}
			catch (SerializationException e) 
			{
				Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
				throw;
			}
			finally 
			{
				fs.Close();
			}

			return result;
		}
	}
}