using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private NoiseSettings.SimpleNoiseSettings m_nsSettings;
    private Noise m_nNoise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings pSettings)
    {
        m_nsSettings = pSettings;
    }

    public float Evaluate(Vector3 pPoint)
    {
        float noiseValue = 0;
        float frequency = m_nsSettings.m_fBaseRoughness;
        float amplitude = 1;

        for (int i = 0; i < m_nsSettings.pNumLayers; i++)
        {
            float v = m_nNoise.Evaluate(pPoint * frequency + m_nsSettings.m_v3Centre);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= m_nsSettings.m_fRoughness;
            amplitude *= m_nsSettings.m_fPersistence;
        }

        noiseValue -= m_nsSettings.m_fMinValue;
        return noiseValue * m_nsSettings.m_fStrength;
    }
}