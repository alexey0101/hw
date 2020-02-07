using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
 * Карякин Алексей
 * БПИ1910(1)
*/
namespace ДЗ
{
    class Program
    {
        public delegate double MathOperation(double a, double b);

        //Создаём словарь и инициализируем его необходимыми нам операциями.
        static Dictionary<string, MathOperation> operations = new Dictionary<string, MathOperation>
        {
            ["+"] = (x, y) => x + y,
            ["-"] = (x, y) => x - y,
            ["*"] = (x, y) => x * y,
            ["/"] = (x, y) => x / y,
            ["^"] = (x, y) => Math.Pow(x, y)
        };

        /// <summary>
        /// Принимает на вход строку вида "A + B", где + - некоторая операция, вычисляет значение выражения.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>Значение выражения "A + B"</returns>
        public static double Calculate(string expr)
        {
            string[] exprFragm = expr.Split(' '); //Разбиваем строку на 'A', '+', 'B'
            double operandA = double.Parse(exprFragm[0]);
            double operandB = double.Parse(exprFragm[2]);
            string operatorExpr = exprFragm[1];

            if (operations.ContainsKey(operatorExpr)) //Проверяем наличие данного ключа в словаре.
            {
                return operations[operatorExpr](operandA, operandB);
            }
            else
            {
                throw new ArgumentException("Invalid operator");
            }
        }


        /// <summary>
        /// Считает значения выражений, записанный в файле expressions.txt
        /// </summary>
        static void CalculateExpressions()
        {
            string[] expressions = File.ReadAllLines("expressions.txt"); //Считываем все выражения.
            double expressionValue;

            File.WriteAllText("answers.txt", "");

            foreach (string expression in expressions) 
            {
                expressionValue = Calculate(expression); //Вычисление значения выражения

                File.AppendAllText("answers.txt", String.Format("{0:f3}", expressionValue) + Environment.NewLine); //Запись в файл значения выражения
            }
        }

        //Я сначала подумал, что нужно просто откинуть цифры после 3-й. Без округления. Получилось много ошибок, а их как я понял быть не должно вообще, но на всякий случай пусть это останется тут.
        /*
        static void CalculateExpressions2()
        {
            string[] expressions = File.ReadAllLines("expressions.txt");
            double expressionValue;

            foreach (string expression in expressions)
            {
                expressionValue = Calculate(expression);

                File.AppendAllText("answers2.txt", DigitsRemoval(expressionValue) + Environment.NewLine);
            }
        }

        
        static string DigitsRemoval(double expressionValue)
        {
            string[] expressionStr = expressionValue.ToString().Split(',');

            if (expressionStr.Length > 1 && expressionStr[1].Length > 3)
            {
                expressionStr[1] = expressionStr[1].Substring(expressionStr[1].IndexOf(',') + 1, 3);
            }

            return String.Join(",", expressionStr);
        }

        static void ValidationCheck2()
        {
            string[] myAnswers = File.ReadAllLines("answers.txt");
            string[] checker = File.ReadAllLines("expressions_checker.txt");
            int mistakes = 0;

            for (int i = 0; i < myAnswers.Length; i++)
            {
                if (myAnswers[i] == checker[i])
                {
                    File.AppendAllText("results2.txt", "OK" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("results2.txt", "Error" + Environment.NewLine);
                    mistakes++;
                }
            }

            File.AppendAllText("results2.txt", mistakes.ToString());
        }*/

        /// <summary>
        /// Сравнивает раннее вычисленные значения из файла answers.txt и значения из файла expressions_checker.txt
        /// </summary>
        static void ValidationCheck()
        {
            string[] myAnswers = File.ReadAllLines("answers.txt"); //Массив с вычисленными раннее значениями.
            string[] checker = File.ReadAllLines("expressions_checker.txt"); //Массив с ответами для проверки.
            int mistakes = 0; //Счётчик ошибок

            File.WriteAllText("results.txt", "");

            for (int i = 0; i < myAnswers.Length; i++)
            {
                if (myAnswers[i] == checker[i])
                {
                    File.AppendAllText("results.txt", "OK" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("results.txt", "Error" + Environment.NewLine);
                    mistakes++;
                }
            }

            File.AppendAllText("results.txt", mistakes.ToString());
        }

        static void Main(string[] args)
        {
            do
            {
                try
                {
                    CalculateExpressions(); //Вычисление выражений из файла expressions.txt
                    ValidationCheck(); //Проверка значений
                    Console.WriteLine("Значения выражений подсчитаны и проверены.");
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Если вы хотите повторить выполнение программы, нажмите любую клавишу, иначе нажмите Esc.");

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape); //Повтор решения

        }
    }
}