using UnityEngine;
using static UnityEngine.Mathf;
namespace Graph
{
    public static class LibraryFunctions
    {
        public enum WaveFunctions {Wave, MultiWave, Ripple}

        public delegate float Function(float a_X, float a_Time);
        public static float Wave(float a_XPos, float a_Time)
        {
            //Simple Sine wave
            return Sin(PI * (a_XPos + a_Time));
        }
        public static float MultiWave(float a_X, float a_TimeOffset, float a_Time)
        {
            //Get a simple sin wave and add another sine wave ontop of it.
            //this creates an effect where the the 2nd sine wave is sliding ontop of the previous.
            //because the 2nd sine wave is occuring at twice the rate and we have two sine waves, we limit
            //to -1, 1 by dividing the result by 1.5 as its max amplitude is 1.5.
            float y = Sin(PI * (a_X + a_TimeOffset * a_Time));
            y += Sin(2f * PI * (a_X + a_Time)) * 0.5f;
            return y / 1.5f;
        }
        public static float Ripple(float a_X, float a_Time)
        {
            //Abs so its always the same distance on either side, since we have mirrored values we divide its final
            //result so that the amplitude is not massive and unreadable, by having 10*d division it also ensures the value
            //is reduced the further away the point is.
            float d = Abs(a_X);
            float y = Sin(4f * PI * (d - a_Time));
            return y / (1f + 10f * d);
        }
    }
}