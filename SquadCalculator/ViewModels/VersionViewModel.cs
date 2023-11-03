using System;
using SquadCalculator.Models;

namespace SquadCalculator.ViewModels;

public class VersionViewModel
{
    public VersionChecker VersionChecker { get; set;} = new VersionChecker();
    
    private Command? _loadedCommand;
    
    public Command LoadedCommand
    {
        get
        {
            return _loadedCommand ??= new Command(async () => { await VersionChecker.CheckForUpdateAsync(); });
        }
    }
}