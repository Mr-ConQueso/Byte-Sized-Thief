using UnityEngine;
using UnityEngine.Rendering;
using Image = UnityEngine.UI.Image;

public class CircleMaskAnimation : Image
{
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
