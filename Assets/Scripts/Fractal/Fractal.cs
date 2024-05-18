using UnityEngine;

namespace Fractal
{
    public class Fractal : MonoBehaviour
    {
        [SerializeField, Range(1, 8)]
        private int depth = 4;

        [SerializeField] private Mesh mesh;
        [SerializeField] private Material material;
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

        void Update()
        {
            transform.Rotate(0f,22.5f * Time.deltaTime, 0f);
        }
    }
}
