using System;
using UnityEngine;

public class MaterialChangeScript : MonoBehaviour
{
    public static Action changeToStandartMatDelegate;

    private Material highlightMaterial;
    private Material standartMaterial;
    private Material yellowMaterial;


    private MeshRenderer meshRenderer;

    [SerializeField]
    private GameObject go;

    private void OnEnable()
    {
        changeToStandartMatDelegate += ChangeToStandartMaterial;
    }

    private void OnDisable()
    {
        changeToStandartMatDelegate -= ChangeToStandartMaterial;
    }

    private void Start()
    {
        highlightMaterial = Resources.Load<Material>("HighlightMaterial");
        yellowMaterial = Resources.Load<Material>("YellowMaterial");
        meshRenderer = go.GetComponent<MeshRenderer>();
        standartMaterial = meshRenderer.material;
    }

    public void ChangeMaterial()
    {
        meshRenderer.material = highlightMaterial;
    }

    private void ChangeToStandartMaterial()
    {
        if (GlobalContainer.GetInstance().Cataclysm == Cataclysm.Earthquake && name.Contains("0") && !name.Contains("0,0"))
        {
            if (meshRenderer != null) meshRenderer.material = yellowMaterial;
        }
        else
        {
            if (meshRenderer != null) meshRenderer.material = standartMaterial;
        }
    }
}
