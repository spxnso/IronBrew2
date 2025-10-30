
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpVarArg : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.VarArg && instruction.B != 0;

        public override string GetObfuscated(ObfuscationContext context) =>
            "local A=Inst[OP_A];local B=Inst[OP_B];for Idx=A,B do Stk[Idx]=Vararg[Idx-A];end;";

        public override void Mutate(Instruction instruction)
        {
            instruction.B += instruction.A - 1;
        }
    }

    public class OpVarArgB0 : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.VarArg && instruction.B == 0;

        public override string GetObfuscated(ObfuscationContext context) =>
            "local A=Inst[OP_A];Top=A+Varargsz-1;for Idx=A,Top do local VA=Vararg[Idx-A];Stk[Idx]=VA;end;";
    }
}