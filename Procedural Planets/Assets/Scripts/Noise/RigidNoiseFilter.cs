using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter
{
    private NoiseSettings.RigidNoiseSettings m_nsSettings;
    private Noise m_nNoise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings pSettings)
    {
        m_nsSettings = pSettings;
    }

    public float Evaluate(Vector3 pPoint)
    {
        float noiseValue = 0;
        float frequency = m_nsSettings.m_fBaseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < m_nsSettings.pNumLayers; i++)
        {
            float v = 1-Mathf.Abs(m_nNoise.Evaluate(pPoint * frequency + m_nsSettings.m_v3Centre));
            v *= v;
            v *= weight;
            weight = Mathf.Clamp01(v * m_nsSettings.m_fWeightMultiplier);

            noiseValue += v * amplitude;
            frequency *= m_nsSettings.m_fRoughness;
            amplitude *= m_nsSettings.m_fPersistence;
        }

        noiseValue -= m_nsSettings.m_fMinValue;
        return noiseValue * m_nsSettings.m_fStrength;
    }
}