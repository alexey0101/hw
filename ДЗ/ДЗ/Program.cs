using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CalcLibrary;

/*
 * Карякин Алексей
 * БПИ1910(1)
*/
namespace ДЗ
{

    class Program
    {
        /// <summary>
        /// Сравнивает раннее вычисленные значения из файла answers.txt и значения из файла expressions_checker.txt
        /// </summary>
        static void ValidationCheck()
        {
            string[] myAnswers = File.ReadAllLines("../../answers.txt"); //Массив с вычисленными раннее значениями.
            string[] checker = File.ReadAllLines("../../expressions_checker.txt"); //Массив с ответами для проверки.
            string[] exprC = File.ReadAllLines("../../expressions.txt");
            int mistakes = 0; //Счётчик ошибок

            File.WriteAllText("results.txt", "");

            for (int i = 0; i < myAnswers.Length; i++)
            {
                if (String.Equals(myAnswers[i], checker[i]))
                {
                    File.AppendAllText("../../results.txt", "OK" + Environment.NewLine);
                }
                else
                {
                    File.AppendAllText("../../results.txt", "Error" + Environment.NewLine);
                    mistakes++;
                }
            }

            File.AppendAllText("../../results.txt", mistakes.ToString());
        }

        /// <summary>
        /// Выводит на консоль сообщение об исключительной ситуации.
        /// </summary>
        /// <param name="message"></param>
        static void ConsoleErrorHandler(string message)
        {
            Console.WriteLine(message + " " + DateTime.Now);
        }

        /// <summary>
        /// Записывает в файл информацию об ошибке.
        /// </summary>
        /// <param name="message"></param>
        static void ResultErrorHandler(string message)
        {
            File.AppendAllText("../../answers.txt", message + Environment.NewLine);
        }

        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Calculator calc = new Calculator();

                    calc.ErrorNotification += ConsoleErrorHandler;
                    calc.ErrorNotification += ResultErrorHandler;
                    calc.CalculateExpressions(); //Вычисление выражений из файла expressions.txt
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