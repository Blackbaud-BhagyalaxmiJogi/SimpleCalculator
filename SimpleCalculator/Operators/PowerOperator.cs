namespace SimpleCalculator.Operators
{
    public class PowerOperator : Operator
    {
        public override string Symbol => "^";
        public override int Precedence => 3;
        public override bool IsRightAssociative => true;
        public override int OperandCount => 2;
        public override double Apply(double[] operands)
        {
            if (operands.Length != OperandCount)
                throw new ArgumentException($"PowerOperator requires {OperandCount} operands.");
            return Math.Pow(operands[0], operands[1]);
        }
    }

}
