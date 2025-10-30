using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpForPrep : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.ForPrep;

        public override string GetObfuscated(ObfuscationContext context) =>
            @"
local A = Inst[OP_A];
local Index = Stk[A]
local Step = Stk[A + 2];
if (Step > 0) then 
	if (Index > Stk[A+1]) then
		InstrPoint = Inst[OP_B];
	else
		Stk[A+3] = Index;
	end
elseif (Index < Stk[A+1]) then
	InstrPoint = Inst[OP_B];
else
	Stk[A+3] = Index;
end
";

        public override void Mutate(Instruction instruction)
        {
            instruction.B += instruction.PC + 2;
        }
    }
}