using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpGetUpval : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.GetUpval;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=Upvalues[Inst[OP_B]];";
    }
}