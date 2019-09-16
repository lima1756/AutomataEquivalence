using System;
using System.Collections.Generic;
using System.IO;

namespace AutomataEquivalence
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please enter the 2 matrix");
                return;
            }

            var data1 = ReadFile(args[0]);
            var data2 = ReadFile(args[1]);
            if (data1==null || data2==null)
            {
                Console.ReadKey();
                return;
            }
            var m1 = new Matrix(ReadFile(args[0]));
            var m2 = new Matrix(ReadFile(args[1]));

            Compare(m1, m2);
            
            Console.ReadKey();
        }

        private static void Compare(Matrix m1, Matrix m2)
        {
            if (!m1.CompareSize(m2))
            {
                Console.WriteLine("Not equivalent");
                return;
            }

            List<Tuple<int, int>> visited = new List<Tuple<int, int>>();
            Queue<Tuple<int, int>> toVisit = new Queue<Tuple<int, int>>();
            toVisit.Enqueue(Tuple.Create(m1.Start(), m2.Start()));
            while (toVisit.Count > 0)
            {
                var current = toVisit.Dequeue();
                visited.Add(current);
                foreach (var transition in m1.Transitions)
                {
                    var col1 = m1.GetTransitionPosition(transition);
                    var col2 = m2.GetTransitionPosition(transition);
                    if (col1 == -1 || col2 == -1)
                    {
                        Console.WriteLine("Not equivalent");
                        return;
                    }

                    var tuple = Tuple.Create(m1[current.Item1][col1], m2[current.Item2][col2]);
                    var stateM1 = m1.GetStateType(tuple.Item1);
                    var stateM2 = m2.GetStateType(tuple.Item2);
                    if (!(
                        stateM1 == stateM2 ||
                        stateM1 == StateType.StartFinal && (stateM2 == StateType.Start || stateM2 == StateType.Start) ||
                        stateM2 == StateType.StartFinal && (stateM1 == StateType.Start || stateM1 == StateType.Start)
                    ))
                    {
                        Console.WriteLine("Not equivalent");
                        return;
                    }

                    if (!visited.Contains(tuple) && !toVisit.Contains(tuple))
                    {
                        toVisit.Enqueue(tuple);
                    }
                }
            }
            Console.WriteLine("Equivalent");
        }

        private static string[] ReadFile(string path)
        {
            try
            {
                return System.IO.File.ReadAllLines(path);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Directory no found");
                return null;
            }
        }
    }
}
