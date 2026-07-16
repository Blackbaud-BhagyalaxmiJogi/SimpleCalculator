using SimpleCalculator.Operators;


namespace SimpleCalculator.Calculation
{
    public class PostfixEvaluator
    {
        public double EvaluatePostfix(Queue<string> postfixTokens)
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
