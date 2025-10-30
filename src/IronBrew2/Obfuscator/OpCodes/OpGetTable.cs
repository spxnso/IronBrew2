
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpGetTable : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.GetTable && instruction.C <= 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=Stk[Inst[OP_B]][Stk[Inst[OP_C]]];";
    }

    public class OpGetTableConst : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.GetTable && instruction.C > 255;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=Stk[Inst[OP_B]][Inst[OP_C]];";

        public override void Mutate(Instruction instruction)
        {
            instruction.C -= 255;
            instruction.ConstantMask |= InstructionConstantMask.RC;
        }
    }
}