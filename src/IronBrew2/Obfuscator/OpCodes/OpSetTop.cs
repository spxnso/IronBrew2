
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpSetTop : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.SetTop;

        public override string GetObfuscated(ObfuscationContext context) =>
            "Top=Inst[OP_A];";
    }
}