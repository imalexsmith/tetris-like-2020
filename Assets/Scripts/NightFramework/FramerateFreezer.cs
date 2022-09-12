using UnityEngine;

// ========================
// Revision 2020.10.11
// ========================

public class FramerateFreezer : MonoBehaviour
{
    // ========================================================================================
    public int NumberForSum = 1000000;


    // ========================================================================================
    private void Update()
    {
        if (NumberForSum > 0)
        {
            var sT = System.DateTime.Now;
            var sum = 0;

            for (int i = 0; i <= NumberForSum; i++)
                sum += i;

            print($"Frame delta time ~ {Time.deltaTime} sec.");
            print($"Time allocated for sum ~ {(System.DateTime.Now - sT).TotalSeconds} sec.");
            print($"Approximate FPS ~ {1f / Time.deltaTime}");
        }
    }
}
