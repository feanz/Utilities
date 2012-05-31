using System;
using System.Text;
using System.Xml.Serialization;
using Utilities.Extensions;
using Utilities.Maths.ExtensionMethods;

namespace Utilities.Maths
{
    /// <summary>
    ///   Matrix used in linear algebra
    /// </summary>
    [Serializable]
    public class Matrix
    {
        private int _height = 1;
        private int _width = 1;

        /// <summary>
        ///   Constructor
        /// </summary>
        /// <param name="width"> Width of the matrix </param>
        /// <param name="height"> Height of the matrix </param>
        /// <param name="values"> Values to use in the matrix </param>
        public Matrix(int width, int height, double[,] values = null)
        {
            _width = width;
            _height = height;
            Values = values ?? new double[width,height];
        }

        /// <summary>
        ///   Width of the matrix
        /// </summary>
        [XmlElement]
        public virtual int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                Values = new double[Width,Height];
            }
        }

        /// <summary>
        ///   Height of the matrix
        /// </summary>
        [XmlElement]
        public virtual int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                Values = new double[Width,Height];
            }
        }

        /// <summary>
        ///   Sets the values of the matrix
        /// </summary>
        /// <param name="x"> X position </param>
        /// <param name="y"> Y position </param>
        /// <returns> the value at a point in the matrix </returns>
        public virtual double this[int x, int y]
        {
            get
            {
                if (x.Between(0, _width) && y.Between(0, _height))
                    return Values[x, y];
                throw new Exception("Index out of bounds");
            }

            set
            {
                if (x.Between(0, _width) && y.Between(0, _height))
                {
                    Values[x, y] = value;
                    return;
                }
                throw new Exception("Index out of bounds");
            }
        }

        /// <summary>
        ///   Values for the matrix
        /// </summary>
        [XmlElement]
        public virtual double[,] Values { get; set; }

        /// <summary>
        ///   Gets the determinant of a square matrix
        /// </summary>
        /// <returns> The determinant of a square matrix </returns>
        public virtual double Determinant()
        {
            if (Width != Height)
                throw new Exception("The determinant can not be calculated for a non square matrix");
            if (Width == 2)
                return (this[0, 0]*this[1, 1]) - (this[0, 1]*this[1, 0]);
            double Answer = 0.0;
            for (int x = 0; x < Width; ++x)
            {
                var TempMatrix = new Matrix(Width - 1, Height - 1);
                int WidthCounter = 0;
                for (int y = 0; y < Width; ++y)
                {
                    if (y != x)
                    {
                        for (int z = 1; z < Height; ++z)
                            TempMatrix[WidthCounter, z - 1] = this[y, z];
                        ++WidthCounter;
                    }
                }
                if (x%2 == 0)
                {
                    Answer += TempMatrix.Determinant();
                }
                else
                {
                    Answer -= TempMatrix.Determinant();
                }
            }
            return Answer;
        }

        /// <summary>
        ///   Transposes the matrix
        /// </summary>
        /// <returns> Returns a new transposed matrix </returns>
        public virtual Matrix Transpose()
        {
            var tempValues = new Matrix(Height, Width);
            for (var x = 0; x < Width; ++x)
                for (var y = 0; y < Height; ++y)
                    tempValues[y, x] = Values[x, y];
            return tempValues;
        }

        #region Operators

        public static Matrix operator +(Matrix M1, Matrix M2)
        {
            if (M1.IsNull())
                throw new ArgumentNullException("M1");
            if (M2.IsNull())
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            var TempMatrix = new Matrix(M1.Width, M1.Height);
            for (var x = 0; x < M1.Width; ++x)
                for (var y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] + M2[x, y];
            return TempMatrix;
        }

        public static Matrix operator /(Matrix M1, double D)
        {
            if (M1.IsNull())
                throw new ArgumentNullException("M1");
            return M1*(1/D);
        }

        public static Matrix operator /(double D, Matrix M1)
        {
            if (M1.IsNull())
                throw new ArgumentNullException("M1");
            return M1*(1/D);
        }

        public static bool operator ==(Matrix M1, Matrix M2)
        {
            if (M1.IsNull() && M2.IsNull())
                return true;
            if (M1.IsNull())
                return false;
            if (M2.IsNull())
                return false;
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                return false;
            for (var x = 0; x <= M1.Width; ++x)
                for (var y = 0; y <= M1.Height; ++y)
                    if (Math.Abs(M1[x, y] - M2[x, y]) > 0.01)
                        return false;
            return true;
        }

        public static bool operator !=(Matrix M1, Matrix M2)
        {
            return !(M1 == M2);
        }

        public static Matrix operator *(Matrix M1, Matrix M2)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            if (M2 == null)
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Dimensions for the matrices are incorrect.");
            Matrix TempMatrix = new Matrix(M2.Width, M1.Height);
            for (int x = 0; x < M2.Width; ++x)
            {
                for (int y = 0; y < M1.Height; ++y)
                {
                    TempMatrix[x, y] = 0.0;
                    for (int i = 0; i < M1.Width; ++i)
                        for (int j = 0; j < M2.Height; ++j)
                            TempMatrix[x, y] += (M1[i, y]*M2[x, j]);
                }
            }
            return TempMatrix;
        }

        public static Matrix operator *(Matrix M1, double D)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y]*D;
            return TempMatrix;
        }

        public static Matrix operator *(double D, Matrix M1)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y]*D;
            return TempMatrix;
        }

        public static Matrix operator -(Matrix M1, Matrix M2)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            if (M2 == null)
                throw new ArgumentNullException("M2");
            if (M1.Width != M2.Width || M1.Height != M2.Height)
                throw new ArgumentException("Both matrices must be the same dimensions.");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = M1[x, y] - M2[x, y];
            return TempMatrix;
        }

        public static Matrix operator -(Matrix M1)
        {
            if (M1 == null)
                throw new ArgumentNullException("M1");
            Matrix TempMatrix = new Matrix(M1.Width, M1.Height);
            for (int x = 0; x < M1.Width; ++x)
                for (int y = 0; y < M1.Height; ++y)
                    TempMatrix[x, y] = -M1[x, y];
            return TempMatrix;
        }

        #endregion

        #region Public Overridden Functions

        public override bool Equals(object obj)
        {
            if (obj is Matrix)
                return this == (Matrix) obj;
            return false;
        }

        public override int GetHashCode()
        {
            double hash = 0;
            for (var x = 0; x < Width; ++x)
                for (var y = 0; y < Height; ++y)
                    hash += this[x, y];
            return (int) hash;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var Seperator = "";
            builder.Append("{").Append(Environment.NewLine);
            for (var x = 0; x < Width; ++x)
            {
                builder.Append("{");
                for (int y = 0; y < Height; ++y)
                {
                    builder.Append(Seperator).Append(this[x, y]);
                    Seperator = ",";
                }
                builder.Append("}").Append(Environment.NewLine);
                Seperator = "";
            }
            builder.Append("}");
            return builder.ToString();
        }

        #endregion
    }
}