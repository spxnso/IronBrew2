using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpLt : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Lt && instruction.A == 0 && instruction.B <= 255 && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "if(Stk[Inst[OP_A]] < Stk[Inst[OP_C]])then InstrPoint=InstrPoint+1;else InstrPoint=Inst[OP_B];end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.A = instruction.B;

            instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
            instruction.InstructionType = InstructionType.AsBxC;
        }
    }

    public class OpLtB : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Lt && instruction.A == 0 && instruction.B > 255 && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "if(Inst[OP_A] < Stk[Inst[OP_C]])then InstrPoint=InstrPoint+1;else InstrPoint=Inst[OP_B];end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.A = instruction.B - 255;

            instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
            instruction.InstructionType = InstructionType.AsBxC;
            instruction.ConstantMask |= InstructionConstantMask.RA;
        }
    }

    public class OpLtC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Lt && instruction.A == 0 && instruction.B <= 255 && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "if(Stk[Inst[OP_A]] < Inst[OP_C])then InstrPoint=InstrPoint+1;else InstrPoint=Inst[OP_B];end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.A = instruction.B;
            instruction.C -= 255;

            instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
            instruction.InstructionType = InstructionType.AsBxC;
            instruction.ConstantMask |= InstructionConstantMask.RC;
        }
    }

    public class OpLtBC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Lt && instruction.A == 0 && instruction.B > 255 && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "if(Inst[OP_A] < Inst[OP_C])then InstrPoint=InstrPoint+1;else InstrPoint=Inst[OP_B];end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.A = instruction.B - 255;
            instruction.C -= 255;

            instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
            instruction.InstructionType = InstructionType.AsBxC;
            instruction.ConstantMask |= InstructionConstantMask.RA | InstructionConstantMask.RC;
        }
    }
}