using System;
using Core.SceneConfigurator;

namespace Core.ModelManagement
{
	public class SerializedController : ISerialiser
	{
		private ISerialiser _curSerialiser;

		public SerializedController()
		{
			var config = SceneConfiguratorResource.SceneConfig;

			var serilizableType = config.SerializableType;
			CreateSerilizer(serilizableType);
		}

		private void CreateSerilizer(SerializableType serilizableType)
		{
            switch (serilizableType)
            {
                case SerializableType.None:
                    throw new ArgumentOutOfRangeException("serilizableType", serilizableType, null);
                case SerializableType.Newtonsoft:
                    _curSerialiser = new NewtonsoftSerilizer();
                    break;
                case SerializableType.FullSerializer:
                    _curSerialiser = new FullSerializer();
                    break;
                case SerializableType.GameDevWareSerializer:
                    _curSerialiser = new GameDevWareSerializer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("serilizableType", serilizableType, null);
            }
        }

        public void Serialize<T>(string fullPath, T sourceData)
		{
			_curSerialiser.Serialize(fullPath, sourceData);
		}

		public T DeSerialize<T>(string fullPath)
		{
			return _curSerialiser.DeSerialize<T>(fullPath);
		}
    }
}