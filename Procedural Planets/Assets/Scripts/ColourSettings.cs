using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColourSettings : ScriptableObject
{
    public Material m_mPlanetMaterial;
    public BiomeColourSettings m_bcsBiomeColourSettings;
    public Gradient m_gOceanColour;

    [System.Serializable]
    public class BiomeColourSettings
    {
        [System.Serializable]
        public class Biome
        {
            public Gradient m_gGradient;
            public Color m_cTint;
            [Range(0, 1)]
            public float m_fStartHeight;
            [Range(0, 1)]
            public float m_fTintPercent;
        }

        public Biome[] m_bBiomes;
        public NoiseSettings m_nsNoiseSettings;
        public float m_fNoiseOffset;
        public float m_fNoiseStrength;
        [Range(0, 1)]
        public float m_fBlendAmount;
    }
}