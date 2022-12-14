using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Business;

public static class Calculation
{
    public static double Calculate(string operation, double firstNumber, double secondNumber)
    {
        double result = 0;
        switch (operation)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "*":
                result = firstNumber * secondNumber;
                break;
            case "/":                
                result = firstNumber / secondNumber;
                break;
            default:
                break;
        }
        return result;
    }
}
