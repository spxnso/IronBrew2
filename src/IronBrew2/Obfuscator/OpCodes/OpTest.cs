
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
	public class OpTest : VOpCode
	{
		public override bool IsInstruction(Instruction instruction) =>
			instruction.OpCode == OpCode.Test && instruction.C == 0;

		public override string GetObfuscated(ObfuscationContext context) =>
			"if Stk[Inst[OP_A]] then InstrPoint=InstrPoint + 1; else InstrPoint = Inst[OP_B]; end;";
		
		public override void Mutate(Instruction instruction)
		{
			instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
			instruction.InstructionType = InstructionType.AsBxC;
		}
	}
	
	public class OpTestC : VOpCode
	{
		public override bool IsInstruction(Instruction instruction) =>
			instruction.OpCode == OpCode.Test && instruction.C != 0;

		public override string GetObfuscated(ObfuscationContext context) =>
			"if not Stk[Inst[OP_A]] then InstrPoint=InstrPoint+1;else InstrPoint=Inst[OP_B];end;";

		public override void Mutate(Instruction instruction)
		{
			instruction.B = instruction.PC + instruction.Chunk.Instructions[instruction.PC + 1].B + 2;
			instruction.InstructionType = InstructionType.AsBxC;
		}
	}
} 