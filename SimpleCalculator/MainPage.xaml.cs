using SimpleCalculator.Business;
using SimpleCalculator.Models;
using System.Linq;
using System.Reflection.Emit;

namespace SimpleCalculator;

public partial class MainPage : ContentPage
{
	int currentState = 1;
	string operant;
	double firstNumber;
	double secondNumber;
	int negativeNumber = 1;

	//history
	string currentOperationHistory;
    public List<string> history = new();  

	public MainPage()
	{
		InitializeComponent();
		OnClear(this, null);
	}

	void OnClear(object sender, EventArgs e)
	{
		firstNumber = 0;
		secondNumber = 0;
		currentState = 1;
		this.result.Text = "0";
		UpdateHistory(sender, e, "");
	}

	void OnSquared(object sender, EventArgs e)
	{
		if (firstNumber == 0)
		{
			return;
		}

		currentState = -1;
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

        currentState = -1;

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

        if (this.result.Text =="0" || currentState < 0)
		{
			this.result.Text = string.Empty;
			if (currentState < 0) 
			{ 
				currentState *= -1;	
			}
		}

        this.result.Text += btnPressed;				
        
		if (double.TryParse(this.result.Text, out number))
		{
            this.result.Text = number.ToString("N0");
            if (currentState == 1) 
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
		if (currentState == 1)
		{
			negativeNumber = -1;
		}
		else
		{
            negativeNumber = 1;
        }
		OnOperantSelection(sender, e);
	}

    void OnOperantSelection(object sender, EventArgs e)
	{
		
		Button button = (Button) sender;
		string btnPressed = button.Text;
		operant = btnPressed;        

        if ((negativeNumber == -1) && (currentState == -2))
        {
            this.result.Text = "-";
            negativeNumber = 1;
        }
        else
		{
            OnCalculate(sender, e);
            currentState = -2;    			
           // this.result.Text = "";            
        }
        
        //history
        currentOperationHistory = $"{firstNumber} {operant}";
        UpdateHistory(sender, e, currentOperationHistory);
        
    }

    void OnCalculate(object sender, EventArgs e)
	{
		if (currentState == 2)
		{
			var result = Calculation.Calculate(operant, firstNumber, secondNumber);
			this.result.Text = result.ToString();
			
			currentState = -1;

			//history
			currentOperationHistory = $"{firstNumber} {operant} {secondNumber}";
            UpdateHistory(sender, e, currentOperationHistory);	

            firstNumber = result;
        }
	}

	void UpdateHistory(object sender, EventArgs e, string currentHistory)
	{   
        if (currentState ==-1)
		{
            history.Reverse();
            history.Add(currentHistory);			
            
            this.historyListView.ItemsSource = "";
            this.historyListView.ItemsSource = history;
            history.Reverse();

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

