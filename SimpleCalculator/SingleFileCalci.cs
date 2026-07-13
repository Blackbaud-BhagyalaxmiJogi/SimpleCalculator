using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    public class SingleFileCalci
    {
        private static readonly string logFilePath = "expression_history.txt";
        private static List<string> calculationHistory = new List<string>();

        static void Main1(string[] args)
        {
            bool Running = true;
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("            Welcome to the Calculator             ");
            Console.WriteLine("--------------------------------------------------");

            while (Running)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EvaluateExpression();
                        break;
                    case "2":
                        EvaluateExpressionsFromFile();
                        break;
                    case "3":
                        ShowHistory();
                        break;
                    case "4":
                        ClearHistory();
                        break;
                    case "5":
                        Running = false;
                        Console.WriteLine("\nGoodbye!");
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection.");
                        Console.ResetColor();
                        break;
                }


            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\nMAIN MENU:");
            Console.WriteLine("1. Enter an Expression Manually");
            Console.WriteLine("2. Process Expressions from a File (.txt)");
            Console.WriteLine("3. View Evaluation History File");
            Console.WriteLine("4. Clear History");
            Console.WriteLine("5. Exit");
            Console.Write("Choice: ");
        }

        static void EvaluateExpression()
        {
            Console.Clear();
            Console.WriteLine("Enter your full expression (e.g., 6+5*(4-2.6)/80%3^2):");
            Console.Write("> ");
            string input = Console.ReadLine();

            try
            {
                double result = Evaluate(input);
                string entry = $"{input} = {result}";

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nResult: {result}");
                Console.ResetColor();

                LogResult(entry);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Syntax/Math Error: {ex.Message}");
                Console.ResetColor();
            }
        }


        ///  This method Reads expressions from an external text file, processes them, and logs the final output.

        static void EvaluateExpressionsFromFile()
        {
            Console.Clear();
            Console.WriteLine("Process Expressions From File");
            Console.Write("Enter the path to your .txt file (or filename if in the same folder): ");
            string inputFilePath = Console.ReadLine();


            if (!File.Exists(inputFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File not found. Make sure the filename/path is correct.");
                Console.ResetColor();
                return;
            }

            try
            {
                // File Handling: Read all lines from the source file
                string[] lines = File.ReadAllLines(inputFilePath);
                Console.WriteLine($"\nFound {lines.Length} lines. Processing...\n");
                Console.WriteLine(new string('-', 40));

                int count = 1;
                foreach (string line in lines)
                {
                    // Skip empty lines or whitespace strings
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        double result = Evaluate(line);
                        string entry = $"{line.Trim()} = {result}";

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"[{count}] SUCCESS: {entry}");
                        Console.ResetColor();

                        LogResult(entry);
                    }
                    catch (Exception ex)
                    {
                        // Catch bad syntax on specific lines without crashing the entire loop
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[{count}] ERROR on expression '{line.Trim()}': {ex.Message}");
                        Console.ResetColor();
                    }
                    count++;
                }
                Console.WriteLine(new string('-', 40));
                Console.WriteLine("Batch processing complete.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An I/O error occurred while reading the file: {ex.Message}");
            }
        }

        static void LogResult(string entry)
        {
            // Collections
            calculationHistory.Add(entry);
            // File Handling
            File.AppendAllText(logFilePath, $"[{DateTime.Now}] {entry}\n");
        }

        // SHUNTING-YARD ALGORITHM PARSER 
        public static double Evaluate(string expression)
        {
            expression = expression.Replace(" ", "");
            List<string> tokens = Tokenize(expression);
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            Dictionary<string, int> precedence = new Dictionary<string, int>()
            {
                {"#", 4}, {"^", 3}, {"*", 2}, {"/", 2}, {"%", 2}, {"+", 1}, {"-", 1}
            };

            for (int i = 0; i < tokens.Count; i++)
            {
                string token = tokens[i];

                if (double.TryParse(token, out _))
                {
                    outputQueue.Enqueue(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0) throw new Exception("Mismatched parentheses.");
                    operatorStack.Pop();
                }
                else
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(" &&
                           (precedence[operatorStack.Peek()] > precedence[token] ||
                           (precedence[operatorStack.Peek()] == precedence[token] && token != "^")))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "(") throw new Exception("Mismatched parentheses.");
                outputQueue.Enqueue(operatorStack.Pop());
            }

            Stack<double> evalStack = new Stack<double>();

            while (outputQueue.Count > 0)
            {
                string token = outputQueue.Dequeue();

                if (double.TryParse(token, out double number))
                {
                    evalStack.Push(number);
                }
                else
                {
                    if (token == "#")
                    {
                        if (evalStack.Count < 1) throw new Exception("Invalid expression structure.");
                        evalStack.Push(-evalStack.Pop());
                        continue;
                    }

                    if (evalStack.Count < 2) throw new Exception("Invalid expression structure.");
                    double b = evalStack.Pop();
                    double a = evalStack.Pop();

                    switch (token)
                    {
                        case "+": evalStack.Push(a + b); break;
                        case "-": evalStack.Push(a - b); break;
                        case "*": evalStack.Push(a * b); break;
                        case "/":
                            if (b == 0) throw new DivideByZeroException("Division by zero!");
                            evalStack.Push(a / b);
                            break;
                        case "%":
                            if (b == 0) throw new DivideByZeroException("Modulo by zero!");
                            evalStack.Push(a % b);
                            break;
                        case "^": evalStack.Push(Math.Pow(a, b)); break;
                    }
                }
            }

            if (evalStack.Count != 1) throw new Exception("Failed to evaluate expression.");
            return evalStack.Pop();
        }

        private static List<string> Tokenize(string expr)
        {
            List<string> tokens = new List<string>();
            int i = 0;

            while (i < expr.Length)
            {
                char c = expr[i];

                if (char.IsDigit(c) || c == '.')
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.'))
                    {
                        sb.Append(expr[i]);
                        i++;
                    }
                    tokens.Add(sb.ToString());
                }
                else
                {
                    if (c == '-' && (tokens.Count == 0 || tokens[tokens.Count - 1] == "(" || "+-*/%^".Contains(tokens[tokens.Count - 1])))
                    {
                        tokens.Add("#");
                    }
                    else
                    {
                        tokens.Add(c.ToString());
                    }
                    i++;
                }
            }
            return tokens;
        }

        static void ShowHistory()
        {
            Console.Clear();
            if (File.Exists(logFilePath))
            {
                string[] logs = File.ReadAllLines(logFilePath);
                foreach (var log in logs) Console.WriteLine(log);
            }
            else Console.WriteLine("History empty.");
        }

        static void ClearHistory()
        {
            if (File.Exists(logFilePath)) File.Delete(logFilePath);
            calculationHistory.Clear();
            Console.WriteLine("History wiped cleanly.");
        }
    }
}
