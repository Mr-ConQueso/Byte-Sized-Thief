using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyControl : MonoBehaviour
{
    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        // Create a ray from the camera towards the object
        //Ray ray = new Ray(Camera.main.transform.position, _transform.position - Camera.main.transform.position);
        Ray ray = new Ray(_transform.position, Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);

        // Cast the ray and get all hits
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, 200);

        // Loop through all hits
        foreach (RaycastHit hit in hits)
        {
            Renderer hitRenderer = hit.collider.GetComponent<Renderer>();

            if (hitRenderer != null)
            {
                // Access the material and switch it to transparent mode
                Material material = hitRenderer.material;

                // Switch the rendering mode to transparent
                SetMaterialToTransparent(material);

                // Change the alpha (transparency)
                Color color = material.color;
                color.a = 0.5f;  // Adjust the transparency level
                material.color = color;
            }
        }
    }

    // Method to set the material to transparent mode
    void SetMaterialToTransparent(Material material)
    {
        material.SetFloat("_Mode", 3); // Set mode to Transparent (3)
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000; // Transparent queue
    }
}
