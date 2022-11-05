using SimpleCalculator.Business;
using SimpleCalculator.Enums;
using SimpleCalculator.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using Prism.Mvvm;

namespace SimpleCalculator;

public partial class MainPage : ContentPage 
{
	CalculatorState currentState = CalculatorState.GetFirstNumber;
    string operant = String.Empty;
	double firstNumber;
	double secondNumber;
	int negativeNumber = 1;

	//history
	string currentOperationHistory;
    //ObservableCollection buggy
    private ObservableCollection<History> history = new();
     

    public MainPage()
	{
		InitializeComponent();
        //not needed as observable collection is not working
        // this.historyListView.BindingContext = new History();       

        OnClear(this, null);        
    }

	void OnClear(object sender, EventArgs e)
	{
		firstNumber = 0;
		secondNumber = 0;        
        currentState = CalculatorState.GetFirstNumber;
        this.result.Text = "0";
		UpdateHistory(sender, e, "");
	}

	void OnSquared(object sender, EventArgs e)
	{
		if (firstNumber == 0)
		{
			return;
		}
        
        currentState = CalculatorState.CalculationCompleted;
        //history
        currentOperationHistory = $"{firstNumber} * {firstNumber}";
        UpdateHistory(sender, e, currentOperationHistory);

        firstNumber = firstNumber * firstNumber;
		this.result.Text = firstNumber.ToString();
	}

	void OnSquareRoot(object sender, EventArgs e)
	{
        if (firstNumber == 0)
        {
            return;
        }
       
        currentState = CalculatorState.CalculationCompleted;

        //history
        currentOperationHistory = $"sqrt {firstNumber}";
        UpdateHistory(sender, e, currentOperationHistory);

        firstNumber = Math.Sqrt(firstNumber);
        this.result.Text = firstNumber.ToString();        
    }

	void OnNumberSelection(object sender, EventArgs e)
	{
		Button button = (Button) sender;
		string btnPressed = button.Text;
        double number;
        
        if (this.result.Text == "0" || 
			currentState == CalculatorState.CalculationCompleted || 
			currentState == CalculatorState.GetSecondNumber )
        {
			this.result.Text = string.Empty;
            
            if (currentState == CalculatorState.CalculationCompleted) 
                currentState = CalculatorState.GetFirstNumber;
            
            if (currentState == CalculatorState.GetSecondNumber)              	
                currentState = CalculatorState.ReadyToCalculate;            
        }

        if (negativeNumber == -1)
        {
            this.result.Text += $"-{btnPressed}";
            negativeNumber = 1;
        } 
        else
        {
            this.result.Text += btnPressed;
        } 

		if (double.TryParse(this.result.Text, out number))
		{
            this.result.Text = number.ToString("N0");
            
            if (currentState == CalculatorState.GetFirstNumber)
            { 
				firstNumber = number;                
            }
            else 
			{ 
				secondNumber = number;                
            }
		}		
	}
    
	void OnNegativeNumber(object sender, EventArgs e)
	{
        if (this.result.Text == "-")
        {
            negativeNumber = 1;
            this.result.Text = "";
        }        
        else if ((currentState == CalculatorState.GetFirstNumber ||
                currentState == CalculatorState.GetSecondNumber) &&
                (this.result.Text =="" || operant != String.Empty))
        {
            this.result.Text = "-";
            negativeNumber = -1;
		}
		else if(this.result.Text != "-" && this.result.Text != "")
		{
            OnOperantSelection(sender, e);
            negativeNumber = 1;
        }		
	}

    void OnOperantSelection(object sender, EventArgs e)
	{		
		Button button = (Button) sender;
		string btnPressed = button.Text;    
        this.result.Text = String.Empty;        

        OnCalculate(sender, e, true);

        operant = btnPressed;
        currentState = CalculatorState.GetSecondNumber;
           
        //history
        currentOperationHistory = $"{firstNumber} {operant}";
        UpdateHistory(sender, e, currentOperationHistory);        
    }

    //work around - optional parameters does not work with maui
    void OnCalculateBtn(object sender, EventArgs e)
    {
        OnCalculate(sender, e);
    }

    void OnCalculate(object sender, EventArgs e, bool clearEntryField = false)
    {        
        if (currentState == CalculatorState.ReadyToCalculate)
        {
			var result = Calculation.Calculate(operant, firstNumber, secondNumber);
			this.result.Text = result.ToString();

            currentState = CalculatorState.CalculationCompleted;

            //history
            currentOperationHistory = $"{firstNumber} {operant} {secondNumber}";
            UpdateHistory(sender, e, currentOperationHistory);	

            firstNumber = result;
            operant = String.Empty;
            if (clearEntryField)
                this.result.Text = String.Empty;
        }
	}

	void UpdateHistory(object sender, EventArgs e, string currentHistory)
	{
        if (currentState == CalculatorState.CalculationCompleted)
        {			
            history.Add(new History()
            {
                Display = currentHistory,
                FirstNumber = firstNumber,
                SecondNumber = secondNumber,
                Operant = operant                
            } );

            //listview update broken - workaround
            history.FirstOrDefault().IsUpdate = true;               
            this.historyListView.ItemsSource = "";
            this.historyListView.ItemsSource = history;
            
            //work around for listview Microsoft Bug
            if (history.Count() > 3)
			{
                this.historyListView.ScrollTo(history.Last(), ScrollToPosition.End, false);
            }         
			
			currentOperationHistory = string.Empty;
			currentHistory = string.Empty;
        }
        else if(currentHistory=="")
		{
			history.Clear();
            
            currentOperationHistory = string.Empty;
            this.historyListView.ItemsSource = "";
        }
		
        this.currentOperationHistoryLabel.Text = currentHistory;
    }

}

