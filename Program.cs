using System;

namespace SystemsExample
{
    // Базовий клас для систем рівнянь NxN
    class SystemNxN
    {
        protected int _size;
        protected double[,] _a;
        protected double[] _b;
        protected const double EPSILON = 1e-6;

        public int Size => _size;

        public SystemNxN(int n)
        {
            _size = n;
            _a = new double[n, n];
            _b = new double[n];
        }

        ~SystemNxN()
        {
            Console.WriteLine($"Об'єкт SystemNxN розміром {_size}×{_size} знищено.");
        }

        // Безпечне зчитування числа
        protected double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input == null)
                {
                    Console.WriteLine("Ввід перервано. Спробуйте ще раз.");
                    continue;
                }

                if (double.TryParse(input, out double result))
                    return result;

                Console.WriteLine("Некоректне значення. Введіть число ще раз.");
            }
        }

        //Введення коефіцієнтів системи
        public virtual void SetCoefficients()
        {
            Console.WriteLine($"\nВведіть коефіцієнти для системи {_size}×{_size}:");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                    _a[i, j] = ReadDouble($"a[{i + 1},{j + 1}] = ");
                _b[i] = ReadDouble($"b[{i + 1}] = ");
            }
        }

        // Виведення системи
        public virtual void Display()
        {
            Console.WriteLine($"\nСистема рівнянь {_size}×{_size}:");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                    Console.Write($"{_a[i, j]}*x{j + 1}" + (j < _size - 1 ? " + " : ""));
                Console.WriteLine($" = {_b[i]}");
            }
        }

        // Перевірка вектора
        public virtual void CheckVector(double[] X)
        {
            if (X.Length != _size)
            {
                Console.WriteLine($"Довжина вектора {X.Length} не відповідає розміру системи {_size}.");
                return;
            }

            Console.WriteLine($"\nПеревірка вектора розміром {_size}:");
            bool satisfies = true;

            for (int i = 0; i < _size; i++)
            {
                double left = 0;
                for (int j = 0; j < _size; j++)
                    left += _a[i, j] * X[j];

                Console.WriteLine($"Рівняння {i + 1}: {left} ≈ {_b[i]}");
                if (Math.Abs(left - _b[i]) > EPSILON)
                    satisfies = false;
            }

            Console.WriteLine(satisfies
                ? "Вектор задовольняє систему."
                : "Вектор не задовольняє систему.");
        }

        // Введення вектора
        public double[] ReadVector()
        {
            double[] X = new double[_size];
            Console.WriteLine($"\nВведіть компоненти вектора розміром {_size}:");
            for (int i = 0; i < _size; i++)
                X[i] = ReadDouble($"x{i + 1} = ");
            return X;
        }
    }

    // Похідний клас для системи 2×2
    class System2x2 : SystemNxN
    {
        public System2x2() : base(2)
        {
            Console.WriteLine("\nСтворено об'єкт системи 2×2");
        }

        ~System2x2()
        {
            Console.WriteLine("Об'єкт системи 2×2 знищено.");
        }

        public override void SetCoefficients()
        {
            Console.WriteLine("\nНалаштування системи 2×2:");
            base.SetCoefficients();
        }

        public override void Display()
        {
            Console.WriteLine("\nВивід системи 2×2:");
            base.Display();
        }

        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\nПеревірка рішення для системи 2×2:");
            base.CheckVector(X);
        }
    }

    // Похідний клас для системи 3×3
    class SLAR3x3 : SystemNxN
    {
        public SLAR3x3() : base(3)
        {
            Console.WriteLine("\nСтворено об'єкт системи 3×3");
        }

        ~SLAR3x3()
        {
            Console.WriteLine("Об'єкт системи 3×3 знищено.");
        }

        public override void SetCoefficients()
        {
            Console.WriteLine("\nНалаштування системи 3×3:");
            base.SetCoefficients();
        }

        public override void Display()
        {
            Console.WriteLine("\nВивід системи 3×3:");
            base.Display();
        }

        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\nПеревірка рішення для системи 3×3:");
            base.CheckVector(X);
        }
    }

    // Основний клас програми
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                var sys2 = new System2x2();
                sys2.SetCoefficients();
                sys2.Display();
                var X2 = sys2.ReadVector();
                sys2.CheckVector(X2);

                var sys3 = new SLAR3x3();
                sys3.SetCoefficients();
                sys3.Display();
                var X3 = sys3.ReadVector();
                sys3.CheckVector(X3);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка виконання: {ex.Message}");
            }

            Console.WriteLine("\nПрограму завершено. Натисніть будь-яку клавішу...");
            Console.ReadKey();
        }
    }
}
