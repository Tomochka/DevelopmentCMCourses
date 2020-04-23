namespace Matrix
{
    using System;
    using System.Collections.Generic;

    public class Matrix
    {
        public readonly double[,] Array;

        //The constructor for matrix of type double
        public Matrix(double[,] matrixArray)
        {
            Array = new double[matrixArray.GetLength(0), matrixArray.GetLength(1)];

            for (var i = 0; i < matrixArray.GetLength(0); i++)
            {
                for (var j = 0; j < matrixArray.GetLength(1); j++)
                {
                    Array[i, j] = matrixArray[i, j];
                }
            }
        }

        //The constructor for specifying matrices of size nxm
        public Matrix(int n, int m)
        {
            Array = new double[n, m];

            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    Array[i, j] = 0;
                }
            }
        }

        //The constructor for matrix of type Matrix
        public Matrix(Matrix matrix)
        {
            Array = new double[matrix.Array.GetLength(0), matrix.Array.GetLength(1)];

            for (var i = 0; i < matrix.Array.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.Array.GetLength(1); j++)
                {
                    Array[i, j] = matrix.Array[i, j];
                }
            }
        }

        //Addition and subtraction of matrices
        public static Matrix Addition(Matrix[] matrices, short sign)
        {
            var matrixSum = new Matrix(matrices[0]);

            for (var k = 1; k < matrices.Length; k++)
            {
                for (var i = 0; i < matrixSum.Array.GetLength(0); i++)
                {
                    for (var j = 0; j < matrixSum.Array.GetLength(1); j++)
                    {
                        matrixSum.Array[i, j] += sign * matrices[k].Array[i, j];
                    }
                }
            }

            return matrixSum;
        }

        //Multiplication matrix by a number
        public static Matrix MultiplicationByScalar(Matrix matrix, double number)
        {
            var matrixMultScalar = new Matrix(matrix);

            for (var i = 0; i < matrix.Array.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.Array.GetLength(1); j++)
                {
                    matrixMultScalar.Array[i, j] = matrixMultScalar.Array[i, j] * number;
                }
            }

            return matrixMultScalar;
        }

        //Matrix multiplication
        public static Matrix Multiplication(params Matrix[] matrix)
        {
            var matrixForSum = new Matrix(matrix[0]);
            var matrixMult = new Matrix(matrixForSum);

            for (var k = 1; k < matrix.Length; k++)
            {
                matrixMult = new Matrix(matrixForSum.Array.GetLength(0), matrix[k].Array.GetLength(1));

                for (var i = 0; i < matrixForSum.Array.GetLength(0); i++)
                {
                    for (var j = 0; j < matrix[k].Array.GetLength(1); j++)
                    {
                        double sum = 0;

                        for (var l = 0; l < matrixForSum.Array.GetLength(1); l++)
                        {
                            sum += matrixForSum.Array[i, l] * matrix[k].Array[l, j];
                        }

                        matrixMult.Array[i, j] = sum;
                    }
                }

                matrixForSum = new Matrix(matrixMult);
            }

            return matrixMult;
        }

        //Raising the matrix to a power.
        public static Matrix Exponentiation(Matrix matrix, int n)
        {
            var arrayMatrix = new Matrix[n];

            for (var i = 0; i < arrayMatrix.Length; i++)
            {
                arrayMatrix[i] = new Matrix(matrix);
            }

            return Multiplication(arrayMatrix);
        }

        //The method of calculating the determinant of a matrix by the basic definition (with permutations)
        public static double Determinant(Matrix matrix)
        {
            double det = 0, composition = 1;

            //Array of indices. Performing permutations in it, we obtain all possible combinations for the product of the elements of the matrix
            var indices = IndicesMatrix(matrix.Array.GetLength(0));

            do
            {
                for (var i = 0; i < matrix.Array.GetLength(0); i++)
                {
                    composition *= matrix.Array[i, indices[i]];
                }

                var sign = (int)Math.Pow(-1, Inversions(indices));
                det += sign * composition;
                composition = 1;
            }
            while (NextPermutation(indices));

            return det;
        }

        //Calculation of the matrix of minors for the elements matrix_ij of the matrix
        public static Matrix Minor(Matrix matrix)
        {
            var matrixMinor = new Matrix(matrix);
            var size = matrix.Array.GetLength(0);
            var matrixForDet = new Matrix(size - 1, size - 1);

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var m = 0;
                    var n = 0;

                    for (var k = 0; k < size; k++)
                    {
                        for (var l = 0; l < size; l++)
                        {
                            if ((k == i) || (l == j))
                            {
                                continue;
                            }

                            matrixForDet.Array[m, n] = matrix.Array[k, l];

                            if (n < matrixForDet.Array.GetLength(1) - 1)
                            {
                                n++;
                            }
                            else
                            {
                                n = 0;
                                if (m < matrixForDet.Array.GetLength(0) - 1)
                                {
                                    m++;
                                }
                            }
                        }
                    }

                    matrixMinor.Array[i, j] = Determinant(matrixForDet);
                }
            }

            return matrixMinor;
        }

        //Computation of the inverse matrix
        public static Matrix Inverse(Matrix matrix)
        {
            var matrixForInverse = Transposition(AlgebraicSupplements(Minor(matrix)));
            var matrixInverse = MultiplicationByScalar(matrixForInverse, 1 / Determinant(matrix));
            return matrixInverse;
        }

        //Transpose matrix
        public static Matrix Transposition(Matrix matrix)
        {
            var matrixTransp = new Matrix(matrix);

            for (var i = 0; i < matrixTransp.Array.GetLength(0); i++)
            {
                for (var j = 0; j < matrixTransp.Array.GetLength(1); j++)
                {
                    if (i >= j)
                    {
                        continue;
                    }

                    Swap(ref matrixTransp.Array[i, j], ref matrixTransp.Array[j, i]);
                }
            }

            return matrixTransp;
        }

        //Calculating the rank of the matrix
        public static int Rang(Matrix matrix)
        {
            var rang = 0;
            var q = 1;
            var matrixBasic = new Matrix(matrix);

            while (q <= Math.Min(matrixBasic.Array.GetLength(0), matrixBasic.Array.GetLength(1)))
            {
                var matrixForRang = new Matrix(q, q);

                for (var i = 0; i < (matrixBasic.Array.GetLength(0) - (q - 1)); i++)
                {
                    for (var j = 0; j < (matrixBasic.Array.GetLength(1) - (q - 1)); j++)
                    {
                        for (var k = 0; k < q; k++)
                        {
                            for (var m = 0; m < q; m++)
                            {
                                matrixForRang.Array[k, m] = matrixBasic.Array[i + k, j + m];
                            }
                        }

                        var det = Determinant(matrixForRang);
                        const double eps = 0.000001;

                        if (Math.Abs(det) > eps)
                        {
                            rang = q;
                        }
                    }
                }

                q++;
            }

            return rang;
        }

        //Sum of elements of the main diagonal of the matrix
        public static double SumDiagonalElements(Matrix matrix)
        {
            double sum = 0;

            for (var i = 0; i < matrix.Array.GetLength(0); i++)
            {
                sum += matrix.Array[i, i];
            }

            return sum;
        }

        //Modified ToString() method to output matrix to console
        public override string ToString()
        {
            var str = string.Empty;

            for (var i = 0; i < Array.GetLength(0); i++)
            {
                for (var j = 0; j < Array.GetLength(1); j++)
                {
                    str += Array[i, j].ToString("F2") + " ";
                }

                str += "\n";
            }

            return str;
        }

        //A method that performs a permutation in an array of integers. The numList array must initially be sorted in ascending order.
        private static bool NextPermutation(IList<int> numList)
        {
            
             // 1. We are looking for a maximal index j such that a[j] < a[j + 1]. If there is no such index, then the permutation is the last one.
             // 2. Find the largest index l such that a[j] < a[l]. Because j + 1 there exists, then l always satisfies the condition j < l
             // 3. Change the places a[j] and a[l].
             // 4. Expand the sequence starting a[j + 1] until the last element[n] is entered.
            

            //1.
            var largestIndex = -1;

            for (var i = numList.Count - 2; i >= 0; i--)
            {
                if (numList[i] >= numList[i + 1])
                {
                    continue;
                }

                largestIndex = i;
                break;
            }

            if (largestIndex < 0)
            {
                return false;
            }

            //2.
            var largestIndex2 = -1;
            for (var i = numList.Count - 1; i >= 0; i--)
            {
                if (numList[largestIndex] >= numList[i])
                {
                    continue;
                }

                largestIndex2 = i;
                break;
            }

            //3.
            var tmp = numList[largestIndex];
            numList[largestIndex] = numList[largestIndex2];
            numList[largestIndex2] = tmp;

            //4.
            for (int i = largestIndex + 1, j = numList.Count - 1; i < j; i++, j--)
            {
                tmp = numList[i];
                numList[i] = numList[j];
                numList[j] = tmp;
            }

            return true;
        }

        // Method for obtaining the source array of indexes
        private static int[] IndicesMatrix(int n)
        {
            var result = new int[n];

            for (var i = 0; i < n; i++)
            {
                result[i] = i;
            }

            return result;
        }

        // Method for determining the parity or oddness of the permutation
        private static int Inversions(IReadOnlyList<int> m)
        {
            var result = 0;
            for (var i = 0; i < m.Count; i++)
            {
                for (var j = i + 1; j < m.Count; j++)
                {
                    if (m[i] > m[j])
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        //Swaps the elements value1 and value2
        private static void Swap(ref double value1, ref double value2)
        {
            var spare = value1;
            value1 = value2;
            value2 = spare;
        }

        //Calculation of the matrix of algebraic complements
        private static Matrix AlgebraicSupplements(Matrix matrix)
        {
            var matrixAlgSupp = new Matrix(matrix);
            var size = matrix.Array.GetLength(0);

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if ((i + j) % 2 == 1)
                    {
                        matrixAlgSupp.Array[i, j] = -matrixAlgSupp.Array[i, j];
                    }
                }
            }

            return matrixAlgSupp;
        }
    }
}

