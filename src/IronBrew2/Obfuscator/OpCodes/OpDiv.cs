using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpDiv : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Div && instruction.B <= 255 && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=Stk[Inst[OP_B]] / Stk[Inst[OP_C]];";
    }

    public class OpDivB : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Div && instruction.B > 255 && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]] = Inst[OP_B] / Stk[Inst[OP_C]];";

        public override void Mutate(Instruction instruction)
        {
            instruction.B -= 255;
            instruction.ConstantMask |= InstructionConstantMask.RB;
        }
    }

    public class OpDivC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Div && instruction.B <= 255 && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]] = Stk[Inst[OP_B]] / Inst[OP_C];";

        public override void Mutate(Instruction instruction)
        {
            instruction.C -= 255;
            instruction.ConstantMask |= InstructionConstantMask.RC;
        }
    }

    public class OpDivBC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Div && instruction.B > 255 && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]] =  Inst[OP_B] / Inst[OP_C];";

        public override void Mutate(Instruction instruction)
        {
            instruction.B -= 255;
            instruction.C -= 255;
            instruction.ConstantMask |= InstructionConstantMask.RB | InstructionConstantMask.RC;
        }
    }
}