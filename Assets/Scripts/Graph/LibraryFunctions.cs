using UnityEngine;
using static UnityEngine.Mathf;
namespace Graph
{
    public static class LibraryFunctions
    {
        public enum WaveFunctions {Wave, MultiWave, Ripple, Sphere, Torus}

        public static GraphFunction GetFunction(WaveFunctions function) => functions[(int)function];
        public delegate Vector3 GraphFunction(float u, float v, float time);

        private static GraphFunction[] functions = { Wave, MultiWave, Ripple, Sphere, Torus};
        
        private static Vector3 Wave(float u, float v, float time)
        {
            Vector3 p;
            p.x = u;
            p.z = v;
            
            //Simple Sine wave
            p.y = Sin(PI * (u + v + time));
            return p;
        }
        private static Vector3 MultiWave(float u, float v, float time)
        {
            Vector3 p;
            p.x = u;
            p.z = v;
            //Get a simple sin wave and add another sine wave ontop of it.
            //this creates an effect where the the 2nd sine wave is sliding ontop of the previous.
            //because the 2nd sine wave is occuring at twice the rate and we have two sine waves, we limit
            //to -1, 1 by dividing the result by 1.5 as its max amplitude is 1.5.
            p.y = Sin(PI * (u + time));
            //To add something interesting we add the z position just to the second sine wave.
            p.y += Sin(2f * PI * (v + time)) * 0.5f;
            //add a third wave with x and z but have its timing a quarter frequency
            p.y += Sin(PI * (v + u + time * 0.25f));
            p.y *= (1f / 2.5f);
            return p;
        }
        private static Vector3 Ripple(float u, float v, float time)
        {
            Vector3 p;
            p.x = u;
            p.z = v;
            //Abs so its always the same distance on either side, since we have mirrored values we divide its final
            //result so that the amplitude is not massive and unreadable, by having 10*d division it also ensures the value
            //is reduced the further away the point is.
            //float d = Abs(a_X);

            //Get distance from 0,0,0
            float d = Sqrt(u * u + v * v);
            p.y = Sin(4f * PI * (d - time));
            p.y /= (1f + 10f * d);
            return p;
        }

        //I understand roughly the way this sphere works but got confused by the torus explanation once it got to intersecting radius.
        //Didnt want to get hung up in tutorial hell so for now it has just been followed.
        private static Vector3 Sphere(float u, float v, float time)
        {
            Vector3 p;
            float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + time));
            float s = r * Cos(0.5f * PI * v);
            p.x = s * Sin(PI * u);
            p.z = s * Cos(PI * u);
            p.y = r * Sin(PI * 0.5f * v);

            return p;
        }
        private static Vector3 Torus(float u, float v, float time)
        {
            Vector3 p;
            //Tutorial creates two radii, not sure how the math there works but due to having two radii
            //it is then possible to have a variable radius on both radii causing the torus to twist.
            //float r1 = 0.75f;
            //float r2 = 0.25f;
            
            float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * time));
            float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * time));
            
            float s = r1 + r2 * Cos(PI * v);
            p.x = s * Sin(PI * u);
            p.y = r2 * Sin(PI * v);
            p.z = s * Cos(PI * u);

            return p;
        }

        public static Vector3 LerpTwoPoints(float u, float v, float t, GraphFunction from, GraphFunction to, float progress)
        {
            return Vector3.Lerp(from(u, v, t), to(u,v,t), SmoothStep(0f,01, progress));
        }
    }
}