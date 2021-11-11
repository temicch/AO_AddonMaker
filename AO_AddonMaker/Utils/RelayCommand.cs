using System;
using System.Windows.Input;

namespace Application.PL.Utils;

public class RelayCommand : ICommand
{
    private readonly Predicate<object> canExecute;
    private readonly Action<object> execute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        execute(parameter);
    }
}
