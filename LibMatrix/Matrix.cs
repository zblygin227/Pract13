using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibMatrix
{
    public class Matrix<T>
    {
        private T[,] _items;
        private int _rows, _columns;

        public Matrix(int rowCount, int columnCount, string extension = ".matrix")
        {
            _items = new T[rowCount, columnCount];
            Extension = extension;
        }

        public string Extension { get; private set; }

        public T this[int row, int column]
        {
            get { return _items[row, column]; }
            set { _items[row, column] = value; }
        }

        public void DefaultInit()
        {
            _items = default;
            _rows = default;
            _columns = default;
        }

        public int Rows
        {
            get => _rows;
            private set
            {
                if (value == _rows)
                {
                    return;
                }
                _rows = value;
            }
        }

        public int Columns
        {
            get => _columns;
            private set
            {
                if (value == _columns)
                {
                    return;
                }
                _columns = value;
            }
        }

        public void Add(T[,] items)
        {
            _rows = items.GetLength(0);
            _columns = items.GetLength(1);
            _items = new T[_rows, _columns];
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    _items[i, j] = items[i, j];
                }
            }
        }

        private static readonly BinaryFormatter _formatter = new();

        public void Save(string path)
        {
            BinaryFormatter formatter = new();
            using (FileStream stream = new(path, FileMode.Create))
            {
                formatter.Serialize(stream, _items);
            }
        }


        public void Open(string path)
        {
            BinaryFormatter formatter = new();
            using (FileStream stream = new(path, FileMode.Open))
            {
                _items = formatter.Deserialize(stream) as T[,];
            }
        }

        public DataTable ToDataTable()
        {
            var res = new DataTable();
            for (int i = 0; i < _items.GetLength(1); i++)
            {
                res.Columns.Add("Столбец " + (i + 1), typeof(T));
            }

            for (int i = 0; i < _items.GetLength(0); i++)
            {
                var row = res.NewRow();

                for (int j = 0; j < _items.GetLength(1); j++)
                {
                    row[j] = _items[i, j];
                }

                res.Rows.Add(row);
            }
            return res;
        }
    }

    public static class VisualArray
    {
        public static DataTable ToDataTable<T>(this T[] arr)
        {
            var res = new DataTable();
            for (int i = 0; i < arr.Length; i++)
            {
                res.Columns.Add("col" + (i + 1), typeof(T));
            }
            var row = res.NewRow();
            for (int i = 0; i < arr.Length; i++)
            {
                row[i] = arr[i];
            }
            res.Rows.Add(row);
            return res;
        }
    }
}
