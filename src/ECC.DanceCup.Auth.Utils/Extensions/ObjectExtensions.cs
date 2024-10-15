﻿namespace ECC.DanceCup.Auth.Utils.Extensions;

public static class ObjectExtensions
{
    public static T AsRequired<T>(this T? obj)
        where T : class
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj;
    }
    
    public static T AsRequired<T>(this T? obj)
        where T : struct
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        return obj.Value;
    }
}