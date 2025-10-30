using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpSetUpval : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.SetUpval;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Upvalues[Inst[OP_B]]=Stk[Inst[OP_A]];";
    }
}