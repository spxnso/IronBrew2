
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpTailCall : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.TailCall && instruction.B > 1;

        public override string GetObfuscated(ObfuscationContext context) =>
            @"
local A = Inst[OP_A];
do return Stk[A](Unpack(Stk, A + 1, Inst[OP_B])) end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.B += instruction.A - 1;
        }
    }

    public class OpTailCallB0 : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.TailCall && instruction.B == 0;

        public override string GetObfuscated(ObfuscationContext context) =>
            @"
local A = Inst[OP_A];
do return Stk[A](Unpack(Stk, A + 1, Top)) end;
";
    }

    public class OpTailCallB1 : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.TailCall && instruction.B == 1;

        public override string GetObfuscated(ObfuscationContext context) =>
            "do return Stk[Inst[OP_A]](); end;";
    }
}