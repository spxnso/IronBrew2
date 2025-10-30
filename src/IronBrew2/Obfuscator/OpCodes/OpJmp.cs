using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpJmp : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Jmp;

        public override string GetObfuscated(ObfuscationContext context) =>
            "InstrPoint=Inst[OP_B];";

        public override void Mutate(Instruction instruction)
        {
            instruction.B += instruction.PC + 1;
        }
    }
}