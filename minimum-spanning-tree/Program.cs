using System;
using System.Collections.Generic;
using System.IO;

namespace minimum_spanning_tree
{
    class Program
    {
        static void Main(string[] args)
        {
            (int, int) sizeG = (0, 0);
            List<(int, int, double)> edgesList = Read(ref sizeG, "test.txt");
            double[,] matrix = AdjMatrix(sizeG, edgesList);
            List<(int, int, double)> result = SkeletalTreeCalc(sizeG, matrix);
            Console.WriteLine("Список ребер дерева мин...");
            foreach (var item in result)
                Console.WriteLine($"{item.Item1} -- {item.Item2} == {item.Item3}");
            double sum = SumEgdes(result);
            Console.WriteLine($"Стоймость минимального остовного дерева: {sum}");
        }
        static List<(int, int, double)> Read(ref (int, int) sizeG, string path)
        { 
            List<(int, int, double)> list = new List<(int, int, double)>();
            StreamReader read = new StreamReader(path);
            string[] size = read.ReadLine()?.Split(' ');
            if (size != null)
            {
                sizeG = (Convert.ToInt32(size[0]), Convert.ToInt32(size[1]));
                for (int i = 0; i < sizeG.Item2; ++i)
                {
                    size = read.ReadLine()?.Split(' ');
                    if (size != null)
                        list.Add((Convert.ToInt32(size[0]), Convert.ToInt32(size[1]), Convert.ToDouble(size[2])));
                }
            }
            return list;
        }
        static double[,] AdjMatrix((int, int) sizeMatrix, List<(int, int, double)> edgeList)
        {
            double[,] matrixA = new Double[sizeMatrix.Item1, sizeMatrix.Item1];
            foreach (var item in edgeList)
            {
                matrixA[item.Item1 - 1, item.Item2 - 1] = item.Item3;
                matrixA[item.Item2 - 1, item.Item1 - 1] = item.Item3;   
            }
            return matrixA;
        }
        static List<(int, int, double)> SkeletalTreeCalc((int, int) sizeG, double[,] matrix)
        {
            List<(int, int, double)> result = new List<(int, int, double)>();
            List<int> part1 = new List<int>();
            List<int> part2 = new List<int>();
            for (int i = 1; i < sizeG.Item1; i++)
                part2.Add(i + 1);
            part1.Add(1);
            while (part2.Count != 0)
            {
                double min = Double.PositiveInfinity;
                (int, int, double) pos = (0, 0, 0);
                foreach (var item in part1)
                    for (int j = 0; j < sizeG.Item1; j++)
                        if (SearchInList(part2, j + 1) && matrix[item - 1, j] != 0)
                            if (min > matrix[item - 1, j])
                            {
                                min = matrix[item - 1, j];
                                pos = (item, j + 1, matrix[item - 1, j]);
                            }
                if (pos.Item2 != 0 && min != Double.PositiveInfinity)
                {
                    part1.Add(pos.Item2);
                    part2.Remove(pos.Item2);
                    result.Add(pos);
                }
            }
            return result;
        }
        static bool SearchInList(List<int> list, int a)
        {
            foreach (var item in list)
            {
                if (item == a)
                {
                    return true;
                }
            }   
            return false;
        }
        static double SumEgdes(List<(int, int, double)> list)
        {
            double sum = 0;
            foreach (var item in list)
                sum += item.Item3;
            return sum;
        }
    }
}