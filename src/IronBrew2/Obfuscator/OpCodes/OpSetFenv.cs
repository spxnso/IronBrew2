using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpSetFenv : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.SetFenv;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Env = Stk[Inst[OP_A]]";
    }
}