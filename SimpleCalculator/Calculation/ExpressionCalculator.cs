using SimpleCalculator.Operators;
using System.Text;

namespace SimpleCalculator.Calculation
{
    // Tokenize the raw string into numbers, operators, and parentheses.
    // Convert the infix token list into postfix (RPN) order.
    // Evaluate the postfix expression to a single numeric result.
    public class ExpressionCalculator
    {
        // Special token representing unary negation (distinct from binary subtraction, which uses the same '-' character in the input).
        // negation rather than subtraction.
        private const string UnaryMinusToken = "#";
        private const string OpenParen = "(";
        private const string CloseParen = ")";

        // Arithmetic Operators
        private static readonly string BinaryOperatorSymbols = OperatorFactory.GetBinaryOperatorSymbols();



        public double Evaluate(string expression)
        {
            string cleanedExpression = expression.Replace(" ", "");
            List<string> tokens = Tokenize(cleanedExpression);
            Queue<string> postfixTokens = ConvertToPostfix(tokens);
            return EvaluatePostfix(postfixTokens);
        }


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
            bool afterAnotherOperator = tokens.Count > 0 && BinaryOperatorSymbols.Contains(tokens[tokens.Count - 1]);

            return isFirstToken || afterOpenParen || afterAnotherOperator;
        }

 
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
            Operator stackOperator = OperatorFactory.GetOperator(stackToken);
            Operator incomingOperator = OperatorFactory.GetOperator(incomingToken);

            bool isHigherPrecedence = stackOperator.Precedence > incomingOperator.Precedence;

            // '^' is right-associative, so equal-precedence '^' should NOT be popped (it stacks up for right-to-left evaluation).
            // Every other operator is left-associative.
            bool isEqualPrecedenceAndLeftAssociative = stackOperator.Precedence == incomingOperator.Precedence && !incomingOperator.IsRightAssociative;

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
                else
                {
                    ApplyOperator(OperatorFactory.GetOperator(token), evalStack);
                }
            }

            if (evalStack.Count != 1)
            {
                throw new FormatException("Failed to evaluate expression.");
            }

            return evalStack.Pop();
        }

        private static void ApplyOperator(Operator op, Stack<double> evalStack)
        {
            if (evalStack.Count < op.OperandCount)
            {
                throw new FormatException("Invalid expression structure.");
            }

            double[] operands = new double[op.OperandCount];
            for (int i = op.OperandCount - 1; i >= 0; i--)
            {
                operands[i] = evalStack.Pop();
            }

            evalStack.Push(op.Apply(operands));
        }
    }
}
