using System;
using System.Threading;
using System.Collections.Generic;

namespace Radix_sort
{
	public class Program
	{
    static Dictionary<int, int[]> threadsBuckets;
    static Dictionary<int, int[]> distributionDict;
    static void Main(string[] args)
    {
      int numOfThreads = 3, maxNum = 0;
      int[] initialArray = { 101, 1, 20, 50, 9, 98, 27, 455, 35, 854599, 229, 948, 27, 1563, 350, 9, 9, 18, 7 };
      distributionDict = new Dictionary<int, int[]>();
      threadsBuckets = new Dictionary<int, int[]>();

      splitArray(initialArray, numOfThreads);

      //find largest element in the Array
      for (int i = 1; i < initialArray.Length; i++)
        if (maxNum < initialArray[i])
          maxNum = initialArray[i];

      Console.WriteLine("Number of threads: " + Environment.ProcessorCount);
      Console.WriteLine("Original Array: ");
      PrintArray(initialArray);

      int rounds = (int)Math.Floor(Math.Log10(maxNum) + 1);
      Console.WriteLine("Number of rounds: " + rounds);
      Console.WriteLine();

      Thread a = new Thread(() => radixsort(1, 0));
      Thread b = new Thread(() => radixsort(1, 1));
      Thread c = new Thread(() => radixsort(1, 2));

      a.Start();
      a.Join();
      b.Start();
      b.Join();
      c.Start();
    }
    static void splitArray(int[] array, int numOfThreads)
    {
      threadsBuckets.Clear();

      var len = (int)array.Length / numOfThreads;
      int arrayIndex = 0;
      int numRemaining = array.Length - len * numOfThreads; // not zero if splitted arrays are not of the same sizes

      for (int threadNum = 0; threadNum < numOfThreads; threadNum++)
      {
        if (numRemaining > 0) // create array for the thread with remaining/extra number
        {
          threadsBuckets.Add(threadNum, new int[len + 1]);
          numRemaining--;
        }
        else // create array for the thread
        {
          threadsBuckets.Add(threadNum, new int[len]);
        }
        // adding elements to the thread's array
        for (int index = 0; index < threadsBuckets[threadNum].Length; index++)
        {
          threadsBuckets[threadNum][index] = array[arrayIndex];
          arrayIndex++;
        }
      }
    }
    static void radixsort(int place, int threadNumber)
    {
      int[] Array = threadsBuckets[threadNumber];
      int n = Array.Length;
      int max = Array[0];

      //find largest element in the Array
      for (int i = 1; i < n; i++)
        if (max < Array[i])
          max = Array[i];

      Console.WriteLine("Original array for thread#" + (threadNumber + 1) + " :");
      PrintArray(threadsBuckets[threadNumber]);

      countingsort(threadsBuckets[threadNumber], place);

      Console.WriteLine("Thread#" + (threadNumber + 1) + " has sorted for digit " + place + ":");
      PrintArray(threadsBuckets[threadNumber]);
    }
    static void countingsort(int[] Array, int place)
    {
      int n = Array.Length;
      int[] output = new int[n];

      //range of the number is 0-9 for each place considered.
      int[] freq = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
      //count number of occurrences in freq array
      for (int i = 0; i < n; i++)
      {
        freq[(Array[i] / place) % 10]++;
      }

      //Change count[i] so that count[i] now contains actual 
      //position of this digit in output[] 
      for (int i = 1; i < 10; i++)
      {
        freq[i] += freq[i - 1];
      }

      //Build the output array 
      for (int i = n - 1; i >= 0; i--)
      {
        output[freq[(Array[i] / place) % 10] - 1] = Array[i];
        freq[(Array[i] / place) % 10]--;
      }

      //Copy the output array to the input Array, Now the Array will 
      //contain sorted array based on digit at specified place
      for (int i = 0; i < n; i++)
        Array[i] = output[i];
    }
    static void PrintArray(int[] Array)
    {
      int n = Array.Length;
      for (int i = 0; i < n; i++)
        Console.Write(Array[i] + " ");
      Console.Write("\n\n");
    }
	}
}
