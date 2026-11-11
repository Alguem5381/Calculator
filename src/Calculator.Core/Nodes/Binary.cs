using FlowOnRails;

namespace Calculator.Core.Nodes;

public enum BinaryOperation
{
    Unknown,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Power,
    Remainder
}

public abstract record BinaryError(string Message = "Error") : Error;
public record InvalidOperator(BinaryOperation Operation) : BinaryError;
public record DivisionByZero : BinaryError;

public class Binary(Node left, Node right, BinaryOperation operation) : Node
{
    public Node Left { get; } = left;
    public Node Right { get; } = right;
    public BinaryOperation Operation { get; } = operation;

    public override RailFor<double> Resolve(Context context)
    {
        var result = 
            from left in Left.Resolve(context)
            from right in Right.Resolve(context)
            select (left, right);

        if (!result.TryUnwrap(out var value, out var error))
            return error;

        switch (Operation)
        {
            case BinaryOperation.Addition:
                return value.left + value.right;

            case BinaryOperation.Subtraction:
                return value.left - value.right;
            
            case BinaryOperation.Multiplication:
                return value.left * value.right;

            case BinaryOperation.Division:
                    return value.right is 0 
                        ? new DivisionByZero() 
                        : value.left / value.right;

            case BinaryOperation.Power:
                {
                    double accumulator = 1;

                    if (value.right < 0)
                    {
                        value.left = 1 / value.left;
                        value.right = -value.right; 
                    }

                    for (var index = 0; index < value.right; index++)
                        accumulator *= value.left;

                    return accumulator;
                }
            
            case BinaryOperation.Remainder:
                return value.right is 0
                    ? new DivisionByZero()
                    : value.left % value.right;

            default:
                return new InvalidOperator(Operation);
        };
    }
}