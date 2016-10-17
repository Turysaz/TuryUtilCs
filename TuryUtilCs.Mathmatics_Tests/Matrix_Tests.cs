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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TuryUtilCs.Mathmatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuryUtilCs.Mathmatics.Tests
{
    [TestClass()]
    public class Matrix_Tests
    {
        /// <summary>
        /// 1 2 3
        /// 4 5 6
        /// 7 8 9
        /// </summary>
        private Matrix _testA_3x3;

        /// <summary>
        /// 
        /// </summary>
        private Matrix _testB_3x4;

        /// <summary>
        /// 
        /// </summary>
        private Matrix _testC_2x3;

        /// <summary>
        /// 
        /// </summary>
        private Matrix _testD_3x3;

        /// <summary>
        /// 1 0 0 0
        /// 0 1 0 0
        /// 0 0 1 0
        /// 0 0 0 1
        /// </summary>
        private Matrix _identityMatrix_4x4;

        public Matrix_Tests()
        {
            GenerateTestMatrices();
        }

        #region test matrices
        private void GenerateTestMatrices()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixB();
            GenerateTestMatrixC();
            GenerateTestMatrixD();

            GenerateTestMatrixE();
        }

        private void GenerateTestMatrixA()
        {
            _testA_3x3 = new Matrix(3, 3);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _testA_3x3[i, j] = (i * 3) + (j + 1);
                }
            }
        }

        private void GenerateTestMatrixB()
        {
            double[,] arrayInput = new double[3, 4];
            arrayInput[1, 3] = 4;
            arrayInput[2, 2] = 9;
            arrayInput[0, 0] = 1;

            _testB_3x4 = new Matrix(arrayInput);

        }

        private void GenerateTestMatrixC()
        {
            double[][] arrayInput = new double[2][];
            arrayInput[0] = new double[3];
            arrayInput[1] = new double[3];
            arrayInput[0][0] = 1;
            arrayInput[0][1] = 2;
            arrayInput[0][2] = 3;
            arrayInput[1][0] = 4;
            arrayInput[1][1] = 5;
            arrayInput[1][2] = 6;

            _testC_2x3 = new Matrix(arrayInput);
        }

        private void GenerateTestMatrixD()
        {
            // Create matrix with determinant != 0
            GenerateTestMatrixA();

            _testD_3x3 = _testA_3x3.Copy();
            _testD_3x3[0, 1] = 1;
            _testD_3x3[1, 1] = -1;
        }

        private void GenerateTestMatrixE()
        {
            _identityMatrix_4x4 = Matrix.IdentityMatrix(4);
        }

        #endregion


        // ######################
        //      UNIT TESTS
        // ######################


        [TestMethod()]
        public void Matrix_Test_CreationByDimensions()
        {
            // Matrix A is created by it's dimensions
            // and then filled with numbers from 1 to 9

            GenerateTestMatrixA();

            double expected = 1;

            for (int i = 0; i < _testA_3x3.Rows; i++)
            {
                for (int j = 0; j < _testA_3x3.Columns; j++)
                {
                    double actual = _testA_3x3[i, j];
                    Assert.AreEqual(expected, actual);
                    expected++;
                }
            }
        }

        [TestMethod()]
        public void Matrix_Test_CreationBy2Darray()
        {
            // test matrix B is created by two-dimensional array
            GenerateTestMatrixB();

            int k = 0;
            double[] expectedOutput = new double[] {
                1, 0, 0, 0,
                0, 0, 0, 4,
                0, 0, 9, 0 };

            for (int i = 0; i < _testB_3x4.Rows; i++)
            {
                for (int j = 0; j < _testB_3x4.Columns; j++)
                {
                    double actual = _testB_3x4[i, j];
                    double expected = expectedOutput[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Matrix_Test_CreationByArrayOfArrays()
        {
            // test matrix C is created by array of arrays
            GenerateTestMatrixC();

            int k = 0;
            double[] expectedOutput = new double[] {
                1, 2, 3,
                4, 5, 6};

            for (int i = 0; i < _testC_2x3.Rows; i++)
            {
                for (int j = 0; j < _testC_2x3.Columns; j++)
                {
                    double actual = _testC_2x3[i, j];
                    double expected = expectedOutput[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Matrix_Test_CreationByVector()
        {
            Vector input = new Vector(0.5, -9, 3);
            double[] expectedValues = new double[] { 0.5, -9, 3 };

            Matrix m = new Matrix(input);

            // check dimensions
            Assert.AreEqual(3, m.Rows);
            Assert.AreEqual(1, m.Columns);

            // check entries
            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Matrix_Test_CreationByVectorArray()
        {
            double[] input = new double[] { 0.5, -9, 3 };

            Matrix m = new Matrix(input);

            // check dimensions
            Assert.AreEqual(3, m.Rows);
            Assert.AreEqual(1, m.Columns);

            // check entries
            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = input[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Copy_Test()
        {
            GenerateTestMatrixA();

            Matrix copy = _testA_3x3.Copy();

            Assert.AreNotSame(_testA_3x3, copy);

            for (int i = 0; i < copy.Rows; i++)
            {
                for (int j = 0; j < copy.Columns; j++)
                {
                    double actual = copy[i, j];
                    double expected = _testA_3x3[i, j];
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod()]
        public void AddRow_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.AddRow(1);

            Assert.AreEqual(4, m.Rows);

            double[] expectedValues = new double[] {
                1,2,3,
                0,0,0,
                4,5,6,
                7,8,9};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void DeleteRow_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.DeleteRow(1);

            Assert.AreEqual(2, m.Rows);

            double[] expectedValues = new double[] {
                1,2,3,
                7,8,9};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void AddColumn_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.AddColumn(1);

            Assert.AreEqual(4, m.Columns);

            double[] expectedValues = new double[] {
                1,0,2,3,
                4,0,5,6,
                7,0,8,9};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void DeleteColumn_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.DeleteColumn(1);

            Assert.AreEqual(2, m.Columns);

            double[] expectedValues = new double[] {
                1,3,
                4,6,
                7,9};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void SwapRows_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.SwapRows(0, 2);

            double[] expectedValues = new double[] {
                7,8,9,
                4,5,6,
                1,2,3 };

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void SwapColumns_Test()
        {
            //TODO more different tests
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.SwapColumns(0, 2);

            double[] expectedValues = new double[] {
                3,2,1,
                6,5,4,
                9,8,7};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            };
        }

        [TestMethod()]
        public void IsSquareMatrix_Test()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixB();
            GenerateTestMatrixC();
            GenerateTestMatrixE();

            Assert.IsTrue(_testA_3x3.IsSquareMatrix());
            Assert.IsTrue(_identityMatrix_4x4.IsSquareMatrix());
            Assert.IsFalse(_testB_3x4.IsSquareMatrix());
            Assert.IsFalse(_testC_2x3.IsSquareMatrix());
        }

        [TestMethod()]
        public void Multiply_Test_Scalar()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixB();

            Matrix[] testCandidates = new Matrix[] { _testA_3x3, _testB_3x4 };
            double[] factors = new double[] { -1, 0, 10, 3.5, System.Math.PI, -2.0013 };

            foreach (Matrix m in testCandidates)
            {
                foreach (double f in factors)
                {
                    Matrix multiplied = m.Multiply(f);
                    for (int i = 0; i < m.Rows; i++)
                    {
                        for (int j = 0; j < m.Columns; j++)
                        {
                            double expected = m[i, j] * f;
                            double actual = multiplied[i, j];
                            Assert.AreEqual(expected, actual);
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void Inverse_Test()
        {
            GenerateTestMatrixD();
            Matrix invD = _testD_3x3.Inverse();
            Matrix product = Matrix.Multiply(_testD_3x3, invD);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double expected = 0;
                    if (j == i) { expected = 1; }
                    double actual = product[i, j];
                    // delta: randomly small number
                    Assert.AreEqual(expected, actual, 0.00000000001);
                }
            }

        }

        [TestMethod()]
        public void Determinant_Test()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixD();
            GenerateTestMatrixE();

            double detA = _testA_3x3.Determinant();
            double detD = _testD_3x3.Determinant();
            double detE = _identityMatrix_4x4.Determinant();

            Assert.AreEqual(0, detA);
            Assert.AreEqual(66, detD);
            Assert.AreEqual(1, detE);

        }

        [TestMethod()]
        public void Cofactor_Test()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixD();

            double expected;
            double actual;

            actual = _testD_3x3.Cofactor(0, 2);
            expected = 39;
            Assert.AreEqual(expected, actual);

            actual = _testD_3x3.Cofactor(0, 0);
            expected = -57;
            Assert.AreEqual(expected, actual);

            actual = _testA_3x3.Cofactor(1, 1);
            expected = -12;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Transposed_Test()
        {
            GenerateTestMatrixA();

            Matrix m = _testA_3x3.Transposed();
            double[] expectedValues = new double[]{
                1,4,7,
                2,5,8,
                3,6,9};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Add_Test()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixD();

            Matrix m = Matrix.Add(_testA_3x3, _testD_3x3);
            double[] expectedValues = new double[]{
                2,3,6,
                8,4,12,
                14,16,18};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Subtract_Test()
        {
            GenerateTestMatrixA();
            GenerateTestMatrixD();

            Matrix m = Matrix.Subtract(_testA_3x3, _testD_3x3);
            double[] expectedValues = new double[]{
                0,1,0,
                0,6,0,
                0,0,0};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Multiply_Test_MatrixByMatrix()
        {
            GenerateTestMatrixB();
            GenerateTestMatrixD();

            Matrix m = Matrix.Multiply(_testD_3x3, _testB_3x4);
            double[] expectedValues = new double[]{
                1,0,27, 4,
                4,0,54,-4,
                7,0,81,32};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Multiply_Test_MatrixByVector()
        {
            GenerateTestMatrixA();

            Vector v = new Vector(1, 2, 3);
            Matrix m = Matrix.Multiply(_testA_3x3, v);
            double[] expectedValues = new double[] { 14, 32, 50 };

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Multiply_Test_VectorByMatrix()
        {
            GenerateTestMatrixA();

            Vector v = new Vector(1, 2, 3);
            Matrix m = Matrix.Multiply(v, _testA_3x3);
            double[] expectedValues = new double[] { 30, 36, 42};

            int k = 0;
            for (int i = 0; i < m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    double actual = m[i, j];
                    double expected = expectedValues[k];
                    Assert.AreEqual(expected, actual);
                    k++;
                }
            }
        }

        [TestMethod()]
        public void Eigenvalues_Test()
        {
            GenerateTestMatrixD();

            double[] actual = _testD_3x3.Eigenvalues();
            List<double> tempsort = actual.ToList();
            tempsort.Sort();
            actual = tempsort.ToArray();

            double[] expected = new double[] { -4.403, -1.038, 14.441};

            for (int i = 0; i<expected.Length; i++)
            {
                double a = actual[i];
                double e = expected[i];
                Assert.AreEqual(e, a, 0.01);
            }
       }

        [TestMethod()]
        public void Eigenvectors_Test()
        {
            GenerateTestMatrixD();

            Matrix[] actualList = _testD_3x3.Eigenvectors();

            Vector[] expectedList = new Vector[3];
            expectedList[0] = new Vector(-0.293, -1.419, 1);
            expectedList[1] = new Vector(-1.501, 0.058, 1);
            expectedList[2] = new Vector(0.257, 0.455, 1);

            for(int i = 0; i < 3; i++)
            {
                Vector ev = actualList[i].ToVector();
                double angle = Vector.AngleBetween(expectedList[i], ev);

                Assert.AreEqual(0, angle, 0.001);
            }

        }

        [TestMethod()]
        public void IdentityMatrix_Test()
        {
            Matrix e3 = Matrix.IdentityMatrix(3);
            Matrix e10 = Matrix.IdentityMatrix(10);

            Assert.AreEqual(10, e10.Rows);
            Assert.AreEqual(10, e10.Columns);
            Assert.AreEqual(3, e3.Rows);
            Assert.AreEqual(3, e3.Columns);

            for (int i = 0; i < e10.Rows; i++)
            {
                for (int j = 0; j < e10.Columns; j++)
                {
                    double expected = 0;
                    if (i == j) { expected = 1; }
                    double actual = e10[i, j];
                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestMethod()]
        public void ToVector_Test()
        {
            GenerateTestMatrixA();
            Matrix m = _testA_3x3.Copy();
            m = m.DeleteColumn(1).DeleteColumn(1);
            Vector v = m.ToVector();
            Vector expected = new Vector(1, 4, 7);
            Assert.AreEqual(expected.X, v.X);
            Assert.AreEqual(expected.Y, v.Y);
            Assert.AreEqual(expected.Z, v.Z);
        }

    }
}