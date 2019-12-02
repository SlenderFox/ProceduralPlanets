using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };

    public bool m_bAutoUpdate = true;

    [Range(2, 255)]
    public int m_iResolution = 10;

    public FaceRenderMask m_frmFaceRenderMask;

    [Header("Settings")]
    public ShapeSettings m_ssShapeSettings;
    public ColourSettings m_csColourSettings;

    [HideInInspector]
    public bool m_bShapeSettingsFoldout = true;
    [HideInInspector]
    public bool m_bColourSettingsFoldout = true;

    private ShapeGenerator m_sgShapeGenerator = new ShapeGenerator();
    private ColourGenerator m_cgColourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector]
    private MeshFilter[] m_mfMeshFilters;
    private TerrainFace[] m_tfTerrainFaces;

    private readonly Vector3[] m_v3Directions = {
            Vector3.up, Vector3.down,
            Vector3.left, Vector3.right,
            Vector3.forward, Vector3.back
        };

    /// <summary>
    /// Initializes the planets meshes
    /// </summary>
    private void Initialize()
    {
        m_sgShapeGenerator.UpdateSettings(m_ssShapeSettings);
        m_cgColourGenerator.UpdateSettings(m_csColourSettings);

        if (m_mfMeshFilters == null || m_mfMeshFilters.Length == 0)
            m_mfMeshFilters = new MeshFilter[6];

        m_tfTerrainFaces = new TerrainFace[6];

        for (int i = 0; i < 6; i++)
        {
            if (m_mfMeshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("SideFace");
                meshObj.transform.parent = transform;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = m_csColourSettings.m_mPlanetMaterial;
                m_mfMeshFilters[i] = meshObj.AddComponent<MeshFilter>();
                m_mfMeshFilters[i].sharedMesh = new Mesh();
            }

            m_tfTerrainFaces[i] = new TerrainFace(m_sgShapeGenerator, m_mfMeshFilters[i].sharedMesh, m_iResolution, m_v3Directions[i]);
            bool renderFace = m_frmFaceRenderMask == FaceRenderMask.All || (int)m_frmFaceRenderMask - 1 == i;
            m_mfMeshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    /// <summary>
    /// Generates the whole planet
    /// </summary>
    public void GeneratePlanet(bool pOverwite)
    {
        if (m_bAutoUpdate || pOverwite)
        {
            Initialize();
            GenerateMesh();
            GenerateColours();
        }
    }

    /// <summary>
    /// Called whenever the shape settings are updated
    /// </summary>
    public void OnShapeSettingsUpdated()
    {
        if (m_bAutoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    /// <summary>
    /// Called whenever the colour settings are updated
    /// </summary>
    public void OnColourSettingsUpdated()
    {
        if (m_bAutoUpdate)
        {
            Initialize();
            GenerateColours();
        }
    }

    /// <summary>
    /// Recreates the terrain face meshes
    /// </summary>
    private void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (m_mfMeshFilters[i].gameObject.activeSelf)
            {
                m_tfTerrainFaces[i].ConstructMesh();
            }
        }

        m_cgColourGenerator.UpdateElevation(m_sgShapeGenerator.m_mmElevationMinMax);
    }

    /// <summary>
    /// Recreates the colour settings of the planet
    /// </summary>
    private void GenerateColours()
    {
        m_cgColourGenerator.UpdateColours();

        for (int i = 0; i < 6; i++)
        {
            if (m_mfMeshFilters[i].gameObject.activeSelf)
            {
                m_tfTerrainFaces[i].UpdateUVs(m_cgColourGenerator);
            }
        }
    }
}