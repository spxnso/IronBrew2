

using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpSetGlobal : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.SetGlobal;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Env[Inst[OP_B]] = Stk[Inst[OP_A]];";

        public override void Mutate(Instruction instruction)
        {
            instruction.B++;
            instruction.ConstantMask |= InstructionConstantMask.RB;
        }
    }
}