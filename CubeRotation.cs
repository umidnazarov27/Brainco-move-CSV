using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float rotationSpeed = 1f; // Speed of rotation
    private int currentIndex = 0; // Current target rotation index
    private Quaternion targetQuaternion; // Current target rotation in quaternion
    private Vector3[] targetRotations; // Array to store rotations from the CSV file

    void Start()
    {
        // Initialize CSVDataProcessor and load data
        CSVDataProcessor CSVDATA = new CSVDataProcessor("data.csv");
        CSVDATA.LoadData();

        // Convert loaded data to targetRotations array (assuming CSVDATA.Index is int[])
        int[] rotationData = CSVDATA.Index;

        // Convert int[] to Vector3[] (assuming rotations are stored as degrees in 3D space)
        targetRotations = new Vector3[rotationData.Length];
        Debug.Log("Succesfylly");
        foreach (Vector3 rotation in targetRotations)
        {
            Debug.Log(rotation);
        }
        for (int i = 0; i < rotationData.Length; i++)
        {
            targetRotations[i] = new Vector3(0, rotationData[i], 0); // Example: Y-axis rotation
        }

        if (targetRotations.Length > 0)
        {
            // Set the first target rotation
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]);
        }
    }

    void Update()
    {
        // Debug.Log("Succesfylly Update");

        if (targetRotations == null || targetRotations.Length == 0)
            Console.WriteLine("No rotation selected");

        // Debug.Log("Succesfylly Update check");

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotationSpeed * Time.deltaTime);

        // Check if the rotation is close enough to the target
        if (Quaternion.Angle(transform.rotation, targetQuaternion) < 0.1f)
        {
            // Move to the next rotation in the array
            currentIndex = (currentIndex + 1) % targetRotations.Length;
            targetQuaternion = Quaternion.Euler(targetRotations[currentIndex]);
        }
    }
}