using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
namespace DICE_PROBABILITY_CALCULATOR
{

    public enum TokenType
    {
        OP,
        Val,
        Variable
    }

    public class Token
    {
        public TokenType tkType;
        public Token()
        {

        }
    }

    public class Operand : Token
    {
        public decimal val;
        public string name;

        public Operand(decimal d)
        {
            val = d;
            tkType = TokenType.Val;
        }
        public Operand(string s)
        {
            name = s;
            val = decimal.Parse(s.Replace("d", ""));
            tkType = TokenType.Variable;
        }

        public static Operand operator +(Operand a, Operand b)
        {
            return new Operand(a.val + b.val);
        }

        public static Operand operator -(Operand a, Operand b)
        {
            return new Operand(a.val - b.val);
        }

        public static Operand operator *(Operand a, Operand b)
        {
            return new Operand(a.val * b.val);
        }
        public static Operand operator <(Operand a, Operand b)
        {
            if (a.val < b.val)
                return new Operand(1m);
            return new Operand(0m);
        }

        public static Operand operator >(Operand a, Operand b)
        {
            if (a.val > b.val)
                return new Operand(1m);
            return new Operand(0m);
        }



    }

    public class Value : Operand
    {

        public Value(decimal d) : base(d)
        {

        }
    }

    public class Variable : Operand
    {
        public Variable(string s) : base(s)
        {

        }
    }


    public class Operator : Token
    {
        public string op;
        public Operator(string s)
        {
            op = s;
            tkType = TokenType.OP;
        }
    }

    public class MyCalculator
    {
        public Dictionary<string, int> precedenceTable = new Dictionary<string, int>
        {
            {">",0 },{"+",1},{"-",1},{"*",2},{"/",2},{"(",1000},{")",-1000}
        };

        public List<Token> GetTokens(string input)
        {
            Regex regex = new Regex(@"(?<val>\d+|d\d+)|(?<op>\>|\+|\-|\*|\\|\(|\))");
            var mc = regex.Matches(input.Replace(" ", ""));
            List<Token> result = new List<Token>();
            foreach (Match m in mc)
            {
                Token token = null;
                if (m.Groups["op"].Success)
                {
                    token = new Operator(m.Value);
                }
                else
                {
                    if (m.Value.StartsWith("d"))
                    {
                        token = new Variable(m.Value);
                    }
                    else
                    {
                        token = new Value(Decimal.Parse(m.Value));
                    }

                }
                result.Add(token);
            }
            return result;
        }

        private void Compute(Operator t, Stack<Operator> opStack, Stack<Operand> valStack)
        {
            if (opStack.Count == 0)
                return;
            if (valStack.Count == 0)
                return;
            while ((opStack.Peek().op != "(") && precedenceTable[t.op] <= precedenceTable[opStack.Peek().op])
            {
                Operator opToken = opStack.Pop();
                Operand b = valStack.Pop();
                Operand a = valStack.Pop();
                switch (opToken.op)
                {
                    case "+":
                        valStack.Push(a + b);
                        break;
                    case "-":
                        valStack.Push(a - b);
                        break;
                    case "*":
                        valStack.Push(a * b);
                        break;
                    case ">":
                        valStack.Push(a > b);
                        break;
                }
                if (opStack.Count == 0)
                    return;
            }
        }

        public Decimal EvaluateExpression(List<Token> tokens)
        {
            Stack<Operator> opStack = new Stack<Operator>();
            Stack<Operand> valStack = new Stack<Operand>();

            foreach (Token t in tokens)
            {
                if (t is Operator)
                {
                    Operator oprt = t as Operator;
                    Compute(oprt, opStack, valStack);
                    if (opStack.Count > 0)
                    {
                        if (opStack.Peek().op == "(" && oprt.op == ")")
                        {
                            opStack.Pop();
                            continue;
                        }
                    }
                    opStack.Push(oprt);
                }
                else if (t is Operand)
                {
                    Operand val = t as Operand;
                    valStack.Push(val);
                }
            }
            while (opStack.Count > 0)
            {
                Operator t = opStack.Peek();
                Compute(t, opStack, valStack);
            }
            return valStack.FirstOrDefault().val;
        }
    }

    //public class Solution
    //{
    //    public int Calculate(string s)
    //    {
    //        MyCalculator myCalculator = new MyCalculator();
    //        var Tokens = myCalculator.GetTokens(s);
    //        return int.Parse(myCalculator.EvaluateExpression(Tokens).ToString());
    //    }
    //}



    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
