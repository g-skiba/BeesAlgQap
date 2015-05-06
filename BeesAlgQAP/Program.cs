using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeesAlgQAP
{
    static class Program
    {
        public static Random random = new Random();

        static int ITERATIONS_NUM = 100;
        static int N_BEES = 100;
        static int N_BEST_SOLUTIONS = N_BEES / 2;

        static int PROBLEM_SIZE;
        static double[,] hweights;
        static double[,] hdistances;
        static int[,] hpermutations;
        static double[] hresults = new double[N_BEES];

        static double bestVal = double.MaxValue;
        static int[] bestPerm;

        // reprezentacja permutacji:
        // [1,2,0] oznacza ze:
        // - zerowy przedmiot stoi na pierwszym miejscu
        // - pierwszy przedmiot stoi na drugim miejscu
        // - drugi przedmiot stoi na zerowym miejscu

        static void Main(string[] args)
        {
            try
            {
                CudafyModule km = CudafyTranslator.Cudafy();

                GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
                gpu.LoadModule(km);

                ReadValuesFromFile("QAPexample.txt");

                double[,] dweights = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                double[,] ddistances = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                int[,] dpermutations = gpu.Allocate<int>(N_BEES, PROBLEM_SIZE);
                double[] dresults = gpu.Allocate<double>(N_BEES);

                gpu.CopyToDevice(hweights, dweights);
                gpu.CopyToDevice(hdistances, ddistances);

                //losowa inicjalizacja populacji
                GenerateRandomPermutations(0, N_BEES);

                for (int iteration = 0; iteration < ITERATIONS_NUM; iteration++)
                {
                    gpu.CopyToDevice(hpermutations, dpermutations);
                    gpu.Launch(N_BEES, 1).calcCosts(dweights, ddistances, dpermutations, dresults);
                    gpu.CopyFromDevice(dresults, hresults);

                    //posortowanie permutacji wedlug kosztow
                    hpermutations = GetSortedPermutationsArray(hpermutations, hresults);

                    if(bestVal > hresults[0])
                    {
                        //zapamietanie najlepszego rozwiazania
                        bestVal = hresults[0];
                        for (int i = 0; i < PROBLEM_SIZE; i++)
                        {
                            bestPerm[i] = hpermutations[0, i];
                        }
                        PrintResult("New best");
                    }

                    //dla lepszych rozwiazan losowa zmiana - zamiana dwoch elementow w permutacji
                    for (int i = 0; i < N_BEST_SOLUTIONS; i++)
                    {
                        int ind1 = random.Next(PROBLEM_SIZE);
                        int ind2 = random.Next(PROBLEM_SIZE);       //TODO zapewnic zeby byly rozne?
                        int tmp = hpermutations[i, ind1];
                        hpermutations[i, ind1] = hpermutations[i, ind2];
                        hpermutations[i, ind2] = tmp;
                    }

                    //gorsze rozwiazania pomijamy - generujemy nowe losowe permutacje
                    GenerateRandomPermutations(N_BEST_SOLUTIONS, N_BEES);
                }

                PrintResult("Best");

                gpu.FreeAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("\nThe end");
            Console.ReadKey();
        }

        private static void PrintResult(string str)
        {
            Console.WriteLine(String.Format(str + " value: {0}", bestVal));
            Console.Write(str + " permutation: ");
            for (int i = 0; i < PROBLEM_SIZE; i++)
            {
                Console.Write(String.Format("{0}, ", bestPerm[i]));
            }
            Console.WriteLine();
        }

        private static void ReadValuesFromFile(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                String line = sr.ReadLine();
                PROBLEM_SIZE = int.Parse(line);

                hweights = new double[PROBLEM_SIZE, PROBLEM_SIZE];
                hdistances = new double[PROBLEM_SIZE, PROBLEM_SIZE];
                hpermutations = new int[N_BEES, PROBLEM_SIZE];
                bestPerm = new int[PROBLEM_SIZE];

                line = sr.ReadLine();
                for (int i = 0; i < PROBLEM_SIZE; i++)
                {
                    line = sr.ReadLine();
                    string[] splitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < PROBLEM_SIZE; j++)
                    {
                        hdistances[i, j] = int.Parse(splitted[j]);      //or hweights?
                    }
                }
                line = sr.ReadLine();
                for (int i = 0; i < PROBLEM_SIZE; i++)
                {
                    line = sr.ReadLine();
                    string[] splitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < PROBLEM_SIZE; j++)
                    {
                        hweights[i, j] = int.Parse(splitted[j]);        //or hdistances?
                    }
                }
            }
        }

        private static void GenerateRandomPermutations(int begin, int end)
        {
            for (int i = begin; i < end; i++)
            {
                for (int j = 0; j < PROBLEM_SIZE; j++)
                {
                    hpermutations[i, j] = j;
                }
                Shuffle(hpermutations, i);
            }
        }

        public static void PrintValues()
        {
            for (int i = 0; i < PROBLEM_SIZE; i++)
            {
                for (int j = 0; j < PROBLEM_SIZE; j++)
                {
                    Console.Write("{0}, ", hweights[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            for (int i = 0; i < PROBLEM_SIZE; i++)
            {
                for (int j = 0; j < PROBLEM_SIZE; j++)
                {
                    Console.Write("{0}, ", hdistances[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            for (int i = 0; i < N_BEES; i++)
            {
                for (int j = 0; j < PROBLEM_SIZE; j++)
                {
                    Console.Write("{0}, ", hpermutations[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void Shuffle(int[,] array, int row)
        {
            int n = array.GetLength(1);
            while (n > 1)
            {
                n--;
                int j = random.Next(n + 1);
                int temp = array[row, j];
                array[row, j] = array[row, n];
                array[row, n] = temp;
            }
        }

        static T[][] ToJagged<T>(this T[,] array)
        {
            int height = array.GetLength(0), width = array.GetLength(1);
            T[][] jagged = new T[height][];
            for (int i = 0; i < height; i++)
            {
                T[] row = new T[width];
                for (int j = 0; j < width; j++)
                {
                    row[j] = array[i, j];
                }
                jagged[i] = row;
            }
            return jagged;
        }

        static T[,] ToRectangular<T>(this T[][] array)
        {
            int height = array.Length, width = array[0].Length;
            T[,] rect = new T[height, width];
            for (int i = 0; i < height; i++)
            {
                T[] row = array[i];
                for (int j = 0; j < width; j++)
                {
                    rect[i, j] = row[j];
                }
            }
            return rect;
        }

        public static int[,] GetSortedPermutationsArray(int[,] hpermutations, double[] hresults)
        {
            int[][] hpermutationsJagged = ToJagged<int>(hpermutations);
            Array.Sort(hresults, hpermutationsJagged);
            return ToRectangular<int>(hpermutationsJagged);
        }

        [Cudafy]
        public static void calcCosts(GThread thread, double[,] weights, double[,] distances, int[,] permutations, double[] results)
        {
            int tid = thread.blockIdx.x;
            int problemSize = weights.GetLength(0);
            results[tid] = 0;
            for( int x = 0; x < problemSize; x++ )
            {
                for( int y = x + 1 ; y < problemSize; y++)
                {
                    results[tid] += weights[x, y] * distances[permutations[tid, x], permutations[tid, y]];
                }
            }
        }
    }
}
