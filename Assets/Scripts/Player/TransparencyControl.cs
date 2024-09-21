using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyControl : MonoBehaviour
{
    [SerializeField] private LayerMask Translucent;
    [Range(0,1)]
    [SerializeField] private float transparencyLevel;
    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(_transform.position);

        // Create a ray from the camera, through the screen point
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        // Cast the ray and get all hits
        RaycastHit[] hits = Physics.RaycastAll(ray, 200, Translucent);

        // Loop through all hits
        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null)
            {
                // Access the material and switch it to transparent mode
                Material material = hitRenderer.material;

                // Switch the rendering mode to transparent in URP
                SetMaterialToTransparentURP(material);

                // Change the alpha (transparency)
                Color color = material.color;
                color.a = transparencyLevel;  // Adjust the transparency level (you can change this value)
                material.color = color;
            }
        }
    }

    // Method to set the material to transparent mode in URP
    void SetMaterialToTransparentURP(Material material)
    {
        // Set the surface type to transparent
        material.SetFloat("_Surface", 1.0f);  // 1 = Transparent, 0 = Opaque
        
        // Enable the shader keyword for transparent materials
        material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

        // Set blend modes
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);  // Disable ZWrite for transparency

        // Make sure transparency is rendered correctly in URP
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
}
