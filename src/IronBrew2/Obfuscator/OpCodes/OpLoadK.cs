using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;


namespace IronBrew2.Obfuscator.OpCodes
{
	public class OpLoadK : VOpCode
	{
		public override bool IsInstruction(Instruction instruction) =>
			instruction.OpCode == OpCode.LoadConst; // && instruction.Chunk.Constants[instruction.B].Type != ConstantType.String;

		public override string GetObfuscated(ObfuscationContext context) =>
			"Stk[Inst[OP_A]] = Inst[OP_B];";

		public override void Mutate(Instruction instruction)
		{
			instruction.B++;
			instruction.ConstantMask |= InstructionConstantMask.RB;
		}
	}
}