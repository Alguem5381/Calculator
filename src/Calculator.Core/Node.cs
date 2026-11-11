using FlowOnRails;

namespace Calculator.Core;

public abstract class Node
{
    public abstract RailFor<double> Resolve(Context context);
}
