Shader "Unlit/StencilRef"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //This does geometry = 2000 - 100 = 1900 in the inspector.
        //This also makes it first in the processing queue
        Tags { "Queue"="Geometry-1" }

        //Because unity processes objects according to their position in the scene comapred ot the camera we turn zwrite off 
        zwrite off
        //Turn off the pixels for this object so it is not rendered at all.
        colormask 0 
        
        Stencil
        {
            Ref 2 // Stencil Reference.
            Comp Always //This operation will always return true
            Pass Replace //Replace the stencil buffer values with whatever the ref of this object is. 
        }
        Pass
        {
            
        }
    }
}
