using FlowOnRails;

namespace Calculator.Core.Nodes;

public class Number(double number) : Node
{
    public double Value { get; } = number;

    public override RailFor<double> Resolve(Context context)
        => Value;
}