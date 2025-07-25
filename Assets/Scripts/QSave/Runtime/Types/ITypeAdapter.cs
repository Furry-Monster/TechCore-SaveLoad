using System;

namespace QSave.Runtime
{

    public interface ITypeAdapter
    {
        public Type SourceType { get; }
        public Type TargetType { get; }
        object ConvertToSerializable(object native);
        object ConvertFromSerializable(object serializable);
    }
}