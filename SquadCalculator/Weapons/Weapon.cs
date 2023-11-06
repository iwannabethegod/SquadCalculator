namespace SquadCalculator.Weapons;

public enum AngleUnit
{
    Degrees,
    Milliradians
}

public enum AngleType
{
    Low,
    High,
    Both
}

//todo angletype both low high

public abstract class Weapon
{
    public abstract string Name { get; set; }
    public abstract double Velocity { get; set; }
    public abstract double Gravity { get; set; }
    public abstract AngleUnit AngleUnit { get; set; }
    public abstract AngleType AngleType { get; set; }
    public abstract int MaxRange { get; set; }

}