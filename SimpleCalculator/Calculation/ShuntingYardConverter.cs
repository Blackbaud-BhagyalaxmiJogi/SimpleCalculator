

using SimpleCalculator.Operators;

namespace SimpleCalculator.Calculation
{
    public class ShuntingYardConverter
    {
        private const string OpenParen = "(";
        private const string CloseParen = ")";

        public Queue<string> ConvertToPostfix(List<string> tokens)
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
    }
}
