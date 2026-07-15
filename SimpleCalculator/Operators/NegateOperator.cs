namespace SimpleCalculator.Operators
{
    public class NegateOperator : Operator
    {
        public override string Symbol => "neg";
        public override int Precedence => 4;
        public override bool IsRightAssociative => true;
        public override int OperandCount => 1;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"NegateOperator requires {OperandCount} operand.");
            return -operands[0];
        }
    }
}
