namespace SimpleCalculator.Operators
{

    /// Retrieves the operator associated with a symbol.
    /// Since operators are stateless, shared instances are reused
    /// instead of creating new objects for each lookup.

    public static class OperatorFactory
    {
        private static readonly Dictionary<string, Operator> operatorsBySymbol = new List<Operator>
        {
            new NegateOperator(),
            new PowerOperator(),
            new MultiplyOperator(),
            new DivideOperator(),
            new ModuloOperator(),
            new AddOperator(),
            new SubtractOperator()
        }.ToDictionary(op => op.Symbol);

        public static bool IsOperator(string token)=> operatorsBySymbol.ContainsKey(token);

        public static Operator GetOperator(string token)
        {
            if (operatorsBySymbol.TryGetValue(token, out var op))
            {
                return op;
            }
            throw new FormatException($"No operator found for symbol '{token}'");
        }


        /// Contains all binary operator symbols. Used to identify whether
        /// a '-' should be treated as unary negation during tokenization.
        /// Automatically stays up to date as new binary operators are added.

        public static string GetBinaryOperatorSymbols()
        {
            return string.Concat(operatorsBySymbol.Values.Where(op => op.OperandCount == 2).Select(op => op.Symbol));
        }





    }
}
