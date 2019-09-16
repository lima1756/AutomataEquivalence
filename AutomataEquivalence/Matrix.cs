using System;
using System.Collections.Generic;
using System.Text;

namespace AutomataEquivalence
{
    class Matrix
    {
        private static int POZO = -1;
        private readonly int[][] _matrix;
        private readonly List<int> _start;
        private readonly List<int> _end;

        private Matrix()
        {
            _start = new List<int>();
            _end = new List<int>();
        }
        public Matrix(IReadOnlyList<string> lines)
        {
            if (lines.Count <= 0) return;
            _matrix = new int[lines.Count][];
            _start = new List<int>();
            _end = new List<int>();
            Transitions = lines[0].Split(',');
            for (var i = 1; i < lines.Count; i++)
            {
                var values = lines[i].Split(',');
                _matrix[i-1] = new int[values.Length];
                for (var j = 0; j < values.Length; j++)
                {
                    var index = 0;
                    if (j == 0 && values[j].Length > 1)
                    {
                        if (values[j][0] == '>' || (values[j].Length > 1 && values[j][1] == '>'))
                        {
                            _start.Add(i-1);
                            index++;
                        }
                        if (values[j][0] == '*' || (values[j].Length > 1 && values[j][1] == '*'))
                        {
                            _end.Add(i-1);
                            index++;
                        }
                    }
                    _matrix[i - 1][j] = values[j].Substring(index).Length > 0 ? int.Parse(values[j].Substring(index)) : POZO;
                }
            }

        }

        public int Height()
        {
            return this._matrix.Length;
        }

        public int Width()
        {
            return this.Transitions.Length;
        }

        public bool CompareSize(Matrix m)
        {
            return this.Width() == m.Width();
        }

        public int[] this[int i] => this._matrix[i];

        public StateType GetStateType(int node)
        {
            var start = this._start.Contains(node);
            var end = this._end.Contains(node);
            if (start && end)
                return StateType.StartFinal;
            if (start)
                return StateType.Start;
            return end ? StateType.Final : StateType.Intermediate;
        }

        public int Start()
        {
            return this._start[0];
        }

        public string[] Transitions { get; }

        public int GetTransitionPosition(string transition)
        {
            for (var i = 0; i < Transitions.Length; i++)
            {
                if (Transitions[i] == transition)
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
