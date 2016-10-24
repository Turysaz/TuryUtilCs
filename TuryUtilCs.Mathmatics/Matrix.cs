/*
* This file is part of TuryUtilCs, the free C# utility collection.
* Copyright (C) 2016 Till Fischer aka Turysaz
*
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License v2 as published
* by the Free Software Foundation.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along
* with this program; if not, write to the Free Software Foundation, Inc.,
* 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace TuryUtilCs.Mathmatics
{
    /// <summary>
    /// Representation of a matrix (math)
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// The matrix' components / entries
        /// </summary>
        private double[,] _entries;

        /// <summary>
        /// Matrix entry at position [row, column].
        /// The first index of the rows/columns is 0.
        /// </summary>
        /// <param name="row">row of the desired entry</param>
        /// <param name="col">column of the desired entry</param>
        /// <returns>Entry at position [row, column]</returns>
        public double this[int row, int col]
        {
            get { return _entries[row, col]; }
            set { _entries[row, col] = value; }
        }

        /// <summary>
        /// Amount of rows the matrix has.
        /// </summary>
        public int Rows
        {
            get { return _entries.GetLength(0); }
        }

        /// <summary>
        /// Amount of columns the matrix has.
        /// </summary>
        public int Columns
        {
            get { return _entries.GetLength(1); }
        }

        ///<summary>
        /// Creates a matrix with a defined number of rows and columns.
        /// </summary>
        /// <param name="rows">Amount of rows.</param>
        /// <param name="cols">Amount of colums.</param>
        public Matrix(int rows, int columns)
        {
            _entries = new double[rows, columns];
        }

        /// <summary>
        /// Creates a matrix of a two-dimensional double-array.
        /// </summary>
        /// <param name="c"> This array will be converted into a matrix-object.</param>
        public Matrix(double[,] c)
        {
            _entries = c;
        }

        /// <summary>
        /// Creates a matrix of an array of arrays.
        /// Please make shure that all of the entries of c have the same length.
        /// e.g.: c[0].Length == c[1].Length == ...
        /// </summary>
        /// <param name="c">Array of array (de facto two-dimensional array) representing a matrix</param>
        public Matrix(double[][] c)
        {
            int rows = c.Length;
            int cols = c[0].Length;
            _entries = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                if (c[i].Length != cols)
                {
                    throw new MatrixException("Cannot create matrix! Not all rows have the same length!");
                }
                for (int j = 0; j < cols; j++)
                {
                    _entries[i, j] = c[i][j];
                }
            }
        }

        /// <summary>
        /// Creates a new (3x1)-Matrix of a 3D Vector.
        /// </summary>
        /// <param name="v">Vector used to create the matrix.</param>
        public Matrix(Vector v)
        {
            _entries = new double[3, 1];
            this[0, 0] = v.X;
            this[1, 0] = v.Y;
            this[2, 0] = v.Z;
        }

        /// <summary>
        /// Creates a new (nx1)-Matrix of the (nx1)-Vector
        /// represente by v.
        /// </summary>
        /// <param name="v">Vector of the size (nx1)</param>
        public Matrix(double[] v)
        {
            _entries = new double[v.Length, 1];
            for (int i = 0; i<v.Length; i++)
            {
                this[i, 0] = v[i];
            }
        }

        /// <summary>
        /// Create exact copy of the matrix.
        /// (create new object)
        /// </summary>
        /// <returns>Copy of the matrix</returns>
        public Matrix Copy()
        {
            return this.Multiply(1);
        }

        ///<summary>
        /// Adds a row to the Matrix. The new row will only contain zeros. 
        /// Returns a copy.
        /// </summary>
        /// <param name="index">Position at which the new row will be inserted.</param>
        /// <returns>Copy of the matrix with added row.</returns>
        public Matrix AddRow(int index)
        {
            Matrix extendedMatrix = new Matrix(this.Rows + 1, this.Columns);
            int a = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (i == index)
                    {
                        a = 1;
                    }
                    extendedMatrix[i + a, j] = this[i, j];
                }
            }
            return extendedMatrix;
        }

        ///<summary>
        /// Deletes a row of the matrix.
        /// Returns a copy
        /// </summary>
        /// <param name="index">Index of the row that shall be deleted.</param>
        /// <returns>Copy of the matrix without specified row</returns>
        public Matrix DeleteRow(int index)
        {
            if (index >= this.Rows)
            {
                throw new MatrixException(
                        "Can't delete row, because the matrix hasn't that many rows!");
            }
            Matrix shortenedMatrix = new Matrix(this.Rows - 1, this.Columns);
            int a = 0;
            for (int i = 0; i < this.Rows - 1; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    if (i == index)
                    {
                        a = 1;
                    }
                    shortenedMatrix[i, j] = this[i + a, j];
                }
            }
            return shortenedMatrix;
        }

        /// <summary>
        /// Adds a column to the Matrix.
        /// Returns a copy!
        /// The new column will be filled with zeroes.
        /// </summary>
        /// <param name="index">Position of the newly added Column </param>
        /// <returns>Copy of the original Matrix with one additional column</returns>
        public Matrix AddColumn(int index)
        {
            Matrix extendedMatrix = new Matrix(this.Rows, this.Columns + 1);
            byte a = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                a = 0;
                for (int j = 0; j < this.Columns; j++)
                {
                    if (j == index)
                    {
                        a = 1;
                    }
                    extendedMatrix[i, j + a] = this[i, j];
                }
            }
            return extendedMatrix;
        }


        ///<summary>
        /// Deletes a column of the matrix.
        /// Returns a copy
        /// </summary>
        /// <param name="index">Index of the column that shall be deleted.</param>
        /// <returns>Copy of the matrix without specified column</returns>
        public Matrix DeleteColumn(int index)
        {
            Matrix shortenedMatrix = new Matrix(this.Rows, this.Columns - 1);
            byte a = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                a = 0;
                for (int j = 0; j < this.Columns - 1; j++)
                {
                    if (j == index)
                    {
                        a = 1;
                    }
                    shortenedMatrix[i, j] = this[i, j + a];
                }
            }
            return shortenedMatrix;
        }

        /// <summary>
        /// Swaps the row index of two rows of the matrix.
        /// Both rows will be replaced by each other.
        /// DOES NOT RETURN A COPY
        /// </summary>
        /// <param name="row1">first row index</param>
        /// <param name="row2">second row index</param>
        /// <returns>itself with swapped rows</returns>
        public Matrix SwapRows(int row1, int row2)
        {
            double tmp;
            for (int i = 0; i < Columns; i++)
            {
                tmp = this[row1, i];
                this[row1, i] = this[row2, i];
                this[row2, i] = tmp;
            }
            return this;
        }

        /// <summary>
        /// Swaps the column index of two columns of the matrix.
        /// Both columns will be replaced by each other.
        /// DOES NOT RETURN A COPY
        /// </summary>
        /// <param name="row1">first column index</param>
        /// <param name="row2">second column index</param>
        /// <returns>itself with swapped columns</returns>
        public Matrix SwapColumns(int col1, int col2)
        {
            double tmp;
            for (int i = 0; i < Rows; i++)
            {
                tmp = this[i, col1];
                this[i, col1] = this[i, col2];
                this[i, col2] = tmp;
            }
            return this;
        }

        /// <summary>
        /// Check if matrix is square matrix (amount rows == amount columns)
        /// </summary>
        /// <returns>True, if square matrix.</returns>
        public bool IsSquareMatrix()
        {
            return (Rows == Columns);
        }

        /// <summary>
        /// Multiplies a matrix by a scalar factor. Returns a copy.
        /// </summary>
        /// <param name="scalar">Multiplication factor</param>
        /// <returns>Copy of the matrix, multiplied by factor.</returns>
        public Matrix Multiply(double scalar)
        {
            Matrix product = new Matrix(this.Rows, this.Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    product[i, j] = this[i, j] * scalar;
                }
            }
            return product;
        }

        ///<summary>
        /// Calculates the inverse matrix of the matrix.
        /// </summary>
        /// <returns>Inverse matrix</returns>
        public Matrix Inverse()
        {
            if (!IsSquareMatrix())
            {
                throw new MatrixException(
                        "Inverse of a not-squared matrix is not defined!");
            }

            Matrix inverse = new Matrix(Rows, Columns);
            double determinant = Determinant();

            if (determinant == 0)
            {
                throw new MatrixException(
                        "Determinant of the Matrix is 0, can't calculate Inverse!");
            }

            #region explanation of the corresponding mathmatics
            // Calculation of inverse matrix:
            // ==============================
            // 
            // A      - original matrix
            // A'     - inverse of A
            // A_ij   - remaining matrix after deleting row i and column j from A
            // a_ij   - cofactors of A
            // cof(A) - "cofactor matrix" of A
            // adj(A) - adjoint matrix of A (dt: "Adjunkte")
            //
            // ----------
            //
            // a_(ij) = (-1)^(i+j) * det(A_ij)
            // 
            // cof(A) = [a_ij]
            //
            // adj(A) = transposed(cof(A))
            //
            // A' = ( 1/det(A) ) * adj(A)
            //
            // for calculation of determinante see corresponding method
            #endregion math explanation

            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    inverse[i, j] = this.Cofactor(j, i) / determinant;
                }
            }

            return inverse;
        }

        ///<summary>
        /// Calculates the determinant of the matrix.
        /// </summary>
        /// <returns>Determinant of the matrix</returns>
        public double Determinant()
        {
            //OPTIMIERBAR Calculation of Determinant
            // lt. wikipedia ist der laplace'sche Entwicklungssatz für eine 
            // nxn -Matrix von der Ordnung O(n!), bei anderen nur O(n^3), 
            // teilweise sogar besser => "Strassen-Algorithmus"

            #region explanation of the corresponding mathmatics
            // Calculation of the determinant according to Laplace
            // ====================================================
            //
            // A      - original matrix
            // A_ij   - remaining matrix after removing row i and column j
            // a_ij   - entries of A
            //
            //           n
            // det(A) = SUM (-1)^(i+j) * det(A_ij) * a_ij
            //          i=1
            //
            // 
            // (-1)^(i+j) * det(A_ij) is the cofactor â_ij
            #endregion  

            if (!IsSquareMatrix())
            {
                throw new MatrixException(
                        "Determinant is not defined for not-squared matrices!");
            }

            if (Rows == 1 && Columns == 1)
            {
                return this[0, 0];
            }

            double determinant = 0;

            for (int i = 0; i < Columns; i++)
            {
                determinant = determinant + this[0, i] * Cofactor(0, i);
            }

            return determinant;
        }

        /// <summary>
        /// Returns the cofactor a_ij of the matrix A.
        /// </summary>
        /// <param name="row">i (row)</param>
        /// <param name="col">j (column)</param>
        /// <returns>cofactor a_ij</returns>
        public double Cofactor(int row, int col)
        {
            // Calculation of cofactor â_ij
            // ===========================
            //
            // A_ij  - remaining matrix after deleting row i and column j
            //
            // â_ij = (-1)^(i+j) * det(A_ij)

            int d;
            if ((row + col) % 2 == 0)
            {
                d = 1;
            }
            else
            {
                d = -1;
            }
            return d * DeleteColumn(col).DeleteRow(row).Determinant();
        }

        ///<summary>
        /// Creates the transposed matrix.
        /// Returns a copy.
        /// </summary>
        /// <returns>transposed matrix</returns>
        public Matrix Transposed()
        {
            Matrix transposed = new Matrix(this.Columns, this.Rows);
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Columns; j++)
                {
                    transposed[j, i] = this[i, j];
                }
            }
            return transposed;
        }

        /// <summary>
        /// Adds two matrices and returns the sum.
        /// </summary>
        /// <param name="m1">first summand</param>
        /// <param name="m2">second summand</param>
        /// <returns>sum of the matrices</returns>
        public static Matrix Add(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Columns || m1.Rows != m2.Rows)
            {
                throw new MatrixException(
                        "Matrices don't have the same dimensions!");
            }
            Matrix sum = new Matrix(m1.Rows, m1.Columns);
            for (int i = 0; i < m1.Rows; i++)
            {
                for (int j = 0; j < m1.Columns; j++)
                {
                    sum[i, j] = m1[i, j] + m2[i, j];
                }
            }
            return sum;
        }

        /// <summary>
        /// Subtrancted a matrix from another and returns the difference.
        /// </summary>
        /// <param name="minuend"></param>
        /// <param name="subtrahend"></param>
        /// <returns>Difference of the matrices</returns>
        public static Matrix Subtract(Matrix minuend, Matrix subtrahend)
        {
            return Add(minuend, subtrahend.Multiply(-1));
        }

        /// <summary>
        /// Multiplies two matrices
        /// </summary>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix m1, Matrix m2)
        {
            if (m1.Columns != m2.Rows)
            {
                throw new MatrixException(
                        "Rows and Columns of the Matrices do not fit!");
            }

            Matrix product = new Matrix(m1.Rows, m2.Columns);

            for (int col = 0; col < m2.Columns; col++)
            {
                for (int row = 0; row < m1.Rows; row++)
                {
                    double a = 0;
                    for (int i = 0; i < m1.Columns; i++)
                    {
                        a = a + m1[row, i] * m2[i, col];
                    }
                    product[row, col] = a;
                }
            }

            return product;
        }

        /// <summary>
        /// Multiply a (nx3) Matrix A with a vertical (3x1) Vector v. Returns a (nx1)-matrix R.
        /// R = A * v
        /// </summary>
        /// <param name="m">(nx3) matrix</param>
        /// <param name="v">(3x1) vertival vector</param>
        /// <returns>(nx1) matrix</returns>
        public static Matrix Multiply(Matrix m, Vector v)
        {
            return Multiply(m, new Matrix(v));
        }

        /// <summary>
        /// Multiplies a transposed vector v (3x1) with a (3xn) matrix A. Returns a (1xn)-matrix R.
        /// R = v.transposed * A
        /// </summary>
        /// <param name="v">vertical vector (3x1). A transposed copy will be generated.</param>
        /// <param name="m">(3xn) matrix</param>
        /// <returns>(1xn) matrix</returns>
        public static Matrix Multiply(Vector v, Matrix m)
        {
            return Multiply(new Matrix(v).Transposed(), m);
        }
        
        /// <summary>
        /// Calculates the Eigenvectors of the matrix.
        /// The accuracy of the calculation will be set to
        /// 5 decimals (default).
        /// </summary>
        /// <returns>Array of the matrix's eigenvalues</returns>
        public double[] Eigenvalues()
        {
            return Eigenvalues(5);
        }

        /// <summary>
        /// Calculates the matrix's eigenvalues.
        /// </summary>
        /// <param name="accuracy">
        /// Number of correctly computed decimals
        /// of the eigenvalues
        /// </param>
        /// <returns>Array of eigenvalues.</returns>
        public double[] Eigenvalues(int accuracy)
        {
            if (!IsSquareMatrix())
            {
                throw new MatrixException(
                    "Eigenvalues for non-square matrices are not defined!");
            }

            if (Rows != 3)
            {
                throw new NotImplementedException(
                        "Sorry! Eigenvalues are only implemented for 3x3 matrices...");
            }

            return EigenvaluesOf3x3(accuracy);

        }

        /// <summary>
        /// Calculates the eigenvalues of a (3x3)-matrix.<para/>
        /// This method creates the matrix's characteristical
        /// polynomial and then calculates the eigenvalues by
        /// finding the zero set of that polynomial.<para/>
        /// <b>This method shall not be called directly!
        /// Call Eigenvalues() instead!</b>
        /// </summary>
        /// <param name="accuracy">
        /// Number of correctly computed decimals
        /// </param>
        /// <returns>Array(Length = 3) of eigenvalues.</returns>
        protected double[] EigenvaluesOf3x3(int accuracy)
        { 
            
            if (Rows != 3)
            {
                throw new ArgumentException(
                    "The matrix has to be of the size (3x3)!");
            }

            #region defining a-j
            //    / a  b  c \
            //    | d  e  f |
            //    \ g  h  j /

            double a, b, c, d, e, f, g, h, j;

            a = this[0, 0];
            b = this[0, 1];
            c = this[0, 2];

            d = this[1, 0];
            e = this[1, 1];
            f = this[1, 2];

            g = this[2, 0];
            h = this[2, 1];
            j = this[2, 2];
            #endregion

            // characteristical polynomial
            // f(x) = Ax^3 + Bx^2 + Cx + D
            double[] coefficiants = new double[4];
            //A
            coefficiants[3] = -1;
            // B
            coefficiants[2] = a + e + j;
            // C
            coefficiants[1] = c * g + b * d + f * h - a * e - a * j - e * j;
            // D
            coefficiants[0] = a * e * j + b * f * g + c * d * h
                - a * f * h - b * d * j - c * e * g;

            //TODO Try-catch and specifying the exception types in Polynomial!
            // could surround next line (return new...) by try-catch-block and
            // increase accuracy here
            // OR could not do that and put all the exception stuff into the
            // Polynomial class

            return new Polynomial(coefficiants).ZeroSetN3(accuracy);
        }

        /// <summary>
        /// Calculates the matrix's eigenvectors.
        /// Uses default values for the accuracy of the eigenvalue calculation,
        /// the matrix scaling factor and the forceFindReal-flag.
        /// <para/>
        /// eigenvalue accuracy = 5 decimals <para/>
        /// matrix scaling factor = 1 <para/>
        /// force find real = false
        /// </summary>
        /// <returns>
        /// Array of eigenvectors, in the order of the sizes of
        /// their eigenvalues (smallest eigenvalue first, largest last)
        /// </returns>
        public Matrix[] Eigenvectors()
        {
            return Eigenvectors(5, 1, false);
        }

        /// <summary>
        /// Calculates the matrix's eigenvectors.
        /// Uses default values for the accuracy of
        /// the eigenvalue calculation.
        /// <para/>
        /// eigenvalue accuracy = 5 decimals <para/>
        /// </summary>
        /// <returns>
        /// Array of eigenvectors, in the order of the sizes of
        /// their eigenvalues (smallest eigenvalue first, largest last)
        /// </returns>
        public Matrix[] Eigenvectors(int eigenvalueAccuracy)
        {
            return Eigenvectors(eigenvalueAccuracy, 1, false);
        }

        //DOKU
        public Matrix[] Eigenvectors(int eigenvalueAccuracy, int scalingFactor,
            bool forceFindReal)
        {
            return Eigenvectors(eigenvalueAccuracy, scalingFactor, forceFindReal, false);
        }

        /// <summary>
        /// Calculates the Matrix's Eigenvectors.
        /// </summary>
        /// <param name="eigenvalueAccuracy">
        /// Accuracy of the eigenvalue computation:
        /// Defines how many decimals shall be calculated
        /// correctly.
        /// </param>
        /// <param name="scalingFactor">
        /// If the matrix is multiplied by a constant scalar,
        /// so are it's eigenvalues. The eigenvectors however
        /// will not be affected by this at all. Hence, the
        /// matrix can be multiplied by a constant scalar(>1), wich
        /// will make the numerical computation of the eigenvalues
        /// by characteristical polynomial and the eigenvectors faster.
        /// BUT BE AWARE: This might be unstable if the matrix's
        /// entries are large.
        /// </param>
        /// <param name="forceFindReal"><param>
        /// <returns>
        /// Array of eigenvectors, in the order of the sizes of
        /// their eigenvalues (smallest eigenvalue first, largest last)
        /// </returns>
        public Matrix[] Eigenvectors(int eigenvalueAccuracy, 
            int scalingFactor, bool forceFindReal, bool scaleToEigenvalues)
        {
            double[] eigenvalues = null;

            // create a copy of the matrix scaled by the scaling factor
            Matrix scaledCopy = this.Multiply(scalingFactor);

            // this flag is used if the finding of real eigenvalues is forced
            bool eigenvaluesSatisfying = false;

            // stores the amount of iterations of the algorithm
            // this is also used to increase the accuracy at each
            // iteration
            int iterations = 0;

            // do calculate eigenvalues until the result is satisfying
            while (!eigenvaluesSatisfying)
            {
                // the accuracy will be increased at each iteration
                eigenvalues = scaledCopy.Eigenvalues(eigenvalueAccuracy + iterations);

                if (forceFindReal)
                {
                    eigenvaluesSatisfying = true;

                    // Check all eigenvalues
                    // If one of them is NaN, it means that
                    // this eigenvalue is not a real number.
                    // In this case, the eigenvalue search
                    // will be done again with increased 
                    // accuracy.
                    foreach (double e in eigenvalues)
                    {
                        if (double.IsNaN(e))
                        {
                            eigenvaluesSatisfying = false;
                            iterations++;
                            break;
                        }
                    }
                }
                // if not forced to find real eigenvalues, the first
                // result will be accepted.
                else 
                {
                    eigenvaluesSatisfying = true;
                }
            }

            // Sort the eigenvalues, so the smallest will be first
            // and the larges will be last.
            List<double> tmpEvalList = eigenvalues.ToList();
            tmpEvalList.Sort();
            eigenvalues = tmpEvalList.ToArray();

            // array of eigenvectors (will be filled by the next step)
            Matrix[] vectors = new Matrix[eigenvalues.Length];

            // another copy of the matrix is needed, because a linear
            // system needs to be of the size (n, n+1), so an additional
            // column needs to be appended
            Matrix copy;
            for (int a = 0; a < eigenvalues.Length; a++)
            {
                copy = scaledCopy.Copy();

                // Subtract the eigenvalue y from the matrix
                // and add another row (linare system)
                //
                //    / a-y  b   c  | 0 \
                //    |  d  e-y  f  | 0 |
                //    \  g   h  j-y | 0 / 
                //
                for (int i = 0; i < Rows; i++)
                {
                    copy[i, i] -= eigenvalues[a];
                }
                copy = copy.AddColumn(Columns + 1);
                vectors[a] = new Matrix(new LinearSystem(copy).
                    SolveGauss(eigenvalueAccuracy));

                if (scaleToEigenvalues)
                {
                    if(vectors.Length != 3)
                    {
                        throw new NotImplementedException(
                            "Scaling of eigenvectors has only been implemented for 3x3-Matrices.");
                    }
                    
                    // scale the eigenvectors to the length of
                    // their corresponding eigenvalues
                    Vector tmpVec = vectors[a].ToVector();
                    tmpVec = tmpVec.Normalize().MultiplySkalar(eigenvalues[a]);
                    vectors[a] = new Matrix(tmpVec);
                }
            }
            return vectors;
        }

        /// <summary>
        /// Returns an identity matrix of the size nxn.
        /// </summary>
        /// <param name="n">n</param>
        /// <returns>IdentityMatrix nxn</returns>
        public static Matrix IdentityMatrix(int n)
        {
            Matrix e = new Matrix(n, n);
            for (int i = 0; i < n; i++)
            {
                e[i, i] = 1;
            }
            return e;
        }

        ///<summary>
        /// Converts a (3x1)-matrix to a 3D-vector.
        /// </summary>
        /// <returns>Converted Matrix</returns>
        public Vector ToVector()
        {
            if (Rows != 3 || Columns != 1)
            {
                throw new MatrixException(
                        "Cannot create vector, since the matrix has not the format 3x1!");
            }
            return new Vector(this[0, 0], this[1, 0], this[2, 0]);
        }

        /// <summary>
        /// Turns matrix into string.
        /// </summary>
        /// <returns>matrix in plain text</returns>
        public override String ToString()
        {
            String s = "";
            for (int i = 0; i < Rows; i++)
            {
                s += "(  ";
                for (int j = 0; j < Columns; j++)
                {
                    s += this[i, j] + "  ";
                }
                s += ") \n";
            }
            return s;
        }
    }
}
