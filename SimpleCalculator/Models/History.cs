using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Models;

public class History : BindableBase, INotifyPropertyChanged 
{
    //eventbased update, does not work
    public event PropertyChangedEventHandler PropertyChanged;

    
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Display { get; set; }
    public string Operant { get; set; }
    public double FirstNumber { get; set; }
    public double SecondNumber { get; set; }    
    private bool _isUpdate; //listview update broken - workaround

    public bool IsUpdate
    {
		get { return _isUpdate; }
		set 
        { 
            SetProperty(ref _isUpdate, value);
            OnPropertyChanged(); //eventbased update, does not work
        }
	}

    //eventbased update, does not work
    public void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}
