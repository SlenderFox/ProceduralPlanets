using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSettings m_ssShapeSettings;
    private INoiseFilter[] m_nfNoiseFilters;
    public MinMax m_mmElevationMinMax;

    /// <summary>
    /// Constructs the shape generator with the desired settings
    /// </summary>
    /// <param name="pShapeSettings">The settings used for the shape generator</param>
    public void UpdateSettings(ShapeSettings pShapeSettings)
    {
        m_ssShapeSettings = pShapeSettings;
        m_nfNoiseFilters = new INoiseFilter[m_ssShapeSettings.m_nlNoiseLayers.Length];
        m_mmElevationMinMax = new MinMax();

        for (int i = 0; i < m_nfNoiseFilters.Length; i++)
            m_nfNoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(pShapeSettings.m_nlNoiseLayers[i].m_nsNoiseSettings);

    }

    /// <summary>
    /// Scales the unit sphere by a desired radius
    /// </summary>
    /// <param name="pPointOnUnitSphere">The point used for scaling</param>
    /// <returns>The new scaled value</returns>
    public float CalculateUnscaledElevation(Vector3 pPointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (m_nfNoiseFilters.Length > 0)
        {
            firstLayerValue = m_nfNoiseFilters[0].Evaluate(pPointOnUnitSphere);
            if (m_ssShapeSettings.m_nlNoiseLayers[0].m_bEnabled)
                elevation = firstLayerValue;
        }

        for (int i = 1; i < m_nfNoiseFilters.Length; i++)
        {
            if (m_ssShapeSettings.m_nlNoiseLayers[i].m_bEnabled)
            {
                float mask = (m_ssShapeSettings.m_nlNoiseLayers[i].m_bUseFirstLayerAsMask) ? firstLayerValue : 1;
                elevation += m_nfNoiseFilters[i].Evaluate(pPointOnUnitSphere) * mask;
            }
        }
        m_mmElevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float pUnscaledElevation)
    {
        float elevation = Mathf.Max(0, pUnscaledElevation);
        elevation = m_ssShapeSettings.m_fPlanetRadius * (1 + elevation);
        return elevation;
    }
}