using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourGenerator
{
    private const int m_iTextureResolution = 50;

    public ColourSettings m_csColourSettings;
    private Texture2D m_t2dTexture;
    private INoiseFilter m_nfBiomeNoiseFilter;

    public void UpdateSettings(ColourSettings pColourSettings)
    {
        m_csColourSettings = pColourSettings;

        if (m_t2dTexture == null || m_t2dTexture.height != m_csColourSettings.m_bcsBiomeColourSettings.m_bBiomes.Length)
            m_t2dTexture = new Texture2D(m_iTextureResolution * 2, m_csColourSettings.m_bcsBiomeColourSettings.m_bBiomes.Length, TextureFormat.RGBA32, false);

        m_nfBiomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(m_csColourSettings.m_bcsBiomeColourSettings.m_nsNoiseSettings);
    }

    public void UpdateElevation(MinMax pElevationMinMax)
    {
        m_csColourSettings.m_mPlanetMaterial.SetVector("_ElevationMinMax", new Vector4(pElevationMinMax.m_fMin, pElevationMinMax.m_fMax));
    }

    public float BiomePercentFromPoint(Vector3 pPointOnUnitSphere)
    {
        float heightPercent = (pPointOnUnitSphere.y + 1) / 2f;
        heightPercent += (m_nfBiomeNoiseFilter.Evaluate(pPointOnUnitSphere) - m_csColourSettings.m_bcsBiomeColourSettings.m_fNoiseOffset)
            * m_csColourSettings.m_bcsBiomeColourSettings.m_fNoiseStrength;
        float biomeIndex = 0;
        int numBiomes = m_csColourSettings.m_bcsBiomeColourSettings.m_bBiomes.Length;
        float blendRange = m_csColourSettings.m_bcsBiomeColourSettings.m_fBlendAmount / 2f + 0.00001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float dist = heightPercent - m_csColourSettings.m_bcsBiomeColourSettings.m_bBiomes[i].m_fStartHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dist);
            biomeIndex *= 1 - weight;
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColours()
    {
        Color[] colours = new Color[m_t2dTexture.width * m_t2dTexture.height];
        int colourIndex = 0;

        foreach (var biome in m_csColourSettings.m_bcsBiomeColourSettings.m_bBiomes)
        {
            for (int i = 0; i < m_iTextureResolution * 2; i++)
            {
                Color gradientColour;
                if (i < m_iTextureResolution)
                {
                    gradientColour = m_csColourSettings.m_gOceanColour.Evaluate(i / (m_iTextureResolution - 1f));
                }
                else
                {
                    gradientColour = biome.m_gGradient.Evaluate((i - m_iTextureResolution) / (m_iTextureResolution - 1f));
                }
                Color tintColour = biome.m_cTint;
                colours[colourIndex++] = gradientColour * (1 - biome.m_fTintPercent) + tintColour * biome.m_fTintPercent;
            }
        }

        m_t2dTexture.SetPixels(colours);
        m_t2dTexture.Apply();

        m_csColourSettings.m_mPlanetMaterial.SetTexture("_texture", m_t2dTexture);
    }
}