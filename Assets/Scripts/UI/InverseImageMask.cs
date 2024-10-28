using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InverseImageMask : Image
{
    private static readonly int StencilComp = Shader.PropertyToID("_StencilComp");
    private Material maskMaterial;
    
    public override Material materialForRendering
    {
        get
        {
            if (maskMaterial == null)
            {
                maskMaterial = new Material(base.materialForRendering);
                maskMaterial.SetInt(StencilComp, (int)CompareFunction.NotEqual);
            }
            return maskMaterial;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        maskMaterial = null;
    }
}