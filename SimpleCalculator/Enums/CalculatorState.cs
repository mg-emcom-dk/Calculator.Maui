using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Enums;

enum CalculatorState
{
    GetFirstNumber, //previously 1
    ReadyToCalculate, //previously 2
    CalculationCompleted, //previously -1
    GetSecondNumber //previously -2
}
