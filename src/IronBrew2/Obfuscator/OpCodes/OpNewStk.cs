
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpNewStk : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.NewStack;

        public override string GetObfuscated(ObfuscationContext context) => "Stk = {};for Idx = 0, PCount do if Idx < Params then Stk[Idx] = Args[Idx + 1]; else break end; end;";
    }
}