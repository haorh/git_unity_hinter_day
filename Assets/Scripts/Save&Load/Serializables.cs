using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class SlotStatisticsList
{
    public List<SlotStatistics> slotLists;

    public List<SlotStatistics> GetTurretStatisticList()
    {
        return slotLists;
    }    
}

[Serializable]
public class SlotStatistics 
{
    public string turretName;
    public string parentName;
    public float localPosX, localPosY;
    public float localRotationX, localRotationY, localRotationZ;        
}

[Serializable]
public class MachineGunData
{
    public float fireRateLevel;
    public float damageLevel;
    public float speedLevel;
    public float accuracyLevel;
}

[Serializable]
public class BasicGunTurretData
{
    public float fireRateLevel;
    public float bulletDamageLevel;
    public float bulletSpeedLevel;
    public float accuracyLevel;
}

[Serializable]
public class PlanetData
{
    public float rotationSpeedLevel;
}