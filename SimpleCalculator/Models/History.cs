using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Models;

public class History : BindableBase
{
    #region Properties
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Display { get; set; }
    public string Operant { get; set; }
    public double FirstNumber { get; set; }
    public double SecondNumber { get; set; }

    //listview update broken - workaround
    private bool _isUpdate;
    #endregion
    
    public bool IsUpdate
    {
		get { return _isUpdate; }
		set { SetProperty(ref _isUpdate, value); }
	}

}
