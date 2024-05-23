using System;
using UnityEngine;

namespace Fractal
{
    public class Fractal : MonoBehaviour
    {
        [SerializeField, Range(1, 8)]
        private int depth = 4;

        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;

        private FractalPart[][] Parts;
        struct FractalPart
        {
            public Vector3 Position;
            public Transform Transform;
            public Quaternion Rotation;
        }
        
        private void Awake()
        {
            //Parts = new FractalPart[depth][];
            //
            //CreatePart(0);
            ////*= 5 is because we have forward,back,left,right,up directions
            //for (int i = 0, length = 1; i < Parts.Length; i++, length *= 5)
            //{
            //    Parts[i] = new FractalPart[length];
            //    FractalPart[] depthArray = Parts[i];
            //    for (int j = 0; j < depthArray.Length; j++)
            //    {
            //        CreatePart(i);
            //    }
            //}
        }

        void Start()
        {
            name = "Fractal " + depth;
            //Stop infinite recursion
            if (depth <= 1)
                return;
        
            Fractal childA = CreateFractalChild(Vector3.up, Quaternion.identity);
            Fractal childB = CreateFractalChild(Vector3.right, Quaternion.Euler(0,0,-90f));
            Fractal childC = CreateFractalChild(Vector3.left, Quaternion.Euler(0,0,90f));
            Fractal childD = CreateFractalChild(Vector3.forward, Quaternion.Euler(90f,0,0));
            Fractal childE = CreateFractalChild(Vector3.back, Quaternion.Euler(-90f,0,0));
        
            childA.transform.SetParent(transform, false);
            childB.transform.SetParent(transform, false);
            childC.transform.SetParent(transform, false);
            childD.transform.SetParent(transform, false);
            childE.transform.SetParent(transform, false);
        }

        private Fractal CreateFractalChild(Vector3 aSpawnDirection, Quaternion aRotation)
        {
            Fractal child;
            child = Instantiate(this);
            child.depth = depth - 1; 
            child.transform.rotation = aRotation;
            child.transform.localPosition = 0.75f * aSpawnDirection;
            child.transform.localScale = 0.5f * Vector3.one;
            return child;
        }

        //private void CreatePart(int levelIndex)
        //{
        //    GameObject newGameObject = new GameObject("Fractal Part " + levelIndex);
        //    newGameObject.transform.SetParent(transform, false);
        //    
        //    MeshFilter meshFilter = newGameObject.AddComponent<MeshFilter>();
        //    meshFilter.mesh = mesh;
        //    
        //    MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer >();
        //    meshRenderer.material = material;
        //}

        void Update()
        {
            transform.Rotate(0f,22.5f * Time.deltaTime, 0f);
        }
    }
}
