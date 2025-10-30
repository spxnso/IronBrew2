
using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;
using IronBrew2.Obfuscator;

namespace IronBrew2.Obfuscator.OpCodes;

public class OpCall : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B > 2 &&
        instruction.C > 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results = { Stk[A](Unpack(Stk, A + 1, Inst[OP_B])) };
local Edx = 0;
for Idx = A, Inst[OP_C] do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end
";

    public override void Mutate(Instruction instruction)
    {
        instruction.B += instruction.A - 1;
        instruction.C += instruction.A - 2;
    }
}

public class OpCallB2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 2 &&
        instruction.C > 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results = { Stk[A](Stk[A + 1]) };
local Edx = 0;
for Idx = A, Inst[OP_C] do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end
";
    public override void Mutate(Instruction instruction)
    {
        instruction.C += instruction.A - 2;
    }
}

public class OpCallB0 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 0 &&
        instruction.C > 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results = { Stk[A](Unpack(Stk, A + 1, Top)) };
local Edx = 0;
for Idx = A, Inst[OP_C] do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end
";
    public override void Mutate(Instruction instruction)
    {
        instruction.C += instruction.A - 2;
    }
}

public class OpCallB1 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 1 &&
        instruction.C > 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results = { Stk[A]() };
local Limit = Inst[OP_C];
local Edx = 0;
for Idx = A, Limit do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end
";
    public override void Mutate(Instruction instruction)
    {
        instruction.C += instruction.A - 2;
    }
}

public class OpCallC0 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B > 2 &&
        instruction.C == 0;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results, Limit = _R(Stk[A](Unpack(Stk, A + 1, Inst[OP_B])))
Top = Limit + A - 1
local Edx = 0;
for Idx = A, Top do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end;
";
    public override void Mutate(Instruction instruction)
    {
        instruction.B += instruction.A - 1;
    }
}

public class OpCallC0B2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 2 &&
        instruction.C == 0;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results, Limit = _R(Stk[A](Stk[A + 1]))
Top = Limit + A - 1
local Edx = 0;
for Idx = A, Top do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end;
";
    public override void Mutate(Instruction instruction)
    {
        instruction.B += instruction.A - 1;
    }
}

public class OpCallC1 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B > 2 &&
        instruction.C == 1;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A](Unpack(Stk, A + 1, Inst[OP_B]))
";
    public override void Mutate(Instruction instruction)
    {
        instruction.B += instruction.A - 1;
    }
}

public class OpCallC1B2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 2 &&
        instruction.C == 1;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A](Stk[A + 1])
";
}

public class OpCallB0C0 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 0 &&
        instruction.C == 0;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results, Limit = _R(Stk[A](Unpack(Stk, A + 1, Top)))
Top = Limit + A - 1
local Edx = 0;
for Idx = A, Top do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end;
";
}

public class OpCallB0C1 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 0 &&
        instruction.C == 1;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A](Unpack(Stk, A + 1, Top))
";
}

public class OpCallB1C0 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 1 &&
        instruction.C == 0;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
local Results, Limit = _R(Stk[A]())
Top = Limit + A - 1
local Edx = 0;
for Idx = A, Top do 
	Edx = Edx + 1;
	Stk[Idx] = Results[Edx];
end;
";
}

public class OpCallB1C1 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 1 &&
        instruction.C == 1;

    public override string GetObfuscated(ObfuscationContext context) =>
        "Stk[Inst[OP_A]]();";
}

public class OpCallC2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B > 2 &&
        instruction.C == 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A] = Stk[A](Unpack(Stk, A + 1, Inst[OP_B])) 
";
    public override void Mutate(Instruction instruction)
    {
        instruction.B += instruction.A - 1;
    }
}

public class OpCallC2B2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 2 &&
        instruction.C == 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A] = Stk[A](Stk[A + 1]) 
";
}

public class OpCallB0C2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 0 &&
        instruction.C == 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A] = Stk[A](Unpack(Stk, A + 1, Top))
";
}

public class OpCallB1C2 : VOpCode
{
    public override bool IsInstruction(Instruction instruction) =>
        instruction.OpCode == OpCode.Call && instruction.B == 1 &&
        instruction.C == 2;

    public override string GetObfuscated(ObfuscationContext context) =>
        @"
local A = Inst[OP_A]
Stk[A] = Stk[A]()
";
}
