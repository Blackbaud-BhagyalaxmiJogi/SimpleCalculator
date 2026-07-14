namespace SimpleCalculator.Operators
{
    public class SubtractOperator : Operator
    {
        public override string Symbol => "-";
        public override int Precedence => 1;
        public override bool IsRightAssociative => false;
        public override int OperandCount => 2;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"SubtractOperator requires {OperandCount} operands.");
            return operands[0] - operands[1];
        }
    }
    
}
