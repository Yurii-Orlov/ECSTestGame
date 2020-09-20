using FullSerializer;

namespace Common.Serialization
{
    public sealed class JsonSerializer
    {
        private static readonly fsSerializer serializer = new fsSerializer();

        public static string Serialize<T>(T value)
        {
            fsData data;
            serializer.TrySerialize(typeof(T), value, out data).AssertSuccessWithoutWarnings();
            return fsJsonPrinter.CompressedJson(data);
        }

        public static T Deserialize<T>(string serializedState)
        {
            fsData data = fsJsonParser.Parse(serializedState);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(T), ref deserialized).AssertSuccessWithoutWarnings();
            return (T)deserialized;
        }
    }
}
