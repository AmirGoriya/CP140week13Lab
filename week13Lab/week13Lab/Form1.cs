/* 
 * ****************************************************************************
 * 
 * Name: Amir Goriya
 * Lab: Week 13
 * Date: 12/01/2017
 * Purpose: This form allows the user to load students grade data for three separate...
 * ...sections from three separate files, and outputs useful information such as...
 * ...averages, lowest, and highest scores, etc.
 * 
 * *****************************************************************************
 * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Required namespace for StreamReader and StreamWriter

namespace week13Lab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Function executed upon clicking "Exit"
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close(); // Exits the form
        }

        // Function executed upon clicking "Load Data"
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            // Necessary StreamReader and StreamWriter objects instantiated
            StreamReader fileSection1 = new StreamReader("./section1.txt");
            StreamReader fileSection2 = new StreamReader("./section2.txt");
            StreamReader fileSection3 = new StreamReader("./section3.txt");

            StreamWriter fileGrades = new StreamWriter("./results.txt");

            // Obtain the sizes necessary 
            int sizeSection1 = GetFileSize(fileSection1);
            int sizeSection2 = GetFileSize(fileSection2);
            int sizeSection3 = GetFileSize(fileSection3);
            int sizeSectionAll = sizeSection1 + sizeSection2 + sizeSection3;

            // Instantiate the necessary jagged array to hold the grades
            int[][] gradesSectionAll = new int[3][]; // This jagged array has 3 rows

            // Fill each row of the jagged array with contents from each corresponding file
            gradesSectionAll[0] = PopulateArray(fileSection1, sizeSection1);
            gradesSectionAll[1] = PopulateArray(fileSection2, sizeSection2);
            gradesSectionAll[2] = PopulateArray(fileSection3, sizeSection3);

            // Fill each listbox with the contents from each corresponding jagged array row
            AddToListbox(gradesSectionAll[0], lboxSection1);
            AddToListbox(gradesSectionAll[1], lboxSection2);
            AddToListbox(gradesSectionAll[2], lboxSection3);

            // Instantiates a list to hold all sections' grades (by calling a function)
            List<int> gradesSectionAllList = PopulateList(gradesSectionAll);
            gradesSectionAllList.RemoveAt(12); // Removes the 13th element in the list currently
            gradesSectionAllList.RemoveAt(26); // Removes the 27th element in the list currently

            // Adds the contents of the list created above to its corresponding listbox
            AddToListbox(gradesSectionAllList, lboxSectionAll);

            // Writes the contents of the list object (grades of all sections minus the two removed...
            // ...elements) to an output file (results.txt) and then closes the output file.
            WriteResults(fileGrades, gradesSectionAllList);
            fileGrades.Close();

            // Calls the functions to obtain the averages for each section and all sections...
            // ...and assigns them to their corresponding read-only textboxes.
            tboxAvgScrSection1.Text = (GetAverage(gradesSectionAll[0])).ToString();
            tboxAvgScrSection2.Text = (GetAverage(gradesSectionAll[1])).ToString();
            tboxAvgScrSection3.Text = (GetAverage(gradesSectionAll[2])).ToString();
            tboxAvgScrSectionAll.Text = (GetAverage(gradesSectionAll)).ToString();

            // Creates strings to hold which sections have the highest and lowest score (individual scores only)
            string whichSectionsHighest = "";
            string whichSectionsLowest = "";

            // Calls the functions to obtain highest and lowest scores and assigns them to their...
            // ...corresponding text labels.
            tboxHighScrSectionAll.Text = (GetHighest(gradesSectionAll, out whichSectionsHighest)).ToString();
            tboxHighScoreSections.Text = whichSectionsHighest;
            tboxLowScrSectionAll.Text = (GetLowest(gradesSectionAll, out whichSectionsLowest)).ToString();
            tboxLowScoreSections.Text = whichSectionsLowest;
        }

        /// <summary>
        /// Returns the number of non-empty lines in a file.
        /// </summary>
        /// <param name="inputFile">An input file</param>
        /// <returns>(int) number of non-empty lines in the input file</returns>
        private int GetFileSize(StreamReader inputFile)
        {
            int nonEmptyLines = 0; // Counter variable
            
            // Loop ends when the cursor reaches the end of the file.
            while (!inputFile.EndOfStream)
            {
                if (!(inputFile.ReadLine() == null))
                {
                    nonEmptyLines++; // Counter goes up if the current line is not empty
                }
                
            }
            inputFile.BaseStream.Position = 0; // Resets the cursor to the beginning of the file for future reading
            return nonEmptyLines;
        }

        /// <summary>
        /// Obtains data from the input file and fills the array with it.
        /// </summary>
        /// <param name="inputFile">The input file (must only have numerical data)</param>
        /// <param name="returnArraySize">The size of the array being made</param>
        /// <returns>(int[]) An integer array filled with data from the input file</returns>
        private int[] PopulateArray(StreamReader inputFile, int returnArraySize)
        {
            int[] numbers = new int[returnArraySize];

            // Loop counts up to the size of the array that user wants returned
            for (int i = 0; i < returnArraySize; i++)
            {
                try // Catches any non-numerical/bad data in the file
                {
                    // Parses current line in the input file to int and copies it to the...
                    // ...current array element
                    numbers[i] = int.Parse(inputFile.ReadLine());
                }
                catch
                {
                    MessageBox.Show("Invalid/non-numerical data found in file. Please check file.");
                }
            }
            return numbers;
        }

        /// <summary>
        /// Fills a listbox with numbers from an integer array.
        /// </summary>
        /// <param name="numbers">An integer array</param>
        /// <param name="outputListBox">The listbox to be filled</param>
        private void AddToListbox(int[] numbers, ListBox outputListBox)
        {
            // Fills a listbox with the contents of the array
            for (int i = 0; i < numbers.Length; i++)
            {
                outputListBox.Items.Add(numbers[i]);
            }
        }

        /// <summary>
        /// Copies data from a jagged array to an integer list object.
        /// </summary>
        /// <param name="numbers">A jagged array</param>
        /// <returns>(List<int>) An integer list</returns>
        private List<int> PopulateList(int[][] numbers)
        {
            List<int> intList = new List<int>();

            // Fills intList with the contents of the jagged array "numbers"
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++)
                {
                    intList.Add(numbers[i][j]);
                }
            }
            return intList;
        }

        /// <summary>
        /// Fills a listbox with numbers from an integer list.
        /// </summary>
        /// <param name="inputList">An input integer list</param>
        /// <param name="outputListBox">The listbox to be filled</param>
        private void AddToListbox(List<int> inputList, ListBox outputListBox)
        {
            // Fills the outputListBox with contents of the inputList
            foreach (int number in inputList)
            {
                outputListBox.Items.Add(number.ToString());
            }
        }

        /// <summary>
        /// Writes the contents of an integer list to an output file.
        /// </summary>
        /// <param name="outputFile">The file to be written in</param>
        /// <param name="inputList">TAn input ingeter list</param>
        private void WriteResults(StreamWriter outputFile, List<int> inputList)
        {
            // Writes all the numbers in inputList to outputFile
            foreach (int number in inputList)
            {
                outputFile.WriteLine(number.ToString());
            }
        }

        /// <summary>
        /// Calculates the average of all numbers in an integer array.
        /// </summary>
        /// <param name="numbers">An integer array</param>
        /// <returns>(int) The average of all integers in the input array</returns>
        private int GetAverage(int[] numbers)
        {
            int total = 0;
            int average = 0;

            // Adds each number in "numbers" to total
            for (int i = 0; i < numbers.Length; i++)
            {
                total += numbers[i];
            }
            average = total / numbers.Length; // Divides total by number of numbers in "numbers"
            return average;
        }

        /// <summary>
        /// Calculates the average of all numbers in a jagged integer array.
        /// </summary>
        /// <param name="numbers">A jagged integer array</param>
        /// <returns>(int) The average of all integers in the input jagged array</returns>
        private int GetAverage(int[][] numbers)
        {
            int total = 0;
            int average = 0;
            int count = 0; // Counter for number of total elements in the jagged array
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++)
                {
                    total += numbers[i][j];
                    count++;
                }
            }
            average = total / count;
            return average;
        }

        /// <summary>
        /// Finds the highest value in a jagged integer array and outputs the rows (+1) in which the highest value is present.
        /// </summary>
        /// <param name="numbers">A jagged integer array</param>
        /// <param name="whichRows">A string holding the row numbers (+1) in which the highest value is being held.</param>
        /// <returns>(int) Highest value</returns>
        private int GetHighest(int[][] numbers, out string whichRows)
        {
            int highest = numbers[0][0]; // Sets the default highest value to the first element
            whichRows = ""; // A string to hold which rows possess the highest value

            // Nested for loop to find the highest value in the jagged array
            for (int i = 0; i < numbers.GetLength(0); i++) // numbers.GetLength(0) returns number of rows in the jagged array
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++) // numbers[i].GetLength(0) returns the number of elements in row "i"
                {
                    if (numbers[i][j] > highest)
                    {
                        highest = numbers[i][j];
                    }
                }
            }

            // Nested for loop to find which rows are holding the highest value
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++)
                {
                    if (numbers[i][j] == highest)
                    {
                        whichRows += (i + 1).ToString() + ", "; // Row number is concatenated
                    }
                }
            }
            if (whichRows != null)
            {
                whichRows = whichRows.Remove(whichRows.Length-2);
            }
            return highest;
        }

        /// <summary>
        /// Finds the lowest value in a jagged integer array and outputs the rows (+1) in which the lowest value is present.
        /// </summary>
        /// <param name="numbers">A jagged integer array</param>
        /// <param name="whichRows">A string holding the row numbers (+1) in which the lowest value is being held.</param>
        /// <returns>(int) Lowest value</returns>
        private int GetLowest(int[][] numbers, out string whichRows)
        {
            int lowest = numbers[0][0]; // Sets the default highest value to the first element
            whichRows = ""; // A string to hold which rows possess the highest value

            // Nested for loop to find the highest value in the jagged array
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++)
                {
                    if (numbers[i][j] < lowest)
                    {
                        lowest = numbers[i][j];
                    }
                }
            }

            // Nested for loop to find which rows are holding the highest value
            for (int i = 0; i < numbers.GetLength(0); i++)
            {
                for (int j = 0; j < numbers[i].GetLength(0); j++)
                {
                    if (numbers[i][j] == lowest)
                    {
                        whichRows += (i+1).ToString() + ", "; // Row number is concatenated
                    }
                }
            }
            if (whichRows != null)
            {
                whichRows = whichRows.Remove(whichRows.Length - 2);
            }
            return lowest;
        }

    }

}
