

using SimpleCalculator.Operators;
using System.Text;

namespace SimpleCalculator.Calculation
{
    public class Tokenizer
    {
        // Special token representing unary negation (distinct from binary subtraction, which uses the same '-' character in the input).
        // negation rather than subtraction.
        private const string UnaryMinusToken = "#";
        private const string OpenParen = "(";

        // Arithmetic Operators
        private static readonly string BinaryOperatorSymbols = OperatorFactory.GetBinaryOperatorSymbols();

        public List<string> Tokenize(string expression)
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
    }
}
