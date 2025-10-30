using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;
namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpForLoop : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.ForLoop;

        public override string GetObfuscated(ObfuscationContext context) =>
            @"
local A = Inst[OP_A];
local Step = Stk[A + 2];
local Index = Stk[A] + Step;
Stk[A] = Index;
if (Step > 0) then 
	if (Index <= Stk[A+1]) then
		InstrPoint = Inst[OP_B];
		Stk[A+3] = Index;
	end
elseif (Index >= Stk[A+1]) then
	InstrPoint = Inst[OP_B];
	Stk[A+3] = Index;
end
";
        public override void Mutate(Instruction instruction)
        {
            instruction.B += instruction.PC + 1;
        }
    }
}