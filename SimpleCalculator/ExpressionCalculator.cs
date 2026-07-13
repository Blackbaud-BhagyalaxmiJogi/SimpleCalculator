using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    // This class takes an expression
    // convert that infix to postfix expression
    // evaluate the postfix expression
    // result is loged and printed on console
    static class ExpressionCalculator
    {
        // Special token representing unary negation (distinct from binary subtraction, which uses the same '-' character in the input).
        // negation rather than subtraction (e.g. "3*-5", "(-5+2)", "-5+3").
        private const string UnaryMinusToken = "#";
        private const string OpenParen = "(";
        private const string CloseParen = ")";

        // Arthematic Operators
        private const string BinaryOperatorChars = "+-*/%^";

        // To store Operators Precedence
        private static readonly Dictionary<string, int> OperatorPrecedence = new Dictionary<string, int>
        {
            { UnaryMinusToken, 4 },
            { "^", 3 },
            { "*", 2 },
            { "/", 2 },
            { "%", 2 },
            { "+", 1 },
            { "-", 1 }
        };

        public static double Evaluate(string expression)
        {
            string cleanedExpression = expression.Replace(" ", "");
            List<string> tokens = Tokenize(cleanedExpression);
            Queue<string> postfixTokens = ConvertToPostfix(tokens);
            return EvaluatePostfix(postfixTokens);
        }

        // Step 1: Tokenizing

        private static List<string> Tokenize(string expression)
        {
            List<string> tokens = new List<string>();
            int position = 0;

            while (position < expression.Length)
            {
                char currentChar = expression[position];

                if (char.IsDigit(currentChar) || currentChar == '.')
                {
                    position = ReadNumberToken(expression, position, tokens);
                }
                else if (IsUnaryMinus(currentChar, tokens))
                {
                    tokens.Add(UnaryMinusToken);
                    position++;
                }
                else
                {
                    tokens.Add(currentChar.ToString());
                    position++;
                }
            }

            return tokens;
        }

        private static int ReadNumberToken(string expression, int startPosition, List<string> tokens)
        {
            StringBuilder numberBuilder = new StringBuilder();
            int position = startPosition;

            while (position < expression.Length && (char.IsDigit(expression[position]) || expression[position] == '.'))
            {
                numberBuilder.Append(expression[position]);
                position++;
            }

            tokens.Add(numberBuilder.ToString());
            return position;
        }

        private static bool IsUnaryMinus(char currentChar, List<string> tokens)
        {
            if (currentChar != '-')
            {
                return false;
            }

            bool isFirstToken = tokens.Count == 0;
            bool afterOpenParen = tokens.Count > 0 && tokens[tokens.Count - 1] == OpenParen;
            bool afterAnotherOperator = tokens.Count > 0 && BinaryOperatorChars.Contains(tokens[tokens.Count - 1]);

            return isFirstToken || afterOpenParen || afterAnotherOperator;
        }

 
        // Step 2: Infix -> Postfix (Shunting-Yard Algorithm)

        private static Queue<string> ConvertToPostfix(List<string> tokens)
        {
            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    outputQueue.Enqueue(token);
                }
                else if (token == OpenParen)
                {
                    operatorStack.Push(token);
                }
                else if (token == CloseParen)
                {
                    MoveOperatorsUntilOpenParen(operatorStack, outputQueue);
                }
                else
                {
                    MoveHigherPrecedenceOperators(token, operatorStack, outputQueue);
                    operatorStack.Push(token);
                }
            }

            DrainRemainingOperators(operatorStack, outputQueue);

            return outputQueue;
        }

        private static void MoveOperatorsUntilOpenParen(Stack<string> operatorStack, Queue<string> outputQueue)
        {
            while (operatorStack.Count > 0 && operatorStack.Peek() != OpenParen)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            if (operatorStack.Count == 0)
            {
                throw new FormatException("Mismatched parentheses.");
            }

            operatorStack.Pop(); // lastly removing the matching '('
        }

        private static void MoveHigherPrecedenceOperators(string incomingToken, Stack<string> operatorStack, Queue<string> outputQueue)
        {
            while (operatorStack.Count > 0 && operatorStack.Peek() != OpenParen && HasPriorityOver(operatorStack.Peek(), incomingToken))
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }
        }

        private static bool HasPriorityOver(string stackToken, string incomingToken)
        {
            int stackPrecedence = OperatorPrecedence[stackToken];
            int incomingPrecedence = OperatorPrecedence[incomingToken];

            bool isHigherPrecedence = stackPrecedence > incomingPrecedence;

            // '^' is right-associative, so equal-precedence '^' should NOT be popped (it stacks up for right-to-left evaluation).
            // Every other operator is left-associative.
            bool isEqualPrecedenceAndLeftAssociative = stackPrecedence == incomingPrecedence && incomingToken != "^";

            return isHigherPrecedence || isEqualPrecedenceAndLeftAssociative;
        }

        private static void DrainRemainingOperators(Stack<string> operatorStack, Queue<string> outputQueue)
        {
            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == OpenParen)
                {
                    throw new FormatException("Mismatched parentheses.");
                }

                outputQueue.Enqueue(operatorStack.Pop());
            }
        }

        // Step 3: Evaluating the Postfix Expression


        private static double EvaluatePostfix(Queue<string> postfixTokens)
        {
            Stack<double> evalStack = new Stack<double>();

            while (postfixTokens.Count > 0)
            {
                string token = postfixTokens.Dequeue();

                if (double.TryParse(token, out double number))
                {
                    evalStack.Push(number);
                }
                else if (token == UnaryMinusToken)
                {
                    ApplyUnaryMinus(evalStack);
                }
                else
                {
                    ApplyBinaryOperator(token, evalStack);
                }
            }

            if (evalStack.Count != 1)
            {
                throw new FormatException("Failed to evaluate expression.");
            }

            return evalStack.Pop();
        }

        private static void ApplyUnaryMinus(Stack<double> evalStack)
        {
            if (evalStack.Count < 1)
            {
                throw new FormatException("Invalid expression structure.");
            }

            evalStack.Push(-evalStack.Pop());
        }

        private static void ApplyBinaryOperator(string op, Stack<double> evalStack)
        {
            if (evalStack.Count < 2)
            {
                throw new FormatException("Invalid expression structure.");
            }

            double right = evalStack.Pop();
            double left = evalStack.Pop();

            switch (op)
            {
                case "+":
                    evalStack.Push(left + right);
                    break;
                case "-":
                    evalStack.Push(left - right);
                    break;
                case "*":
                    evalStack.Push(left * right);
                    break;
                case "/":
                    if (right == 0)
                    {
                        throw new DivideByZeroException("Division by zero!");
                    }
                    evalStack.Push(left / right);
                    break;
                case "%":
                    if (right == 0)
                    {
                        throw new DivideByZeroException("Modulo by zero!");
                    }
                    evalStack.Push(left % right);
                    break;
                case "^":
                    evalStack.Push(Math.Pow(left, right));
                    break;
                default:
                    throw new FormatException($"Unknown operator '{op}'.");
            }
        }
    }
}
