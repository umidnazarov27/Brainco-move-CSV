using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A script to control the rotation of a Cube object based on data from a CSV file.
public class CubeRotation : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed at which the cube rotates (in degrees per second).
    private int currentIndex = 0; // Index to track the current target rotation in the array.
    private Quaternion targetQuaternion; // Target rotation, represented as a quaternion.
    private Vector3[] targetRotations; // Array to store target rotations read from the CSV file.

    void Start()
    {
        // Initialize the CSVDataProcessor with the path to the CSV file.
        var CSVDATA = new CSVDataProcessor("data.csv");

        // Load the rotation data from the CSV file into the processor.
        CSVDATA.LoadData();

        // Retrieve the loaded rotation data (assuming it's stored as an integer array).
        int[] rotationData = CSVDATA.Index;

        // Log the length of the data to verify successful loading.
        Debug.Log(rotationData.Length);
        Debug.Log("Successfully Loaded Data");

        // Convert the integer array to a Vector3 array for storing 3D rotation values.
        targetRotations = new Vector3[rotationData.Length];

        for (int i = 0; i < rotationData.Length; i++)
        {
            // Assign each rotation value to the Y-axis (as an example) and set X and Z to 0.
            targetRotations[i] = new Vector3(0, rotationData[i], 0);
        }

        // If there are rotations available, set the first target rotation.
        if (targetRotations.Length > 0)
        {
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]); // Convert the first Vector3 to Quaternion.
        }

        // Log all target rotations to verify the conversion.
        foreach (Vector3 rotation in targetRotations)
        {
            Debug.Log(rotation);
        }
        Debug.Log("Successfully Converted to Vectors");
    }

    void Update()
    {
        // Ensure that the rotation data is available before proceeding.
        if (targetRotations == null || targetRotations.Length == 0)
        {
            Console.WriteLine("No rotation selected"); // Console log if no rotations are available.
            return; // Exit the update loop to prevent errors.
        }

        // Log to confirm the update loop is running.
        Debug.Log("Successfully Entered Update Loop");

        // Smoothly interpolate the cube's current rotation to the target rotation using Slerp.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);

        // Check if the current rotation is close enough to the target rotation.
        if (Quaternion.Angle(transform.rotation, targetQuaternion) < 0.1f) // Threshold angle to consider "reached".
        {
            // Move to the next rotation in the array, wrapping back to the start when at the end.
            currentIndex = (currentIndex + 1) % targetRotations.Length;

            // Update the target quaternion for the new rotation.
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]);
        }
    }
}
