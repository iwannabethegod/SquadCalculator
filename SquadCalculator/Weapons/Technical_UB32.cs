namespace SquadCalculator.Weapons;

public class Technical_UB32 : Weapon
{
    public override string Name { get; set; } = "Technical UB-32";
    public override double Velocity { get; set; } = 250;
    public override double Gravity { get; set; } = 9.8 ;
    public override AngleUnit AngleUnit { get; set; } = AngleUnit.Degrees;
    public override AngleType AngleType { get; set; } = AngleType.Both;
    public override int MaxRange { get; set; } = 0;
}