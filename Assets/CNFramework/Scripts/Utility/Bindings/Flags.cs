using System;

namespace CNFramework
{
    [Flags]
    public enum AxisFlags
    {
        X,
        Y,
        Z
    }

    [Flags]
    public enum QuaternionAxisFlags
    {
        X,
        Y,
        Z,
        W
    }
}