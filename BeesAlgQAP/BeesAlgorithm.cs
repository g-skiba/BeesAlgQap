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
    static class BeesAlgorithm
    {

        public static int seed = System.DateTime.Now.Millisecond;
        public static Random random = new Random(seed);

        public static bool usePreviousSeed = false;
        static int ITERATIONS_NUM = 100;
        static int N_BEES = 100;
        static int N_BEST_SOLUTIONS = 50;
        static int N_ELITE = 15;
        static int BEST_NEIGHBOURHOOD_SIZE = 3;
        static int ELITE_NEIGHBOURHOOD_SIZE = 5;

        static int PROBLEM_SIZE;
        static double[,] hweights;
        static double[,] hdistances;
        static double REFERENCE_SOLUTION;
        static int[,] hpermutations;
        static double[] hresults = new double[N_BEES];

        static int[,] hneighbourhoods;
        static double[] hnresults;

        static double bestVal = double.MaxValue;
        static int[] bestPerm;

        static double[] bestFitnesses = new double[ITERATIONS_NUM];
        static double[] maxOfIteration = new double[ITERATIONS_NUM];

        // reprezentacja permutacji:
        // [1,2,0] oznacza ze:
        // - zerowy przedmiot stoi na pierwszym miejscu
        // - pierwszy przedmiot stoi na drugim miejscu
        // - drugi przedmiot stoi na zerowym miejscu


        public static int getIterations()
        {
            return ITERATIONS_NUM;
        }

        public static int getBees()
        {
            return N_BEES;
        }

        public static int getBest()
        {
            return N_BEST_SOLUTIONS;
        }

        public static int getElite()
        {
            return N_ELITE;
        }

        public static int getBestNeighbourhood()
        {
            return BEST_NEIGHBOURHOOD_SIZE;
        }

        public static int getEliteNeighbourhood()
        {
            return ELITE_NEIGHBOURHOOD_SIZE;
        }

        public static void setIterations(int n)
        {
            ITERATIONS_NUM = n;
        }

        public static void setBees(int n)
        {
            N_BEES = n;
        }

        public static void setBest(int n)
        {
            N_BEST_SOLUTIONS = n;
        }

        public static void setElite(int n)
        {
            N_ELITE = n;
        }

        public static void setBestNeighbourhood(int n)
        {
            BEST_NEIGHBOURHOOD_SIZE = n;
        }

        public static void setEliteNeighbourhood(int n)
        {
            ELITE_NEIGHBOURHOOD_SIZE = n;
        }

        public static bool getSeedSaving()
        {
            return usePreviousSeed;
        }

        public static void setSeedSaving(bool value)
        {
            usePreviousSeed = value;
        }

        public static void clear()
        {
            hresults = new double[N_BEES];
            bestFitnesses = new double[ITERATIONS_NUM];
            maxOfIteration = new double[ITERATIONS_NUM];
            bestVal = double.MaxValue;
            if (!usePreviousSeed)
            {
                seed = System.DateTime.Now.Millisecond;
            }
            random = new Random(seed);
        }

        public static void perform(CallbackFields callback)
        {
            try
            {
                clear();
                Console.WriteLine("Running algorithm with parameters:");
                Console.WriteLine("\tIterations: " + ITERATIONS_NUM);
                Console.WriteLine("\tBees: " + N_BEES);
                Console.WriteLine("\tBest: " + N_BEST_SOLUTIONS);
                Console.WriteLine("\tElite: " + N_ELITE);
                Console.WriteLine("\tBest neighbourhood: " + BEST_NEIGHBOURHOOD_SIZE);
                Console.WriteLine("\tElite neighbourhood: " + ELITE_NEIGHBOURHOOD_SIZE);
                Console.WriteLine("\tSeed: " + seed);
                
                CudafyModule km = CudafyTranslator.Cudafy();

                GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
                gpu.LoadModule(km);

                ReadValuesFromFile("QAPexample2.txt");

                double[,] dweights = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                double[,] ddistances = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                int[,] dpermutations = gpu.Allocate<int>(N_BEES, PROBLEM_SIZE);
                double[] dresults = gpu.Allocate<double>(N_BEES);

                int NEIGHBOURHOOD_BEES = N_ELITE * ELITE_NEIGHBOURHOOD_SIZE + N_BEST_SOLUTIONS * BEST_NEIGHBOURHOOD_SIZE;
                hneighbourhoods = new int[NEIGHBOURHOOD_BEES, PROBLEM_SIZE];
                hnresults = new double[NEIGHBOURHOOD_BEES];
                int[,] dneighbourhoods = gpu.Allocate<int>(NEIGHBOURHOOD_BEES, PROBLEM_SIZE);
                double[] dnresults = gpu.Allocate<double>(NEIGHBOURHOOD_BEES);

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

                    maxOfIteration[iteration] = hresults[0];

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

                    bestFitnesses[iteration] = bestVal;

                    //przygotowanie danych do generowania sasiedztwa elity
                    for (int i = 0; i < N_ELITE; i++)
                    {
                        for (int j = 0; j < ELITE_NEIGHBOURHOOD_SIZE; j++)
                        {
                            int row_nr = i * ELITE_NEIGHBOURHOOD_SIZE + j;
                            Array.Copy(hpermutations, i * PROBLEM_SIZE, hneighbourhoods, row_nr * PROBLEM_SIZE, PROBLEM_SIZE);
                        }
                    }

                    //przygotowanie danych do generowania sasiedztwa dobrych rozwiazan
                    for (int i = N_ELITE; i < N_ELITE + N_BEST_SOLUTIONS; i++)
                    {
                        for (int j = 0; j < BEST_NEIGHBOURHOOD_SIZE; j++)
                        {
                            int row_nr = N_ELITE * ELITE_NEIGHBOURHOOD_SIZE + (i - N_ELITE) * BEST_NEIGHBOURHOOD_SIZE + j;
                            Array.Copy(hpermutations, i * PROBLEM_SIZE, hneighbourhoods, row_nr * PROBLEM_SIZE, PROBLEM_SIZE);
                        }
                    }

                    //wygenerowanie sasiedztw (losowe zamiany w permutacjach)
                    for (int i = 0; i < N_ELITE * ELITE_NEIGHBOURHOOD_SIZE + N_BEST_SOLUTIONS * BEST_NEIGHBOURHOOD_SIZE; i++)
                    {
                        int ind1 = random.Next(PROBLEM_SIZE);
                        int ind2 = random.Next(PROBLEM_SIZE);       //TODO zapewnic zeby byly rozne?
                        int tmp = hneighbourhoods[i, ind1];
                        hneighbourhoods[i, ind1] = hneighbourhoods[i, ind2];
                        hneighbourhoods[i, ind2] = tmp;
                    }

                    //policz wyniki dla sasiedztw
                    gpu.CopyToDevice(hneighbourhoods, dneighbourhoods);
                    gpu.Launch(NEIGHBOURHOOD_BEES, 1).calcCosts(dweights, ddistances, dneighbourhoods, dnresults);
                    gpu.CopyFromDevice(dnresults, hnresults);

                    //posortuj wyniki dla wszystkich sasiedztw
                    hneighbourhoods = GetSortedNeibhbourhoodsArray(hneighbourhoods, hnresults);

                    //wybierz najlepsze permutacje z sasiedztw elity
                    for (int i = 0; i < N_ELITE; i++)
                    {
                        int i_n = i * ELITE_NEIGHBOURHOOD_SIZE;
                        Array.Copy(hneighbourhoods, i_n * PROBLEM_SIZE, hpermutations, i * PROBLEM_SIZE, PROBLEM_SIZE);
                    }

                    //wybierz najlepsze permutacje z sasiedztw dobrych rozwiazan
                    for (int i = N_ELITE; i < N_ELITE + N_BEST_SOLUTIONS; i++)
                    {
                        int i_n = N_ELITE * ELITE_NEIGHBOURHOOD_SIZE + (i - N_ELITE) * BEST_NEIGHBOURHOOD_SIZE;
                        Array.Copy(hneighbourhoods, i_n * PROBLEM_SIZE, hpermutations, i * PROBLEM_SIZE, PROBLEM_SIZE);
                    }

                    //gorsze rozwiazania pomijamy - generujemy nowe losowe permutacje
                    GenerateRandomPermutations(N_ELITE + N_BEST_SOLUTIONS, N_BEES);
                }

                PrintResult("Best");

                callback.setFirstValue(bestFitnesses[0]);
                callback.setFinalValue(bestFitnesses[ITERATIONS_NUM - 1]);
                callback.setImprovementValue((bestFitnesses[0] - bestFitnesses[ITERATIONS_NUM -1])/bestFitnesses[0] * 100.0);
                callback.setDatapoints(bestFitnesses, maxOfIteration);
                callback.setReferenceSolution(REFERENCE_SOLUTION);
                callback.setError(Math.Abs(bestFitnesses[ITERATIONS_NUM - 1] - REFERENCE_SOLUTION) / REFERENCE_SOLUTION * 100.0);

                gpu.FreeAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("\nThe end");
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

                sr.ReadLine();
                for (int i = 0; i < PROBLEM_SIZE; i++)
                {
                    line = sr.ReadLine();
                    string[] splitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < PROBLEM_SIZE; j++)
                    {
                        hdistances[i, j] = int.Parse(splitted[j]);      //or hweights?
                    }
                }
                sr.ReadLine();
                for (int i = 0; i < PROBLEM_SIZE; i++)
                {
                    line = sr.ReadLine();
                    string[] splitted = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < PROBLEM_SIZE; j++)
                    {
                        hweights[i, j] = int.Parse(splitted[j]);        //or hdistances?
                    }
                }
                sr.ReadLine();
                line = sr.ReadLine();
                REFERENCE_SOLUTION = double.Parse(line);
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

        private static int[,] GetSortedNeibhbourhoodsArray(int[,] hneighbourhoods, double[] hnresults)
        {
            int[][] hneighbourhoodsJagged = ToJagged<int>(hneighbourhoods);
            for (int i = 0; i < N_ELITE; i++)
            {
                Array.Sort(hnresults, hneighbourhoodsJagged, i * ELITE_NEIGHBOURHOOD_SIZE, ELITE_NEIGHBOURHOOD_SIZE);
            }

            for (int i = N_ELITE; i < N_ELITE + N_BEST_SOLUTIONS; i++)
            {
                Array.Sort(hnresults, hneighbourhoodsJagged, N_ELITE * ELITE_NEIGHBOURHOOD_SIZE + (i - N_ELITE) * BEST_NEIGHBOURHOOD_SIZE, BEST_NEIGHBOURHOOD_SIZE);
            }
            return ToRectangular<int>(hneighbourhoodsJagged);
        }

        [Cudafy]
        public static void calcCosts(GThread thread, double[,] weights, double[,] distances, int[,] permutations, double[] results)
        {
            int tid = thread.blockIdx.x;
            int problemSize = weights.GetLength(0);
            results[tid] = 0;
            for( int x = 0; x < problemSize; x++ )
            {
                //for( int y = x + 1 ; y < problemSize; y++)
                for (int y = 0; y < problemSize; y++ )
                {
                    results[tid] += weights[x, y] * distances[permutations[tid, x], permutations[tid, y]];
                }
            }
        }
    }
}
