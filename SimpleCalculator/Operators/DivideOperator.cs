namespace SimpleCalculator.Operators
{
    internal class DivideOperator : Operator
    {
        public override string Symbol => "/";
        public override int Precedence => 2;
        public override bool IsRightAssociative => false;
        public override int OperandCount => 2;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"DivideOperator requires {OperandCount} operands.");
            if (operands[1] == 0)
                throw new DivideByZeroException("Cannot divide by zero.");
            return operands[0] / operands[1];
        }
    }
}
