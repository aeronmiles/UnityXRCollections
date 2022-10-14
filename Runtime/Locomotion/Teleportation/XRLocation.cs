using System;
using System.Collections.Generic;
using AM.Unity.Component.System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class XRLocation : EntityComponent
{
    public MatchOrientation m_MatchOrientation = MatchOrientation.TargetUpAndForward;
    public Type EnumType { get; protected set; }

    public string Name { get; protected set; }

    public virtual TeleportRequest TeleportRequest()
    {
        var l = transform;
        return new TeleportRequest
        {
            destinationPosition = l.position,
            destinationRotation = l.rotation,
            matchOrientation = m_MatchOrientation
        };
    }

    public void Initialize<T>(T location) where T : struct, IConvertible
    {
        var type = typeof(T);
        if (type.IsEnum)
        {
            EnumType = type;
            Name = location.ToString();
        }
        else
            Debug.LogError("location Type must be of Enum");

    }

    protected new void Awake()
    {
        base.Awake();
    }

    protected new void OnDestroy()
    {
        base.OnDestroy();
    }
}

public static class XRLocationExt
{
    public static XRLocation GetLocation<T>(this IEnumerable<XRLocation> xrLocations, T location) where T : struct, IConvertible
    {
        var type = typeof(T);
        if (type.IsEnum)
        {
            string name = location.ToString();
            foreach (var xrLocation in xrLocations)
            {
                if (xrLocation.EnumType == type && xrLocation.Name == name)
                    return xrLocation;
            }

            return null;
        }
        else
        {
            Debug.LogError("location Type must be of Enum");
            return null;
        }
    }
}