using System;

namespace CNFramework
{
    [Flags]
    public enum AxisFlags
    {
        X = 0,
        Y = 2,
        Z = 4
    }

    [Flags]
    public enum QuaternionAxisFlags
    {
        X = 0,
        Y = 2,
        Z = 4,
        W = 8
    }
}