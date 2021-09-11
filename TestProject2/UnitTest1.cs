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
            Assert.AreEqual(23, int.Parse(myCalculator.EvaluateExpression(Tokens).ToString()));
        }


        [TestMethod]
        public void TestMethod3()
        {
            string s = "2*(3+4)-9";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(5, int.Parse(myCalculator.EvaluateExpression(Tokens).ToString()));
        }




    }
}
