using IronBrew2.Bytecode.Library;
using IronBrew2.Obfuscator;

namespace IronBrew2.Bytecode.IR;

public class Instruction
{
    public object?[] RefOperands = new object?[3];
    public List<Instruction> BackReferences = new List<Instruction>();

    public Chunk Chunk;
    public OpCode OpCode;
    public InstructionType InstructionType;
    public InstructionConstantMask ConstantMask;

    public int A;
    public int B;
    public int C;

    public int Data;
    public int PC;
    public int Line;

    public CustomInstructionData? CustomData;

    public Instruction(Instruction other)
    {
        RefOperands = other.RefOperands.ToArray();
        BackReferences = other.BackReferences.ToList();
        Chunk = other.Chunk;
        OpCode = other.OpCode;
        InstructionType = other.InstructionType;
        A = other.A;
        B = other.B;
        C = other.C;
        Data = other.Data;
        PC = other.PC;
        Line = other.Line;
        CustomData = other.CustomData;
    }

    public Instruction(Chunk chunk, OpCode code, params object?[] refOperands)
    {
        Chunk = chunk;
        OpCode = code;
        A = 0;
        B = 0;
        C = 0;
        Data = 0;

        if (Deserializer.InstructionMappings.TryGetValue(code, out InstructionType type))
            InstructionType = type;
        else
            InstructionType = InstructionType.ABC;

        for (int i = 0; i < refOperands.Length; i++)
        {
            var op = refOperands[i];
            RefOperands[i] = op;

            if (op is Instruction ins)
                ins.BackReferences.Add(this);
        }
    }

    public void UpdateRegisters()
    {
        if (InstructionType == InstructionType.Data)
            return;

        PC = Chunk.InstructionMap[this];

        switch (OpCode)
        {
            case OpCode.LoadConst:
            case OpCode.GetGlobal:
            case OpCode.SetGlobal:
                if (RefOperands[0] is Constant c0)
                    B = Chunk.ConstantMap[c0];
                break;

            case OpCode.Jmp:
            case OpCode.ForLoop:
            case OpCode.ForPrep:
                if (RefOperands[0] is Instruction i0)
                    B = Chunk.InstructionMap[i0] - PC - 1;
                break;

            case OpCode.Closure:
                if (RefOperands[0] is Chunk f0)
                    B = Chunk.FunctionMap[f0];
                break;

            case OpCode.GetTable:
            case OpCode.SetTable:
            case OpCode.Add:
            case OpCode.Sub:
            case OpCode.Mul:
            case OpCode.Div:
            case OpCode.Mod:
            case OpCode.Pow:
            case OpCode.Eq:
            case OpCode.Lt:
            case OpCode.Le:
            case OpCode.Self:
                if (RefOperands[0] is Constant cB)
                    B = Chunk.ConstantMap[cB] + 256;
                if (RefOperands[1] is Constant cC)
                    C = Chunk.ConstantMap[cC] + 256;
                break;
        }
    }

    public void SetupRefs()
    {
        RefOperands = new object?[3] { null, null, null };

        switch (OpCode)
        {
            case OpCode.LoadConst:
            case OpCode.GetGlobal:
            case OpCode.SetGlobal:
                RefOperands[0] = Chunk.Constants[B];
                ((Constant)RefOperands[0]!).BackReferences.Add(this);
                break;

            case OpCode.Jmp:
            case OpCode.ForLoop:
            case OpCode.ForPrep:
                RefOperands[0] = Chunk.Instructions[Chunk.InstructionMap[this] + B + 1];
                ((Instruction)RefOperands[0]!).BackReferences.Add(this);
                break;

            case OpCode.Closure:
                RefOperands[0] = Chunk.Functions[B];
                break;

            case OpCode.GetTable:
            case OpCode.SetTable:
            case OpCode.Add:
            case OpCode.Sub:
            case OpCode.Mul:
            case OpCode.Div:
            case OpCode.Mod:
            case OpCode.Pow:
            case OpCode.Eq:
            case OpCode.Lt:
            case OpCode.Le:
            case OpCode.Self:
                if (B > 255)
                    RefOperands[0] = Chunk.Constants[B - 256];
                if (C > 255)
                    RefOperands[1] = Chunk.Constants[C - 256];
                break;
        }
    }
}
