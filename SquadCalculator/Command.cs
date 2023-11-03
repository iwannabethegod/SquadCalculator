using System;
using System.Threading.Tasks;

namespace SquadCalculator;

public class Command : AsyncCommandBase 
{
    private readonly Func<Task> _command;
        
        

    public Command(Func<Task> command)
    {
        _command = command;
    }
        
    public override bool CanExecute(object parameter)
    {
        return true;
    }

    public override Task ExecuteAsync(object parameter)
    {
        return _command();
    }


}