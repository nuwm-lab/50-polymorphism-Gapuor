using System;
using System.Text;

namespace SystemsExample
{
    // Базовий клас для систем рівняння NxN. Зроблено public.
    public class SystemNxN
    {
        // Поле для зберігання розміру системи
        protected int _size;
        // Матриця коефіцієнтів A
        protected double[,] _a;
        // Вектор правих частин B
        protected double[] _b;
        // Константа для порівняння чисел з плаваючою комою
        protected const double EPSILON = 1e-6;

        public int Size => _size;

        public SystemNxN(int n)
        {
            _size = n;
            _a = new double[n, n];
            _b = new double[n];
        }

        // Деструктор (викликається збирачем сміття)
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

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Ввід не може бути порожнім. Спробуйте ще раз.");
                    continue;
                }

                if (double.TryParse(input.Replace('.', ','), out double result) ||
                    double.TryParse(input.Replace(',', '.'), out result))
                    return result;

                Console.WriteLine("Некоректне значення. Введіть число ще раз.");
            }
        }

        // Введення коефіцієнтів
        public virtual void SetCoefficients()
        {
            Console.WriteLine($"\nВведіть коефіцієнти для системи {_size}×{_size}:");
            for (int i = 0; i < _size; i++)
            {
                Console.WriteLine($"\n-- Рівняння {i + 1} --");
                for (int j = 0; j < _size; j++)
                    _a[i, j] = ReadDouble($"a[{i + 1},{j + 1}] (x{j + 1}) = ");
                _b[i] = ReadDouble($"b[{i + 1}] (права частина) = ");
            }
        }

        // Виведення системи у стандартному рівнянному форматі
        public virtual void Display()
        {
            Console.WriteLine($"\nСистема рівнянь {_size}×{_size} (стандартний вивід):");
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                    // Використання форматування для кращого вигляду
                    Console.Write($"{_a[i, j]:F4}*x{j + 1}" + (j < _size - 1 ? " + " : ""));
                Console.WriteLine($" = {_b[i]:F4}");
            }
        }

        // Перевірка вектора (використовує override для поліморфізму)
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
                
                // Виведення результату перевірки для кожного рівняння
                string status = Math.Abs(left - _b[i]) > EPSILON ? "НЕ ЗБІГАЄТЬСЯ" : "ЗБІГАЄТЬСЯ";
                Console.WriteLine($"Рівняння {i + 1}: {left:F6} ≈ {_b[i]:F6} ({status})");
                
                if (Math.Abs(left - _b[i]) > EPSILON)
                    satisfies = false;
            }

            Console.WriteLine(satisfies
                ? "\nВектор задовольняє систему (в межах EPSILON)."
                : "\nВектор НЕ задовольняє систему.");
        }

        // Введення вектора
        public double[] ReadVector()
        {
            double[] X = new double[_size];
            Console.WriteLine($"\nВведіть компоненти вектора розв'язку (розміром {_size}):");
            for (int i = 0; i < _size; i++)
                X[i] = ReadDouble($"x{i + 1} = ");
            return X;
        }
    }

    // Похідний клас для системи 2×2. Зроблено public.
    public class System2x2 : SystemNxN
    {
        public System2x2() : base(2)
        {
            Console.WriteLine("\nСтворено об'єкт системи 2×2");
        }

        // Деструктор
        ~System2x2()
        {
            Console.WriteLine("Об'єкт системи 2×2 знищено.");
        }

        public override void SetCoefficients()
        {
            Console.WriteLine("\nНалаштування системи 2×2:");
            base.SetCoefficients();
        }

        // ДЕМОНСТРАЦІЯ ПРИХОВУВАННЯ (Method Hiding)
        // Використання 'new' замість 'override'. Якщо викликати через SystemNxN (базовий тип), 
        // буде викликаний метод Display() SystemNxN. Якщо викликати через System2x2, то цей метод.
        public new void Display()
        {
            Console.WriteLine("\nВивід системи 2×2 (ПРИХОВАНИЙ НОВИЙ ФОРМАТ):");
            // Вивід у компактному матричному вигляді
            Console.WriteLine($"| {_a[0, 0],10:F4}  {_a[0, 1],10:F4} |  | x1 |  =  | {_b[0],10:F4} |");
            Console.WriteLine($"| {_a[1, 0],10:F4}  {_a[1, 1],10:F4} |  | x2 |  =  | {_b[1],10:F4} |");
        }
        
        // СУТТЄВА ВІДМІННІСТЬ: Метод для розв'язання та перевірки на виродженість
        public double[] Solve2x2()
        {
            double det = _a[0, 0] * _a[1, 1] - _a[0, 1] * _a[1, 0];
            Console.WriteLine($"\nОбчислення визначника (Determinant) для системи 2×2: Det = {det:F6}");

            if (Math.Abs(det) < EPSILON)
            {
                Console.WriteLine("Система є ВИРОДЖЕНОЮ (degenerate): розв'язків нескінченно багато або їх немає.");
                return new double[0]; // Повертаємо порожній масив, що вказує на відсутність єдиного розв'язку
            }

            double detX1 = _b[0] * _a[1, 1] - _a[0, 1] * _b[1];
            double detX2 = _a[0, 0] * _b[1] - _b[0] * _a[1, 0];

            double x1 = detX1 / det;
            double x2 = detX2 / det;

            Console.WriteLine($"Унікальний розв'язок знайдено (за правилом Крамера): x1 = {x1:F6}, x2 = {x2:F6}");
            return new double[] { x1, x2 };
        }


        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\nПеревірка рішення для системи 2×2 (Динамічний Поліморфізм):");
            base.CheckVector(X);
        }
    }

    // Похідний клас для системи 3×3. Зроблено public.
    public class SLAR3x3 : SystemNxN
    {
        public SLAR3x3() : base(3)
        {
            Console.WriteLine("\nСтворено об'єкт системи 3×3");
        }

        // Деструктор
        ~SLAR3x3()
        {
            Console.WriteLine("Об'єкт системи 3×3 знищено.");
        }

        public override void SetCoefficients()
        {
            Console.WriteLine("\nНалаштування системи 3×3:");
            base.SetCoefficients();
        }

        // ДЕМОНСТРАЦІЯ ПЕРЕВИЗНАЧЕННЯ (Dynamic Polymorphism) та інший формат виводу
        // Завжди викликається цей метод, навіть якщо об'єкт передано як SystemNxN
        public override void Display()
        {
            Console.WriteLine("\nВивід системи 3×3 (Розширена матриця [A|B] — Динамічний Поліморфізм):");
            // Вивід у форматі розширеної матриці [A | B]
            for (int i = 0; i < _size; i++)
            {
                Console.Write("|");
                for (int j = 0; j < _size; j++)
                    Console.Write($" {_a[i, j],10:F4}");
                Console.WriteLine($" | {_b[i],10:F4} |");
            }
        }

        public override void CheckVector(double[] X)
        {
            Console.WriteLine("\nПеревірка рішення для системи 3×3 (Динамічний Поліморфізм):");
            base.CheckVector(X);
        }
    }

    // Основний клас програми. Зроблено public.
    public class Program
    {
        // Демонстрація динамічного поліморфізму (приймає базовий тип)
        public static void DemonstratePolymorphism(SystemNxN sys)
        {
            Console.WriteLine("\n=======================================================");
            Console.WriteLine("--- Демонстрація поліморфізму (викликано через SystemNxN) ---");
            Console.WriteLine($"Статичний тип (аргумент): SystemNxN. Динамічний тип (об'єкт): {sys.GetType().Name}");
            Console.WriteLine("=======================================================");

            // 1. Демонстрація Display:
            //   - Для SLAR3x3 викличеться override (SLAR3x3.Display) -> Динамічний поліморфізм
            //   - Для System2x2 викличеться метод БАЗОВОГО класу SystemNxN -> Приховування (new) ігнорується
            Console.WriteLine("\n[1] Виклик .Display() (Перевірка override vs. new):");
            sys.Display(); 

            // 2. Демонстрація CheckVector:
            //   - Завжди викличеться override (SLAR3x3.CheckVector або System2x2.CheckVector)
            Console.WriteLine("\n[2] Виклик .CheckVector() (Перевірка override):");
            
            double[] testVector;
            // Спроба використати унікальну функціональність похідного класу через перетворення
            if (sys.Size == 2 && sys is System2x2 sys2x2)
            {
                // Використовуємо додатковий функціонал (перевірка на виродженість)
                testVector = sys2x2.Solve2x2();
                
                if (testVector.Length > 0)
                {
                    Console.WriteLine("\nПеревіряємо автоматично розрахований розв'язок:");
                    sys.CheckVector(testVector); // Динамічний поліморфізм працює для override
                }
            }
            else
            {
                // Для інших систем просимо ввести тестовий вектор
                testVector = sys.ReadVector();
                sys.CheckVector(testVector);
            }

            Console.WriteLine("--- Кінець демонстрації поліморфізму ---\n");
        }

        static void Main()
        {
            // Встановлення кодування для коректного відображення українських символів
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                // --- Створення та налаштування System2x2 ---
                var sys2 = new System2x2();
                sys2.SetCoefficients();
                
                // Статичний виклик (працює приховування)
                Console.WriteLine("\n--- Виклик Display напряму на об'єкті System2x2 (HIDING) ---");
                sys2.Display(); // Викликається System2x2.Display() (новий, прихований)
                
                // Демонстрація динамічного поліморфізму
                DemonstratePolymorphism(sys2); // Тут Display() викличеться БАЗОВИЙ SystemNxN.Display()
                                              // через статичний тип SystemNxN!

                // --- Створення та налаштування SLAR3x3 ---
                var sys3 = new SLAR3x3();
                sys3.SetCoefficients();
                
                // Статичний виклик (працює перевизначення)
                Console.WriteLine("\n--- Виклик Display напряму на об'єкті SLAR3x3 (OVERRIDE) ---");
                sys3.Display(); // Викликається SLAR3x3.Display() (перевизначений)
                
                // Демонстрація динамічного поліморфізму
                DemonstratePolymorphism(sys3); // Тут Display() викличеться SLAR3x3.Display()
                                              // завдяки override і динамічному поліморфізму.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПомилка виконання: {ex.Message}");
            }

            // Видалення Console.ReadKey() згідно з вимогою
        }
    }
}