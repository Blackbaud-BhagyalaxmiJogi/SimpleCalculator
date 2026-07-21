namespace SimpleCalculator.Operators
{
    public class AddOperator : Operator
    {
        public override string Symbol => "+";
        public override int Precedence => 1;
        public override bool IsRightAssociative => false;
        public override int OperandCount => 2;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"AddOperator requires {OperandCount} operands.");
            return operands[0] + operands[1];
        }
    }
}
