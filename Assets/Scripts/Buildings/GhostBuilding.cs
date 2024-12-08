using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuilding : MonoBehaviour
{
    private Transform visual;
    private ConstructionType constructionType;
    public float ghostOpacity = 0.5f;

    void Start()
    {
        if (GridBuildingSystem.Instance == null)
        {
            Debug.LogError("GridBuildingSystem.Instance is null. Ensure it's initialized correctly.");
            return;
        }

        RefreshVisual();
        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        RefreshVisual();
    }

    private void LateUpdate()
    {
        if (GridBuildingSystem.Instance == null) return;

        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetConstructionRotation(), Time.deltaTime * 15f);

        if (constructionType != GridBuildingSystem.Instance.GetConstructionType())
        {
            RefreshVisual();
        }
    }

    private void RefreshVisual()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        constructionType = GridBuildingSystem.Instance.GetConstructionType();

        if (constructionType != null)
        {
            visual = Instantiate(constructionType.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;

            // Reduz a opacidade do visual
            SetVisualOpacity(ghostOpacity);
        }
        else
        {
            Debug.LogWarning("No construction type selected in GridBuildingSystem.");
        }
    }

    private void SetVisualOpacity(float opacity)
    {
        // Aplica a opacidade ao material da visualização
        Renderer[] renderers = visual.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials)
            {
                Color color = mat.color;
                color.a = opacity;
                mat.color = color;

                // Configurar a opacidade da sprite
                mat.SetFloat("_Mode", 3);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }
    }
}
