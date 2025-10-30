using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;


namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpGetGlobal : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.GetGlobal;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=Env[Inst[OP_B]];";

        public override void Mutate(Instruction instruction)
        {
            instruction.B++;
            instruction.ConstantMask |= InstructionConstantMask.RB;
        }
    }
}