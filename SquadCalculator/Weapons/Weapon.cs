namespace SquadCalculator.Weapons;

public enum AngleUnit
{
    Degrees,
    Milliradians
}

public abstract class Weapon
{
    public abstract string Name { get; set; }
    public abstract double Velocity { get; set; }
    public abstract double Gravity { get; set; }
    public abstract AngleUnit AngleUnit { get; set; }

}