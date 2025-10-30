
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpSelf : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Self && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "local A=Inst[OP_A];local B=Stk[Inst[OP_B]];Stk[A+1]=B;Stk[A]=B[Stk[Inst[OP_C]]];";
    }

    public class OpSelfC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Self && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "local A=Inst[OP_A];local B=Stk[Inst[OP_B]];Stk[A+1]=B;Stk[A]=B[Inst[OP_C]];";

        public override void Mutate(Instruction instruction)
        {
            instruction.C -= 255;
            instruction.ConstantMask |= InstructionConstantMask.RC;
        }
    }
}