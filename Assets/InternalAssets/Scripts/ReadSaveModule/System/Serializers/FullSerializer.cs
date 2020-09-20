using System;
using System.IO;
using System.Text.RegularExpressions;
using Core.ModelManagement;
using FullSerializer;
using UnityEngine;

namespace Core.ModelManagement
{
	public class FullSerializer: ISerialiser
	{
		private static readonly fsSerializer Serializer = new fsSerializer();
		
		public void Serialize<T>(string fullPath, T sourceData)
		{
			fsData data;
			Serializer.TrySerialize(typeof(T), sourceData, out data).AssertSuccessWithoutWarnings();

			//Debug.Log("[[[FULLLLL Serialize]]]] fullPath:" + fullPath);

			if (!File.Exists(fullPath))
			{
				var file = File.Create(fullPath);
				file.Close();
			}
			
			StreamWriter streamWriter = null; 

			try
			{
				streamWriter = new StreamWriter(fullPath);
				streamWriter.WriteLine(data);
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

        public string SerializeString<T>(T sourceData)
        {
            fsData data;
            Serializer.TrySerialize(typeof(T), sourceData, out data).AssertSuccessWithoutWarnings();
            return data.ToString();
        }

        public T DeSerialize<T>(string fullPath)
		{
			object deserialized = null;
			var returnObj = default(T);

			var fileExists = File.Exists(fullPath);
			if (!fileExists)
			{
				//DebugLogger.Warning(this, " fileNotExists:" + fullPath);
				return returnObj;
			}
			var sourceStr = File.ReadAllText(fullPath);
            var data = fsJsonParser.Parse(sourceStr);
			Serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
			return (T)deserialized;
		}
		
		public T DeSerializeString<T>(string str)
		{
			object deserialized = null;
			var returnObj = default(T);

			if (string.IsNullOrEmpty(str))
			{
				//DebugLogger.Warning(this, " fileNotExists:" + fullPath);
				return returnObj;
			}
			var data = fsJsonParser.Parse(str);
			Serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
			return (T)deserialized;
		}
		
		public T DeSerializeWithAutofixString<T>(string str)
		{
			object deserialized = null;
			var returnObj = default(T);
			if (string.IsNullOrEmpty(str))
			{
				return returnObj;
			}
			var sourceStr = RemoveQuotesFromValues(str);

			var data = fsJsonParser.Parse(sourceStr);
			Serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
			return (T)deserialized;
		}

        public T DeSerializeWithAutofixStringWithSpaces<T>(string str)
		{
            Debug.Log(str);
            if (str == "Flat Calm")
            {
                Debug.Log("1");
            }
			object deserialized = null;
			var returnObj = default(T);
			if (string.IsNullOrEmpty(str))
			{
				return returnObj;
			}
			var sourceStr = RemoveQuotesFromValuesWithSpaces(str);

			var data = fsJsonParser.Parse(sourceStr);
			Serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
			return (T)deserialized;
		}

	    public T DeSerializeWithAutofix<T>(string fullPath)
	    {
	        object deserialized = null;
	        var returnObj = default(T);

	        var fileExists = File.Exists(fullPath);
	        if (!fileExists)
	        {
	            Debug.LogWarning(this + " fileNotExists:" + fullPath);
	            return returnObj;
	        }
	        var sourceStr = File.ReadAllText(fullPath);
	        sourceStr = RemoveQuotesFromValues(sourceStr);

            var data = fsJsonParser.Parse(sourceStr);
	        Serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
	        return (T)deserialized;
	    }

        private string RemoveQuotesFromValues(string input)
	    {
	        string trueRegex = @"(""True"")";
	        string falseRegex = @"(""False"")";
	        string arrayRegex = @"(""\[\]"")";
	        string numbersRegex = @"\""-?([0-9]\d*(?:\.\d+)?)\""";
	        string keys = @""".+?""\s*?:";
	       // string keys = @"\""(.*?)\"""
;

            string result = Regex.Replace(input, trueRegex, "true", RegexOptions.IgnoreCase);
	        result = Regex.Replace(result, falseRegex, "false", RegexOptions.IgnoreCase);
	        result = Regex.Replace(result, arrayRegex, "[]", RegexOptions.IgnoreCase);
	        result = Regex.Replace(result, numbersRegex, delegate (Match match)
	        {
	            string v = match.ToString();
	            return v.Substring(1, v.Length - 2);
            });

	        result = Regex.Replace(result, keys, delegate (Match match)
	        {
	            string v = match.ToString();
	            return v.Replace(" ", string.Empty);
	        });

            return result;
	    }

        private string RemoveQuotesFromValuesWithSpaces(string input)
        {
            string trueRegex = @"(""True"")";
            string falseRegex = @"(""False"")";
            string arrayRegex = @"(""\[\]"")";
            string numbersRegex = @"\""-?([0-9]\d*(?:\.\d+)?)\""";
            string keys = @""".+?""\s*?:";

            string result = Regex.Replace(input, trueRegex, "true", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, falseRegex, "false", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, arrayRegex, "[]", RegexOptions.IgnoreCase);
            result = Regex.Replace(result, numbersRegex, delegate (Match match)
            {
                string v = match.ToString();
                return v.Substring(1, v.Length - 2);
            });

            result = Regex.Replace(result, keys, delegate (Match match)
            {
                string v = match.ToString();
                return v;
                //return v.Replace(" ", string.Empty);
            });

            return result;
        }
    }
}