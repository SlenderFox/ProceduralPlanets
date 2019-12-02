using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(NoiseSettings pSettings)
    {
        switch (pSettings.m_ftFilterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new SimpleNoiseFilter(pSettings.m_snsSimpleNoiseSettings);
            case NoiseSettings.FilterType.Rigid:
                return new RigidNoiseFilter(pSettings.m_rnsRigidNoiseSettings);
        }
        return null;
    }
}