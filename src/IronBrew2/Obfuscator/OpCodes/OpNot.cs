

using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpNot : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Not;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=(not Stk[Inst[OP_B]]);";
    }
}