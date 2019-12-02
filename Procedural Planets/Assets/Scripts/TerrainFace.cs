using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    ShapeGenerator m_sgShapeGenerator;
    private Mesh m_mMesh;
    private int m_iResolution;
    private Vector3 m_v3LocalUp;
    private Vector3 m_v3AxisA;
    private Vector3 m_v3AxisB;

    /// <summary>
    /// Constructor; Creates a flat mesh in one of 6 directions.
    /// </summary>
    /// <param name="pMesh">The mesh used for the terrain face</param>
    /// <param name="pResolution">The resolution of the terrain face mesh</param>
    /// <param name="pLocalUp">The up direction for the terrain face</param>
    public TerrainFace(ShapeGenerator pShapeGenerator, Mesh pMesh, int pResolution, Vector3 pLocalUp)
    {
        m_sgShapeGenerator = pShapeGenerator;
        m_mMesh = pMesh;
        m_iResolution = pResolution;
        m_v3LocalUp = pLocalUp;

        m_v3AxisA = new Vector3(m_v3LocalUp.y, m_v3LocalUp.z, m_v3LocalUp.x);
        m_v3AxisB = Vector3.Cross(m_v3LocalUp, m_v3AxisA);
    }

    /// <summary>
    /// Creates the vertices and triangles using the localup
    /// </summary>
    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[m_iResolution * m_iResolution];
        int[] triangles = new int[(m_iResolution - 1) * (m_iResolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (m_mMesh.uv.Length == vertices.Length) ? m_mMesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < m_iResolution; y++)
        {
            for (int x = 0; x < m_iResolution; x++)
            {
                int i = x + y * m_iResolution;
                Vector2 percent = new Vector2(x, y) / (m_iResolution - 1);
                Vector3 pointOnUnitCube = m_v3LocalUp
                    + (percent.x - 0.5f) * 2 * m_v3AxisA
                    + (percent.y - 0.5f) * 2 * m_v3AxisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                float unscaledElevation = m_sgShapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * m_sgShapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                if (x != m_iResolution - 1 && y != m_iResolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + m_iResolution + 1;
                    triangles[triIndex + 2] = i + m_iResolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + m_iResolution + 1;
                    triIndex += 6;
                }
            }
        }
        // Recalibrate the mesh
        m_mMesh.Clear();
        m_mMesh.vertices = vertices;
        m_mMesh.triangles = triangles;
        m_mMesh.RecalculateNormals();
        m_mMesh.uv = uv;
    }

    public void UpdateUVs(ColourGenerator pColourGenerator)
    {
        Vector2[] uv = m_mMesh.uv;

        for (int y = 0; y < m_iResolution; y++)
        {
            for (int x = 0; x < m_iResolution; x++)
            {
                int i = x + y * m_iResolution;
                Vector2 percent = new Vector2(x, y) / (m_iResolution - 1);
                Vector3 pointOnUnitCube = m_v3LocalUp
                    + (percent.x - 0.5f) * 2 * m_v3AxisA
                    + (percent.y - 0.5f) * 2 * m_v3AxisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i].x = pColourGenerator.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }

        m_mMesh.uv = uv;
    }
}