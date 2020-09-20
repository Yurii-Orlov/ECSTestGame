using System.IO;
using CustomLogger;
using InternalAssets.Scripts.Engine.Utilities;
using OrCor_GameName;

namespace Core.ModelManagement
{
	public abstract class BaseModelObject : IData
	{
		protected abstract string ModelPath { get; }

		public abstract void Write();

		protected void WriteModelToFile(string filePath)
		{
			var modelPath = ModelController.GetPathForData();

			var path = PathUtils.Combine(modelPath, filePath);
			if (!Directory.Exists(modelPath))
			{
				Directory.CreateDirectory(modelPath);
			}

			PlayerDataManager.SerializedController.Serialize(path, this);
		}

		public abstract BaseModelObject Read();
		public abstract void Init();

		protected T ReadModelFromFile<T>(string filePath) where T: IData
		{
			var modelPath = ModelController.GetPathForData();

			var path = PathUtils.Combine(modelPath, filePath);
			
			if (!Directory.Exists(modelPath))
			{
				Directory.CreateDirectory(modelPath);
			}

			var result = PlayerDataManager.SerializedController.DeSerialize<T>(path);

			return result;
		}
	}
}