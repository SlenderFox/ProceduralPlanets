using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    private Planet m_pPlanet;

    private Editor m_eShapeEditor;
    private Editor m_eColourEditor;

    /// <summary>
    /// Called whenever a change is made inside the editor
    /// </summary>
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
                m_pPlanet.GeneratePlanet(false);
        }

        if (GUILayout.Button("Generate Planet"))
            m_pPlanet.GeneratePlanet(true);

        DrawSettingsEditor(m_pPlanet.m_ssShapeSettings, m_pPlanet.OnShapeSettingsUpdated, ref m_pPlanet.m_bShapeSettingsFoldout, ref m_eShapeEditor);
        DrawSettingsEditor(m_pPlanet.m_csColourSettings, m_pPlanet.OnColourSettingsUpdated, ref m_pPlanet.m_bColourSettingsFoldout, ref m_eColourEditor);
    }

    /// <summary>
    /// Draws each of the settings in its own nested window
    /// </summary>
    /// <param name="pSettings">The specific setting drawn</param>
    /// <param name="pOnSettingsUpdated">A reference to the setting updated function</param>
    /// <param name="pFoldout">A reference to the bool that determines if the window is folded out</param>
    /// <param name="pEditor">A reference to the editor object</param>
    private void DrawSettingsEditor(Object pSettings, System.Action pOnSettingsUpdated, ref bool pFoldout, ref Editor pEditor)
    {
        if (pSettings != null)
        {
            pFoldout = EditorGUILayout.InspectorTitlebar(pFoldout, pSettings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (pFoldout)
                {
                    CreateCachedEditor(pSettings, null, ref pEditor);
                    pEditor.OnInspectorGUI();

                    if (check.changed)
                    {
                        pOnSettingsUpdated?.Invoke();
                        //Debug.Log("Change");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called when the script is enabled
    /// </summary>
    private void OnEnable() { m_pPlanet = (Planet)target; }
}