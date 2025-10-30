
namespace IronBrew2.Bytecode.IR;

public class Constant
{
    public List<Instruction> BackReferences;

    public ConstantType Type;
    public object? Data;

    public Constant(ConstantType type, object data)
    {
        Type = type;
        Data = data;
        BackReferences = new();
    }

    public Constant(Constant other)
    {
        Type = other.Type;
        Data = other.Data;
        BackReferences = [.. other.BackReferences];
    }
}