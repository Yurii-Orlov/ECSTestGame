using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Helper class for instantiating ScriptableObjects.
/// </summary>
public class ScriptableObjectClassFactory
{
    private const string ScriptableObjectsPath = "Assets/InternalAssets/Scripts/Database/Data/EntityData/Others";
    private const string RecordClassSuffix = "Record";

    private static string CreateClassText(string className)
    {
        var directoryExists = Directory.Exists(ScriptableObjectsPath);
        if (!directoryExists)
        {
            Directory.CreateDirectory(ScriptableObjectsPath);
        }
        
        var copyPath = ScriptableObjectsPath + "/" + className + ".cs";
        Debug.Log("Creating Classfile: " + copyPath);
        
        if (File.Exists(copyPath) == false)
        {
            using (var outfile = new StreamWriter(copyPath))
            {
                outfile.WriteLine("using System;");
                outfile.WriteLine("using System.Collections.Generic;");
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                
                outfile.WriteLine("namespace Core.SimpleData");
                outfile.WriteLine("{");

                var recordClassName = className + RecordClassSuffix;
                var classNameTitle = "	public class " + className  + " : SimpleTableScriptableObject<" + recordClassName + ">";
                outfile.WriteLine(classNameTitle);
                outfile.WriteLine("	{");
                
                
                outfile.WriteLine("		public TextAsset SourceData;");
                
                outfile.WriteLine("");
                
                outfile.WriteLine("		[ContextMenu(\"Parse\")]");
                outfile.WriteLine("		protected override void Parse()");
                outfile.WriteLine("		{");
                outfile.WriteLine("			base.Parse();");
                outfile.WriteLine("		}");
                
                outfile.WriteLine("");
                
                outfile.WriteLine("		protected override string GetRecordSource()");
                outfile.WriteLine("		{");
                outfile.WriteLine("			return SourceData.text;");
                outfile.WriteLine("		}");
                
                outfile.WriteLine("");

                var functionTitle = "		protected override " + recordClassName + " ParseOneRecord(IEnumerable<string> record)";
                outfile.WriteLine(functionTitle);
                outfile.WriteLine("		{");
                outfile.WriteLine("			var enumerator = record.GetEnumerator();");
                outfile.WriteLine("			enumerator.MoveNext();");
                outfile.WriteLine("");
                outfile.WriteLine("			//get first value from IEenumerable");
                outfile.WriteLine("			var firstEnumerableValue = enumerator.Current;");
                outfile.WriteLine("			var recordName = firstEnumerableValue;");
                outfile.WriteLine("			//DebugLogger.Log(null, \"tableValue: \" + tableValue, LogColor.Chartreuse);");
                outfile.WriteLine("			enumerator.Dispose();");
                outfile.WriteLine("");
                var recordNameSmall = char.ToLowerInvariant(className[0]) + className.Substring(1) + RecordClassSuffix;
                var newRecordLine = "			var " + recordNameSmall + " = new " + recordClassName + "();";
                outfile.WriteLine(newRecordLine);
                var returnLine = "			return " + recordNameSmall + ";";
                outfile.WriteLine(returnLine);
                outfile.WriteLine("		}");
                
                outfile.WriteLine("	}");
                outfile.WriteLine("");
                
                outfile.WriteLine("	[Serializable]");
                var recordClassTitle = "	public class " + recordClassName + " : SimpleDataRecord";
                outfile.WriteLine(recordClassTitle);
                outfile.WriteLine("	{");
                outfile.WriteLine("	}");
                
                outfile.WriteLine("}");
            }
        }

        return copyPath;
    }
    
    public static void CreateClass(string className)
    {
        var copyPath = CreateClassText(className);
        
        AssetDatabase.ImportAsset (copyPath, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
    }

    public static void CreateClass(List<string> classNames)
    {
        foreach (var name in classNames)
        {
            CreateClassText(name);
        }
        
        AssetDatabase.Refresh();
    }
}