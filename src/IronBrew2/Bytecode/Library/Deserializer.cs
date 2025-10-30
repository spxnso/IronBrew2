using System.Runtime.CompilerServices;
using System.Text;
using IronBrew2.Bytecode.IR;

namespace IronBrew2.Bytecode.Library;

public class Deserializer
{
    private MemoryStream _stream;

    private bool _bigEndian;
    private byte _sizeNumber;
    private byte _sizeSizeT;
    private Encoding _fuckingLua = Encoding.GetEncoding(28591);

    private bool _expectingSetlistData;

    public static Dictionary<OpCode, InstructionType> InstructionMappings = new Dictionary<OpCode, InstructionType>()
        {
            { OpCode.Move, InstructionType.ABC },
            { OpCode.LoadConst, InstructionType.ABx },
            { OpCode.LoadBool, InstructionType.ABC },
            { OpCode.LoadNil, InstructionType.ABC },
            { OpCode.GetUpval, InstructionType.ABC },
            { OpCode.GetGlobal, InstructionType.ABx },
            { OpCode.GetTable, InstructionType.ABC },
            { OpCode.SetGlobal, InstructionType.ABx },
            { OpCode.SetUpval, InstructionType.ABC },
            { OpCode.SetTable, InstructionType.ABC },
            { OpCode.NewTable, InstructionType.ABC },
            { OpCode.Self, InstructionType.ABC },
            { OpCode.Add, InstructionType.ABC },
            { OpCode.Sub, InstructionType.ABC },
            { OpCode.Mul, InstructionType.ABC },
            { OpCode.Div, InstructionType.ABC },
            { OpCode.Mod, InstructionType.ABC },
            { OpCode.Pow, InstructionType.ABC },
            { OpCode.Unm, InstructionType.ABC },
            { OpCode.Not, InstructionType.ABC },
            { OpCode.Len, InstructionType.ABC },
            { OpCode.Concat, InstructionType.ABC },
            { OpCode.Jmp, InstructionType.AsBx },
            { OpCode.Eq, InstructionType.ABC },
            { OpCode.Lt, InstructionType.ABC },
            { OpCode.Le, InstructionType.ABC },
            { OpCode.Test, InstructionType.ABC },
            { OpCode.TestSet, InstructionType.ABC },
            { OpCode.Call, InstructionType.ABC },
            { OpCode.TailCall, InstructionType.ABC },
            { OpCode.Return, InstructionType.ABC },
            { OpCode.ForLoop, InstructionType.AsBx },
            { OpCode.ForPrep, InstructionType.AsBx },
            { OpCode.TForLoop, InstructionType.ABC },
            { OpCode.SetList, InstructionType.ABC },
            { OpCode.Close, InstructionType.ABC },
            { OpCode.Closure, InstructionType.ABx },
            { OpCode.VarArg, InstructionType.ABC }
        };

    public Deserializer(byte[] input) =>
        _stream = new MemoryStream(input);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte[] Read(int size, bool factorEndianness = true)
    {
        byte[] bytes = new byte[size];
        _stream.Read(bytes, 0, size);

        if (factorEndianness && (_bigEndian == BitConverter.IsLittleEndian)) //if factor in endianness AND endianness differs between the two versions
            bytes = bytes.Reverse().ToArray();

        return bytes;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadSizeT() =>
        _sizeSizeT == 4 ? ReadInt32() : ReadInt64();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public long ReadInt64() =>
        BitConverter.ToInt64(Read(8), 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int ReadInt32(bool factorEndianness = true) =>
        BitConverter.ToInt32(Read(4, factorEndianness), 0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte ReadByte() =>
        Read(1)[0];

    public string ReadString()
    {
        long c = ReadSizeT();
        int count = (int)c;

        if (count == 0)
            return "";

        byte[] val = Read(count, false);
        return _fuckingLua.GetString(val, 0, count - 1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double ReadDouble() =>
        BitConverter.ToDouble(Read(_sizeNumber), 0);

    public Instruction DecodeInstruction(Chunk chunk, int index)
    {
        int code = ReadInt32();
        Instruction i = new Instruction(chunk, (IronBrew2.Bytecode.Library.OpCode)(code & 0x3F));

        i.Data = code;

        if (_expectingSetlistData)
        {
            _expectingSetlistData = false;

            i.InstructionType = InstructionType.Data;
            return i;
        }

        i.A = (code >> 6) & 0xFF;

        switch (i.InstructionType)
        {
            //WHAT THE FUCK LUA
            case InstructionType.ABC:
                i.B = (code >> 6 + 8 + 9) & 0x1FF;
                i.C = (code >> 6 + 8) & 0x1FF;
                break;

            case InstructionType.ABx:
                i.B = (code >> 6 + 8) & 0x3FFFF;
                i.C = -1;
                break;

            case InstructionType.AsBx:
                i.B = ((code >> 6 + 8) & 0x3FFFF) - 131071;
                i.C = -1;
                break;
        }

        if (i.OpCode == OpCode.SetList && i.C == 0)
            _expectingSetlistData = true;

        return i;
    }

    public List<Instruction> DecodeInstructions(Chunk chunk)
    {
        List<Instruction> instructions = new List<Instruction>();

        int Count = ReadInt32();

        for (int i = 0; i < Count; i++)
            instructions.Add(DecodeInstruction(chunk, i));

        return instructions;
    }

    public Constant DecodeConstant()
    {
        byte typeByte = ReadByte();


        ConstantType constantType;
        object? constantData;
        switch (typeByte)
        {
            case 0:
                constantType = ConstantType.Nil;
                constantData = null;
                break;
            case 1:
                constantType = ConstantType.Boolean;
                constantData = ReadByte() != 0;
                break;
            case 3:
                constantType = ConstantType.Number;
                constantData = ReadDouble();
                break;
            case 4:
                constantType = ConstantType.String;
                constantData = ReadString();
                break;
            default:
                throw new InvalidOperationException($"Unknown constant type: {typeByte}");
        }

        Constant c = new Constant(constantType, constantData!);
        return c;
    }


    public List<Constant> DecodeConstants()
    {
        List<Constant> constants = new List<Constant>();

        int Count = ReadInt32();

        for (int i = 0; i < Count; i++)
            constants.Add(DecodeConstant());

        return constants;
    }

    public Chunk DecodeChunk()
    {
        Chunk c = new Chunk(ReadString())
        {
            Line = ReadInt32(),
            LastLine = ReadInt32(),
            UpvalueCount = ReadByte(),
            ParameterCount = ReadByte(),
            VarargFlag = ReadByte(),
            StackSize = ReadByte(),
            Upvalues = new List<string>()
        };

        c.Instructions = DecodeInstructions(c);
        c.Constants = DecodeConstants();
        c.Functions = DecodeChunks();

        c.UpdateMappings();

        foreach (var inst in c.Instructions)
            inst.SetupRefs();

        int count = ReadInt32();
        for (int i = 0; i < count; i++) // source line pos list
            c.Instructions[i].Line = ReadInt32();

        //skip other debug info cus fuckit.wav

        count = ReadInt32();
        for (int i = 0; i < count; i++) // local list
        {
            ReadString();
            ReadInt32();
            ReadInt32();
        }
        count = ReadInt32();
        for (int i = 0; i < count; i++) // upvalues
            c.Upvalues.Add(ReadString());

        return c;
    }

    public List<Chunk> DecodeChunks()
    {
        List<Chunk> Chunks = new List<Chunk>();

        int count = ReadInt32();

        for (int i = 0; i < count; i++)
            Chunks.Add(DecodeChunk());

        return Chunks;
    }

    public Chunk DecodeFile()
    {
        int header = ReadInt32();

        if (header != 0x1B4C7561 && header != 0x61754C1B)
            throw new Exception("Invalid luac file.");

        if (ReadByte() != 0x51)
            throw new Exception("Only Lua 5.1 is supported.");

        ReadByte(); //format official shit wtf

        _bigEndian = ReadByte() == 0;

        ReadByte(); //size of int (assume 4 fuck off)

        _sizeSizeT = ReadByte();

        ReadByte(); //size of instruction (fuck it not supporting anything else than default)

        _sizeNumber = ReadByte();

        ReadByte(); //not supporting integer number bullshit fuck off

        Chunk c = DecodeChunk();
        return c;
    }
}