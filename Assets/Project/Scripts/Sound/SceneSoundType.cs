using Cysharp.Threading.Tasks;
using System;

[Serializable]
public class SceneSoundType<T> : BaseSoundType where T : Enum
{
    public T Type;
}

