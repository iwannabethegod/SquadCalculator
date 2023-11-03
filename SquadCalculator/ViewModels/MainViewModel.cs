using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SquadCalculator.Weapons;

namespace SquadCalculator.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public static List<Weapon> Weapons { get; set; } = new List<Weapon>()
    {
        new Mortar(),
        new BM21Grad(),
    };
    public static Maps Maps { get; set; } = new Maps();
    
    public static Map? selectedItem;
    public static Weapon? selectedWeapon = Weapons[0];

    public Map? SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnPropertyChanged();
        }
    }
    
    public Weapon? SelectedWeapon
    {
        get => selectedWeapon;
        set
        {
            selectedWeapon = value;
            OnPropertyChanged();
        }
    }
    
    /*
    private Command? _testCommand;
    
    public Command LoadCommand
    {
        get { return _testCommand ??= new Command(async () => { await Maps.LoadMapsAsync(); }); }
    }
    */

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}