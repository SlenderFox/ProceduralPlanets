using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShapeSettings : ScriptableObject
{
    public float m_fPlanetRadius = 1;
    public NoiseLayer[] m_nlNoiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool m_bEnabled = true;
        public bool m_bUseFirstLayerAsMask;
        public NoiseSettings m_nsNoiseSettings;
    }
}