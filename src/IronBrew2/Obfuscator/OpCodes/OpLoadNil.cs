using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;


namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpLoadNil : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.LoadNil;

        public override string GetObfuscated(ObfuscationContext context) =>
            "for Idx=Inst[OP_A],Inst[OP_B] do Stk[Idx]=nil;end;";
    }
}