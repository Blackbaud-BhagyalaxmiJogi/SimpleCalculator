

namespace SimpleCalculator.Calculation
{
    public class ExpressionCalculator : IExpressionCalculator
    {
        private readonly Tokenizer tokenizer;
        private readonly ShuntingYardConverter converter;
        private readonly PostfixEvaluator evaluator;

        public ExpressionCalculator() : this(new Tokenizer(), new ShuntingYardConverter(), new PostfixEvaluator())
        {
        }

        public ExpressionCalculator(Tokenizer tokenizer, ShuntingYardConverter converter, PostfixEvaluator evaluator)
        {
            this.tokenizer = tokenizer;
            this.converter = converter;
            this.evaluator = evaluator;
        }


        public double Evaluate(string expression)
        {
            string cleanedExpression = expression.Replace(" ", "");
            List<string> tokens = tokenizer.Tokenize(cleanedExpression);
            Queue<string> postfixTokens = converter.ConvertToPostfix(tokens);
            return evaluator.EvaluatePostfix(postfixTokens);
        }
    }
}
