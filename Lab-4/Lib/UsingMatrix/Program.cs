namespace UsingMatrix
{
    using System;
    using Matrix;
   
    class Program
    {
        public enum Sign : short
        {
            Plus = 1,
            Minus = -1
        }
        public static void Main(string[] args)
        {

            double[,] arrayA = { { 1, 2, 3, 2 }, { 1, 2, -5, 1 }, { 3, 2, 1, -4 }, { -1, 2, 1, 4 } };
            double[,] arrayB = { { -2, -1, 0, 1 }, { 1, 4, 3, 0 }, { 2, -5, 2, 5 }, { 3, -4, 1, -2 } };
            double[,] arrayD = { { 4, 2, 5, 0 }, { 1, 0, -2, 1 }, { 1, 4, 1, 7 }, { -3, 2, -1, 0 } };



            var a = new Matrix(arrayA);
            var b = new Matrix(arrayB);
            var d = new Matrix(arrayD);

            Console.WriteLine("a = ");
            Console.WriteLine(a);

            Console.WriteLine("b =");
            Console.WriteLine(b);

            Console.WriteLine("d =");
            Console.WriteLine(d);

            Matrix[] arrayMatrix = { a, b, d };

            // Checking the dimension of matrices for addition and subtraction
            var check = true;

            for (var i = 1; i < arrayMatrix.Length; i++)
            {
                if (arrayMatrix[0].Array.GetLength(0) != arrayMatrix[i].Array.GetLength(0) ||
                    arrayMatrix[0].Array.GetLength(1) != arrayMatrix[i].Array.GetLength(1))
                {
                    check = false;
                }
            }

            if (check)
            {
                Console.WriteLine("Addition(a, b, d) =");
                //Addition
                Console.WriteLine(Matrix.Addition(arrayMatrix, (short) Sign.Plus));
                Console.WriteLine("Subtraction(a, b, d) =");
                //Subtraction
                Console.WriteLine(Matrix.Addition(arrayMatrix, (short) Sign.Minus));
            }
            else
            {
                Console.WriteLine("Ошибка сложения и вычитания матриц: Все матрицы(вектора) для вычитания и сложения должны быть одинаковых размерностей");
                Console.WriteLine();
            }

            //Multiplication by a scalar
            Console.WriteLine("MultiplicationByScalar(a,6) =");
            Console.WriteLine(Matrix.MultiplicationByScalar(a, 6));

            //Checking the dimensions of matrices for multiplication
            check = true;
            var composition = new Matrix(arrayMatrix[0]);

            for (var i = 1; i < arrayMatrix.Length; i++)
            {
                if (composition.Array.GetLength(1) != arrayMatrix[i].Array.GetLength(0))
                {
                    check = false;
                }
                else
                {
                    composition = Matrix.Multiplication(composition, arrayMatrix[i]);
                }

            }

            if (check)
            {
                //Multiplication
                Console.WriteLine("Multiplication(a,b,d) =");
                Console.WriteLine(Matrix.Multiplication(arrayMatrix));
            }
            else
            {
                Console.WriteLine("Ошибка умножения матриц: Количество строк первого множителя(матрицы) должно быть равны количеству строк второго множителя(матрицы)");
                Console.WriteLine();
            }

            //Checking the dimension of the matrix for raising to a power, determinant, inverse matrix, etc.
            check = true;

            if (d.Array.GetLength(0) != d.Array.GetLength(1))
            {
                check = false;
            }

            if (check)
            {
                //Exponentiation
                Console.WriteLine("Exponentiation(d) =");
                Console.WriteLine(Matrix.Exponentiation(d, 5));

                //Determinant
                Console.WriteLine("Determinant(d) = {0}", Matrix.Determinant(d));
                Console.WriteLine();

                //Inverse matrix
                Console.WriteLine("Inverse(d) =");
                Console.WriteLine(Matrix.Inverse(d));

                //Matrix of minors for elements dij
                Console.WriteLine("MatrixMinors(d) = ");
                Console.WriteLine(Matrix.Minor(d));

                //Sum of elements of the main diagonal of the matrix
                Console.WriteLine("SumDiagonalElements(d) = {0}", Matrix.SumDiagonalElements(d));
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Ошибка: Для операций возведения в степень, нахождения матрицы миноров, определителя, обратной матрицы, суммы диагональных элементов матрица должна быть квадратной \n");
            }

            //Transposition
            Console.WriteLine("Transposition(d) =");
            Console.WriteLine(Matrix.Transposition(d));

            //Rang
            Console.WriteLine("Rang(d) = {0}", Matrix.Rang(d));
        }
    }
}
