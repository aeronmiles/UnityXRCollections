using System.Collections.Generic;
using AM.Unity.Component.System;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

[DisallowMultipleComponent]
public class XRTeleportManager : MonoSingletonScene<XRTeleportManager>
{
    [SerializeField] TeleportationProvider m_TeleportationProvider;
    EntityManager m_Em;

    private new void Awake()
    {
        base.Awake();
        m_Em = EntityManager.I(gameObject.scene);
        if (m_TeleportationProvider == null)
            m_TeleportationProvider = FindObjectOfType<TeleportationProvider>();
    }

    public bool Teleport<T>(T location) where T : struct, IConvertible
    {
        var type = typeof(T);
        if (!type.IsEnum)
        {
            Debug.LogError("location Type must be Enum");
            return false;
        }

        var xrLocations = MemPool.Get<List<XRLocation>>();
        var destination = m_Em.ComponentsOfType(ref xrLocations).GetLocation(location);
        xrLocations.Clear();
        xrLocations.FreeTo_MemPool();

        if (destination != null)
        {
            m_TeleportationProvider.QueueTeleportRequest(destination.TeleportRequest());
            Debug.Log($"XRTeleportManager -> TeleportTo({type} location = {destination.Name}) :: Success");

            return true;
        }

        Debug.Log($"XRTeleportManager -> TeleportTo({type} location = {destination.Name}) :: location not found");

        return false;
    }
}