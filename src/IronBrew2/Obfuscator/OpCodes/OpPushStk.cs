

using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpPushStk : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.PushStack;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]] = Stk";
    }
}