using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid };
    public FilterType m_ftFilterType;

    [ConditionalHide("m_ftFilterType", 0)]
    public SimpleNoiseSettings m_snsSimpleNoiseSettings;
    [ConditionalHide("m_ftFilterType", 1)]
    public RigidNoiseSettings m_rnsRigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        [Range(1, 8)]
        public int pNumLayers = 1;
        public Vector3 m_v3Centre;
        [Space(4)]
        public float m_fStrength = 1;
        public float m_fBaseRoughness = 1;
        public float m_fRoughness = 2;
        public float m_fPersistence = 0.5f;
        public float m_fMinValue;
    }

    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings
    {
        public float m_fWeightMultiplier = 0.8f;
    }
}