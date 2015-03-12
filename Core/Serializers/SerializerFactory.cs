using System;

namespace Core.Serializers
{
    public enum SerializerType
    {
        Newtonsoft = 0,
        ProtocolBuffers = 10
    }

    public static class SerializerFactory
    {
        public static ISerializer Create(SerializerType serializerType)
        {
            switch (serializerType)
            {
                case SerializerType.Newtonsoft:
                    return new NewtonsoftJsonSerializer();

                case SerializerType.ProtocolBuffers:
                    return new ProtocolBuffersSerializer();

                default:
                    throw new InvalidOperationException(serializerType + " is not implemented.");
            }
        }
    }
}
