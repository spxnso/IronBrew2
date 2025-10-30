using IronBrew2.Bytecode.IR;

namespace IronBrew2.Obfuscator;

public abstract class VOpCode
{
    public int VIndex;

    public abstract bool IsInstruction(Instruction instruction);
    public abstract string GetObfuscated(ObfuscationContext context);
    public virtual void Mutate(Instruction instruction) { }
}