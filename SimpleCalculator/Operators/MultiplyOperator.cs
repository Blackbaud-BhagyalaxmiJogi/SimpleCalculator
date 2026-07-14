namespace SimpleCalculator.Operators
{
    public class MultiplyOperator : Operator
    {
        public override string Symbol => "*";
        public override int Precedence => 2;
        public override bool IsRightAssociative => false;
        public override int OperandCount => 2;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"MultiplyOperator requires {OperandCount} operands.");
            return operands[0] * operands[1];
        }
    }
}
