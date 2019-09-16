using System;
using System.Collections.Generic;
using System.Text;

namespace AutomataEquivalence
{
    class Matrix
    {
        private static int POZO = -1;
        private readonly int[][] matrix;
        private readonly List<int> start;
        private readonly List<int> end;
        private string[] _transitions;

        private Matrix()
        {
            start = new List<int>();
            end = new List<int>();
        }
        public Matrix(string[] lines)
        {
            if (lines.Length <= 0) return;
            matrix = new int[lines.Length][];
            start = new List<int>();
            end = new List<int>();
            _transitions = lines[0].Split(',');
            for (var i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                matrix[i-1] = new int[values.Length];
                for (var j = 0; j < values.Length; j++)
                {
                    var index = 0;
                    if (j == 0 && values[j].Length > 1)
                    {
                        if (values[j][0] == '>' || (values[j].Length > 1 && values[j][1] == '>'))
                        {
                            start.Add(i-1);
                            index++;
                        }
                        else if (values[j][0] == '*' || (values[j].Length > 1 && values[j][1] == '*'))
                        {
                            end.Add(i-1);
                            index++;
                        }
                    }
                    matrix[i - 1][j] = values[j].Substring(index).Length > 0 ? int.Parse(values[j].Substring(index)) : POZO;
                }
            }

        }

        public int Height()
        {
            return this.matrix.Length;
        }

        public int Width()
        {
            return this._transitions.Length;
        }

        public bool CompareSize(Matrix m)
        {
            return this.Height() == m.Height() && this.Width() == m.Width();
        }

        public int[] this[int i] => this.matrix[i];

        public StateType GetStateType(int node)
        {
            bool start = this.start.Contains(node);
            bool end = this.end.Contains(node);
            if (start && end)
                return StateType.StartFinal;
            if (start)
                return StateType.Start;
            if (end)
                return StateType.Final;
            return StateType.Intermediate;
        }

        public int Start()
        {
            return this.start[0];
        }

        public string[] Transitions => this._transitions;

        public int GetTransitionPosition(string transition)
        {
            for (var i = 0; i < _transitions.Length; i++)
            {
                if (_transitions[i] == transition)
                    return i;
            }

            return -1;
        }
    }

    enum StateType
    {
        Start = 0, Final = 1, Intermediate = 2, StartFinal = 3
    }
}
