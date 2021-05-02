using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyUnitTest.APP
{
    public class Calculator
    {
        private ICalculatorService _calculatorService;
        public Calculator(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public int add(int a ,int b)    
        { 
            return _calculatorService.add(a,b);

            //return c;
        }

        public int multip(int a ,int b)
        {
            var c =_calculatorService.multip(a, b);
            return c;
        }
    }
}
