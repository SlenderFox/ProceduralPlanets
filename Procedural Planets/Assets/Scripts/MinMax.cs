using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMax
{
    public float m_fMin { get; private set; }
    public float m_fMax { get; private set; }

    public MinMax()
    {
        m_fMin = float.MaxValue;
        m_fMax = float.MinValue;
    }

    public void AddValue(float pValue)
    {
        if (pValue > m_fMax)
            m_fMax = pValue;
        if (pValue < m_fMin)
            m_fMin = pValue;
    }
}