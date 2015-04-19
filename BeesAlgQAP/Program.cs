using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeesAlgQAP
{
    class Program
    {
        public static Random random = new Random();

        static void Main(string[] args)
        {
            try
            {
                CudafyModule km = CudafyTranslator.Cudafy();

                GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
                gpu.LoadModule(km);

                int PROBLEM_SIZE = 5;
                int N_BEES = 100;

                double[,] hweights = new double[PROBLEM_SIZE, PROBLEM_SIZE];    //should be symmetric
                double[,] hdistances = new double[PROBLEM_SIZE, PROBLEM_SIZE];  //should be symmetric
                int[,] hpermutations = new int[N_BEES, PROBLEM_SIZE];
                double[] hresults = new double[N_BEES];

                double[,] dweights = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                double[,] ddistances = gpu.Allocate<double>(PROBLEM_SIZE, PROBLEM_SIZE);
                int[,] dpermutations = gpu.Allocate<int>(N_BEES, PROBLEM_SIZE);
                double[] dresults = gpu.Allocate<double>(N_BEES);

                GenerateRandomValues(hweights, hdistances, hpermutations, PROBLEM_SIZE, N_BEES);
                PrintValues(hweights, hdistances, hpermutations, PROBLEM_SIZE, N_BEES);

                gpu.CopyToDevice(hweights, dweights);
                gpu.CopyToDevice(hdistances, ddistances);
                gpu.CopyToDevice(hpermutations, dpermutations);
                //gpu.CopyToDevice(hresults, dresults);

                gpu.Launch(N_BEES, 1).calcCosts(dweights, ddistances, dpermutations, dresults);

                gpu.CopyFromDevice(dresults, hresults);

                for (int i = 0; i < N_BEES; i++ )
                {
                    Console.Write("{0}, ", hresults[i]);
                }
                Console.WriteLine();

                gpu.FreeAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("\nThe end");
            Console.ReadKey();
        }

        public static void GenerateRandomValues(double[,] hweights, double[,] hdistances, int[,] hpermutations, int PROBLEM_SIZE, int N_BEES)
        {
            double val;
            for (int i = 0; i < PROBLEM_SIZE; i++)
            {
                for (int j = i; j < PROBLEM_SIZE; j++)
                {
                    val = random.NextDouble();
                    hweights[i, j] = val;
                    hweights[j, i] = val;
                    val = random.NextDouble();
                    hdistances[i, j] = val;
                    hdistances[j, i] = val;
                }
            }

            for (int i = 0; i < N_BEES; i++)
            {
                for (int j = 0; j < PROBLEM_SIZE; j++)
                {
                    hpermutations[i, j] = j;
                }
                Shuffle(hpermutations, i);
            }
        }

        public static void PrintValues(double[,] hweights, double[,] hdistances, int[,] hpermutations, int PROBLEM_SIZE, int N_BEES)
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

        [Cudafy]
        public static void map(GThread thread, int[] a)
        {
            int tid = thread.blockIdx.x;
            if (tid < a.Length)
                a[tid] = 2 * a[tid];
        }

        [Cudafy]
        public static void addVector(GThread thread, int[] a, int[] b, int[] c)
        {
            // Get the id of the thread. addVector is called N times in parallel, so we need 
            // to know which one we are dealing with.
            int tid = thread.blockIdx.x;
            // To prevent reading beyond the end of the array we check that 
            // the id is less than Length
            if (tid < a.Length)
                c[tid] = a[tid] + b[tid];
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
