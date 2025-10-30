
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpNewTableB0 : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) => instruction.OpCode == OpCode.NewTable;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Stk[Inst[OP_A]]={};";
    }
}