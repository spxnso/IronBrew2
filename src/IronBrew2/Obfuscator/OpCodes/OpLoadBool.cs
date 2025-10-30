using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;


namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpLoadBool : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.LoadBool && instruction.C == 0;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=(Inst[OP_B]~=0);";
    }

    public class OpLoadBoolC : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.LoadBool && instruction.C != 0;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]=(Inst[OP_B]~=0);InstrPoint=InstrPoint+1;";

        public override void Mutate(Instruction ins) =>
            ins.C = 0;
    }
}