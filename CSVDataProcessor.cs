using System;
using System.IO;
using System.Globalization;
using CsvHelper;

public class CSVDataProcessor
{
    // Path to the CSV file
    private string filePath;

    // Number of rows (data entries) in the CSV file
    private int rowCount;

    // Arrays to store data for each finger's flexion
    public int[] Index { get; private set; }
    public int[] Middle { get; private set; }
    public int[] Ring { get; private set; }
    public int[] Pinky { get; private set; }

    /// <summary>
    /// Constructor for CSVDataProcessor.
    /// Initializes the file path, calculates row count, and prepares arrays for data storage.
    /// </summary>
    /// <param name="relativeFilePath">Relative path to the CSV file.</param>
    public CSVDataProcessor(string relativeFilePath)
    {
        // Set the file path based on the given relative path and navigate up three levels from the current working directory.
        filePath = Path.Combine(
            Directory.GetParent(Directory.GetCurrentDirectory())?.Parent.Parent?.FullName ?? string.Empty,
            relativeFilePath
        );

        // Calculate the number of rows in the CSV file and initialize arrays to hold data for each finger.
        rowCount = GetRowCount();
        Index = new int[rowCount];
        Middle = new int[rowCount];
        Ring = new int[rowCount];
        Pinky = new int[rowCount];
    }

    /// <summary>
    /// Reads the CSV file to count the number of data rows (excluding header).
    /// </summary>
    /// <returns>Number of data rows in the CSV file.</returns>
    private int GetRowCount()
    {
        int count = 0;  // Initialize row count

        try
        {
            // Open the CSV file in read mode
            using (var reader = new StreamReader(filePath))
            {
                // Skip the header line
                reader.ReadLine();

                // Count each remaining line as one data row
                while (reader.ReadLine() != null) count++;
            }
        }
        catch (FileNotFoundException ex)
        {
            // Handle the case where the file is not found
            Console.WriteLine($"Error: The file {filePath} was not found.");
            Console.WriteLine($"{ex.Message}");
        }

        return count;
    }

    /// <summary>
    /// Loads data from the CSV file into the arrays for each finger.
    /// </summary>
    public void LoadData()
    {
        try
        {
            // Open the CSV file in read mode
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read the header row to identify columns
                csv.Read();
                csv.ReadHeader();

                int i = 0;  // Index to store each row's data in the arrays

                // Loop through each row in the CSV file
                while (csv.Read())
                {
                    // Read and store data from each column for the specific row
                    Index[i] = csv.GetField<int>("index");
                    Middle[i] = csv.GetField<int>("middle");
                    Ring[i] = csv.GetField<int>("ring");
                    Pinky[i] = csv.GetField<int>("pinky");
                    i++;  // Move to the next row
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            // Handle the case where the file is not found
            Console.WriteLine($"Error: The file {filePath} was not found.");
            Console.WriteLine($"{ex.Message}");
        }
        catch (CsvHelperException ex)
        {
            // Handle errors related to CSV parsing (e.g., format issues)
            Console.WriteLine("CSV Parsing Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected errors
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
