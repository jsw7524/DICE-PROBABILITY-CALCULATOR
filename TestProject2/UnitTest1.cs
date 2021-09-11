using DICE_PROBABILITY_CALCULATOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string s = "1*20+300";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(5, Tokens.Count);
        }

        [TestMethod]
        public void TestMethod2()
        {
            string s = "(1+(4+5+2)-3)+(6+8)";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(23m, myCalculator.EvaluateExpression(Tokens));
        }


        [TestMethod]
        public void TestMethod3()
        {
            string s = "2*(3+4)-9";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            var result = myCalculator.EvaluateExpression(Tokens);
            Assert.AreEqual(5m, result);
        }


        [TestMethod]
        public void TestMethod4()
        {
            string s = "5>2";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(3, Tokens.Count);
        }

        [TestMethod]
        public void TestMethod5()
        {
            string s = "5>2";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            var result = myCalculator.EvaluateExpression(Tokens);
            Assert.AreEqual(1m, result);
        }

        [TestMethod]
        public void TestMethod6()
        {
            string s = "d2+d6+63";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(5, Tokens.Count);
        }


    }
}
