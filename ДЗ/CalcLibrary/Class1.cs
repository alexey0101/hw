using System;
using System.IO;
using System.Collections.Generic;

namespace CalcLibrary
{
    public delegate double MathOperation(double a, double b);

    public delegate void ErrorNotificationType(string message);
    public class Calculator
    {
        public event ErrorNotificationType ErrorNotification;

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

            double operandA;
            double operandB;

            //Проверка на переполнение
            if (!double.TryParse(exprFragm[0], out operandA))
            {
                throw new OverflowException("∞");
            }
            //Проверка на переполнение
            if (!double.TryParse(exprFragm[2], out operandB))
            {
                throw new OverflowException("∞");
            }
            string operatorExpr = exprFragm[1];
            double operationValue;

            if (operations.ContainsKey(operatorExpr)) //Проверяем наличие данного ключа в словаре.
            {
                operationValue = operations[operatorExpr](operandA, operandB);

                if (operatorExpr == "/" && operandB == 0)
                {
                    throw new DivideByZeroException("bruh");
                }
                if (double.IsInfinity(operationValue))
                {
                    throw new OverflowException("∞");
                }
                if (double.IsNaN(operationValue))
                {
                    throw new ArgumentException("не число");
                }

                return operationValue;
            }
            else
            {
                throw new ArgumentException("неверный оператор");
            }
        }
        /// <summary>
        /// Считает значения выражений, записанный в файле expressions.txt
        /// </summary>
        public void CalculateExpressions()
        {
            string[] expressions = File.ReadAllLines("../../expressions.txt"); //Считываем все выражения.
            double expressionValue = 0;
            bool flag;

            File.WriteAllText("../../answers.txt", "");

            foreach (string expression in expressions)
            {
                flag = true;
                try
                {
                    expressionValue = Calculate(expression); //Вычисление значения выражения
                }
                catch (DivideByZeroException ex)
                {
                    this.ErrorNotification(ex.Message);
                    flag = false;
                }
                catch (OverflowException ex)
                {
                    this.ErrorNotification(ex.Message);
                    flag = false;
                }
                catch (ArgumentException ex)
                {
                    this.ErrorNotification(ex.Message);
                    flag = false;
                }
                catch (Exception ex)
                {
                    this.ErrorNotification(ex.Message);
                    flag = false;
                }
                if (flag)
                {
                    File.AppendAllText("../../answers.txt", String.Format("{0:f3}", expressionValue) + Environment.NewLine); //Запись в файл значения выражения
                }
            }
        }

    }
}

