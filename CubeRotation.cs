using System;
using System.Collections;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float rotationSpeed = 15f; // Speed of the cube's smooth rotation.
    private int currentIndex = 0; // Index to track the current target rotation.
    private Vector3[] targetRotations; // Array to store target rotations read from the CSV file.

    private Quaternion targetQuaternion; // Target rotation for smooth interpolation.

    void Start()
    {
        // Initialize the CSVDataProcessor with the path to the CSV file.
        var CSVDATA = new CSVDataProcessor("data.csv");

        // Load the rotation data from the CSV file into the processor.
        CSVDATA.LoadData();

        // Retrieve the loaded rotation data (assuming it's stored as an integer array).
        int[] rotationData = CSVDATA.Index;

        // Log the length of the data to verify successful loading.
        Debug.Log($"Loaded {rotationData.Length} rotation values from the CSV file.");

        // Convert the integer array to a Vector3 array for storing 3D rotation values.
        targetRotations = new Vector3[rotationData.Length];

        for (int i = 0; i < rotationData.Length; i++)
        {
            // Assign each rotation value to the Y-axis (as an example) and set X and Z to 0.
            targetRotations[i] = new Vector3(0, rotationData[i], 0);
        }

        // If rotations exist, set the first target rotation and start the coroutine.
        if (targetRotations.Length > 0)
        {
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]);
            StartCoroutine(UpdateCubeRotationSmoothly());
        }
        else
        {
            Debug.LogError("No rotation data found. Ensure the CSV file contains valid rotation values.");
        }
    }

    private IEnumerator UpdateCubeRotationSmoothly()
    {
        while (true)
        {
            // Set the target quaternion for the current rotation.
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]);

            // Interpolate to the target rotation over 1 second.
            float elapsedTime = 0f;
            Quaternion initialRotation = transform.rotation;

            while (elapsedTime < 1f) // Smoothly interpolate for 1 second.
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / 1f; // Normalized time value (0 to 1).
                transform.rotation = Quaternion.Slerp(initialRotation, targetQuaternion, t);

                yield return null; // Wait for the next frame.
            }

            // Ensure the final rotation is set to the exact target.
            transform.rotation = targetQuaternion;

            // Log the current rotation index for debugging.
            Debug.Log($"Smoothly set cube to rotation index {currentIndex}: {targetRotations[currentIndex]}");

            // Move to the next rotation in the array, wrapping back to the start when at the end.
            currentIndex = (currentIndex + 1) % targetRotations.Length;
        }
    }
}
