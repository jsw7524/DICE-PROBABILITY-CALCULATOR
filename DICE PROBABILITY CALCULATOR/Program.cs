using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace DICE_PROBABILITY_CALCULATOR
{

    public enum TokenType
    {
        OP,
        Val
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
        public string name;
        public Dictionary<decimal, decimal> dist = new Dictionary<decimal, decimal>();

        public Operand()
        {

        }
        public Operand(decimal d)
        {
            dist[d] = 1.00m;
            tkType = TokenType.Val;
        }
        public Operand(string s)
        {
            name = s;
            decimal bound = decimal.Parse(s.Replace("d", ""));
            for (var i = 1; i <= bound; i++)
            {
                dist[i] = 1.0m / bound;
            }
            tkType = TokenType.Val;
        }

        public override string ToString()
        {          
            StringBuilder sb = new StringBuilder();

            foreach (var i in dist.OrderBy(a=>a.Key))
            {
                sb.Append($"{i.Key} {(i.Value*100).ToString("##.00")}\n");
            }
            return sb.ToString().TrimEnd('\r', '\n');
        }


        public static Operand operator +(Operand a, Operand b)
        {
            Operand tmp = new Operand();
            foreach (var i in a.dist)
            {
                foreach (var j in b.dist)
                {
                    if (!tmp.dist.ContainsKey(i.Key + j.Key))
                    {
                        tmp.dist[i.Key + j.Key] = 0m;
                    }
                    tmp.dist[i.Key + j.Key] += i.Value * j.Value;
                }
            }
            return tmp;
        }

        public static Operand operator -(Operand a, Operand b)
        {
            Operand tmp = new Operand();
            foreach (var i in a.dist)
            {
                foreach (var j in b.dist)
                {
                    if (!tmp.dist.ContainsKey(i.Key - j.Key))
                    {
                        tmp.dist[i.Key - j.Key] = 0m;
                    }
                    tmp.dist[i.Key - j.Key] += i.Value * j.Value;
                }
            }
            return tmp;
        }

        public static Operand operator *(Operand a, Operand b)
        {
            Operand tmp = new Operand();
            foreach (var i in a.dist)
            {
                foreach (var j in b.dist)
                {
                    if (!tmp.dist.ContainsKey(i.Key * j.Key))
                    {
                        tmp.dist[i.Key * j.Key] = 0m;
                    }
                    tmp.dist[i.Key * j.Key] += i.Value * j.Value;
                }
            }
            return tmp;
        }
        public static Operand operator <(Operand a, Operand b)
        {
            decimal ea = 0m;
            decimal eb = 0m;
            foreach (var i in a.dist)
            {
                ea += i.Key * i.Value;
            }
            foreach (var j in b.dist)
            {
                eb += j.Key * j.Value;
            }
            return new Operand(ea < eb ? 1m : 0m);
        }

        public static Operand operator >(Operand a, Operand b)
        {
            decimal ea = 0m;
            decimal eb = 0m;
            foreach (var i in a.dist)
            {
                ea += i.Key * i.Value;
            }
            foreach (var j in b.dist)
            {
                eb += j.Key * j.Value;
            }
            return new Operand(ea > eb ? 1m : 0m);
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
                        token = new Operand(m.Value);
                    }
                    else
                    {
                        token = new Operand(Decimal.Parse(m.Value));
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

        public Operand EvaluateExpression(List<Token> tokens)
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
            return valStack.FirstOrDefault();
        }
    }

    class Solution
    {
        static void Main(string[] args)
        {
            // Write an answer using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");
            string expr = Console.ReadLine();
            Console.Error.WriteLine(expr);
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(expr);
            var result = myCalculator.EvaluateExpression(Tokens);
            Console.WriteLine(result);
        }
    }

}
