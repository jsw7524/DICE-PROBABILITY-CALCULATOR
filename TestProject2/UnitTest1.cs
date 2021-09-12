using DICE_PROBABILITY_CALCULATOR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            var result = myCalculator.EvaluateExpression(Tokens);
            Assert.AreEqual(1m, result.dist[23m]);
        }


        [TestMethod]
        public void TestMethod3()
        {
            string s = "2*(3+4)-9";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            var result = myCalculator.EvaluateExpression(Tokens);
            Assert.AreEqual(1m, result.dist[5m]);
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
            Assert.AreEqual(1m, result.dist[1m]);
        }

        [TestMethod]
        public void TestMethod6()
        {
            string s = "d2+d6+63";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            Assert.AreEqual(5, Tokens.Count);
        }


        [TestMethod]
        public void TestMethod7()
        {
            Operand d6 = new Operand("d6");
            Operand v1 = new Operand(1m);

            var result = d6 + v1;
            Assert.AreEqual(1m/6m, result.dist[7m]);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestMethod8()
        {
            Operand d6a = new Operand("d6");
            Operand d6b = new Operand("d6");
            var result = d6a + d6b;
            var i=result.dist[0m];
        }

        [TestMethod]
        public void TestMethod9()
        {
            Operand d6a = new Operand("d6");
            Operand d6b = new Operand("d6");
            var result = d6a * d6b;
            Assert.AreEqual(18, result.dist.Count);
        }

        [TestMethod]
        public void TestMethod10()
        {
            string s = "3*(d2+1)";
            MyCalculator myCalculator = new MyCalculator();
            var Tokens = myCalculator.GetTokens(s);
            var result = myCalculator.EvaluateExpression(Tokens);
            Assert.AreEqual(1m, result.dist[6m] + result.dist[9m]);
        }
    }
}
