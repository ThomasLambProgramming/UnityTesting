Shader "Unlit/SimpleColor"
{
    
    Properties
    {
        //PropertyName ("DisplayNameInInspector", type = defaultValue
        //Prop must end with {}, it remains open within the property field so no ; or it will not work.
        
        //2d texture
        _MainTex ("Texture", 2D) = "white" {}
        //Cubemap
        _MainTex1 ("Texture1", Cube) = "black" {}
        //3d Texture (not used very often can probably skip over.)
        _MainTex2 ("Texture2", 3D) = "white" {}
        
        //Dont seem to need use floats, and that floats dont need the {} at the end 
        //for some reason but at same time texture doesnt need it but can have it there and floats cant?
        Testing ("BrightnessAmount", Range(0.2, 1.8)) = 0.3 
        Testing1 ("BrightnessAmount1", float) = 0.3
        //Int seems to allow for float but will probably default to rounded or error. 
        //int and Int seem to be the same, also unity seems to default ints to be floats
        Testing2 ("BrightnessAmount2", Int) = 1  
       
        //Again having vector and Vector doesnt make a difference to unity.
        VecPos ("Vector Position", vector) = (0,0,0,0)
        
        //Color is in 0-1 range. rgba
        MatColor ("Material Color", color) = (0.5,0.2,0.7,1)
       
        //Adding the toggle here makes it so that the float is turned into a checkbox in the inspector. 
        //0 is false, 1 is true.
        //Shaderlab doesnt support bools so we have the float/int 01 alternative.
        [Toggle] BoolAlternative ("Bool Alternative", float) = 0
        //This toggle seems to work similar to a define to use #if statements
        //declare the pragma shader inbetween the CGPROGRAM
        //#pragma shader_feature _Ithinkvarnamehere_ON
        /*
        //Must be all caps regardless as it is a constant and the _ON is the default state
            #if VARNAME_ON
                do dis
            #else
                do dis
            #endif
        So pretty much the same as the #define.
        */
        //Something to consider when working with this drawer is that, if you want to implement it you 
        //have to use the #pragma shader_feature. This belongs to the Shader Variants and its function 
        //is to generate different conditions according to its state (enabled or disabled).
        //Only one of the #pragma shader_feature will compile, all the others will not.
        
        
        //Enums, same thing as the above but with an enum selection.
        //However, pragma shader feature will only export the selected variant, but #pragma multi_compile will export all of them 
        //even if they arent used.
        //BUT #pragma multi_compile DISPLAINGNAME_ENUMONE DISPLAINGNAME_ENUMTWO DISPLAINGNAME_ENUMTHREE
        //Then we can just do #if DISPLAINGNAME_ENUMONE and etc.
        [KeywordEnum(EnumOne, EnumTwo, EnumThree)]
        EnumThing ("DisplaingName", float) = 0
        
        
        //This doesnt use shader variants (#pragma) but are declared by command or function, eg a Cull[Face] command before
        //the pass, seems as though you can define a value for a named thing which is nice. this example has the values
        //set so we can choose what side of the object will be rendered.
        [Enum(Off, 0, Front, 1, Back, 2)]
        Face("FaceCulling", float) = 0
        
        
        //These do exactly what you think they would. need to ask alex about the purpose of the Powerslider
        [Header(CategoryName)]
        [Space(20)]
        //the value given to the power slider seems to change how it moves up and down. very odd.
        [PowerSlider(3)] FloatRange ("Range thingy float power!", Range (0.3, 500)) = 0.4
        [IntRange] IntRange ("Range thingy int power!", Range (1, 500)) = 1 
        
        
        //This is somehow overriding the below blend, not sure how that works but ok (i spelt destination wrong first time so its not the "" name
        //[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Source Factor", Float) = 1
        //[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Destination Factor", Float) = 1
    }
    SubShader
    {
        //Tags are labels that show how and when the shader is being processed, this can be used to see how it will
        //be rendered or how a group of shaders will behave graphically.
        //Can be done in the subshader or pass (in pass will only affect that pass otherwise in subshader will affect all).
        Tags
        { 
            //"TagName1"="TagValue1"
            
            
            "RenderType"="Opaque"
            
            //Queue defines how the surface of the object will look, default for all volume is geometry that meaning it has no transparency
            //Its function is directly related to the object processing order for each material.
            "Queue"="Geometry"
        }
        //This is a facing function or something, dont really know yet havent looked it up.
        Cull [EnumThing]
        //AlphaToMask On
        
        //Can only place in subshader or pass
        //Blend OneMinusSrcColor Zero
        //Blend =   SourceFactor(vec3), DstFactor(vec3) both are colors rgb (there are predefined factors for unity as well)
        //          SourceFactor * 
        //          SrcValue (Source value = fragment shader stage result / pixels rgb color output)
        //          [OP]  (operation to perform default is add)
        //          DstFactor * 
        //          DstValue (Corresponds to the rgb color that has been written in the desination buffer, otherwise known as the RenderTarge (SV_Target)
        // Alpha blending does happen but it is separated due to its non use all the time so its faster to have it separate and only when its needed.
        // Render target is a gpu feature which allows a scene to be rendered in an intermediate memory buffer, forward rendering uses a render target by default
        // deferred shading uses multiples of them.
        Pass
        {
            
            //Make the shader only render the rgba values that we allow
            ColorMask RGBA
            
            CGPROGRAM

        
            #pragma shader_feature BOOLALTERNATIVE_ON
            #pragma shader_feature TESTINGSHIT
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            //There must be connector variables, as properties seems to be just for unity inspector
            //but the below is actual shader variables. *Must be named the same.
            sampler2D _MainTex;
            float4 MatColor;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                    return col * MatColor;
                #if BOOLALTERNATIVE_ON
                    return MatColor;
                #else
                #endif
                
                // apply fog
            }
            
            ENDCG
        }
    } 
    Fallback "Standard" 
}
