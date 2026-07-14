namespace SimpleCalculator.Operators
{
    /// Encapsulates all operator metadata and behavior in one place,
    /// including symbol, precedence, associativity, operand count,
    /// and evaluation logic.
    public abstract class Operator
    {
        public abstract string Symbol { get; }
        public abstract int Precedence { get; }
        public abstract bool IsRightAssociative { get; }
        public abstract int OperandCount { get; }

        /// Applies this operator to its operands. For a binary operator,
        /// operands[0] is the left-hand side and operands[1] is the right-hand side.
        /// For a unary operator, operands[0] is its only operand.
        public abstract double Apply(double[] operands);

    }
}
