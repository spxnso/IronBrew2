using IronBrew2.Bytecode.Library;

namespace IronBrew2.Bytecode.IR;

public class Chunk
{
    public string Name;
    public int Line;
    public int LastLine;
    public byte UpvalueCount;
    public byte ParameterCount;
    public byte VarargFlag;
    public byte StackSize;
    public int CurrentOffset = 0;
    public int CurrentParamOffset = 0;
    public List<Instruction> Instructions;
    public Dictionary<Instruction, int> InstructionMap = new Dictionary<Instruction, int>();
    public List<Constant> Constants;
    public Dictionary<Constant, int> ConstantMap = new Dictionary<Constant, int>();
    public List<Chunk> Functions;
    public Dictionary<Chunk, int> FunctionMap = new Dictionary<Chunk, int>();
    public List<string> Upvalues;


    public Chunk(string name)
    {
        Name = name;
        Instructions = new List<Instruction>();
        Constants = new List<Constant>();
        Functions = new List<Chunk>();
        Upvalues = new List<string>();
    }

    public void UpdateMappings()
    {
        InstructionMap.Clear();
        ConstantMap.Clear();
        FunctionMap.Clear();

        for (int i = 0; i < Instructions.Count; i++)
            InstructionMap.Add(Instructions[i], i);

        for (int i = 0; i < Constants.Count; i++)
            ConstantMap.Add(Constants[i], i);

        for (int i = 0; i < Functions.Count; i++)
            FunctionMap.Add(Functions[i], i);
    }

    public int Rebase(int offset, int paramOffset = 0)
    {
        offset -= CurrentOffset;
        paramOffset -= CurrentParamOffset;

        CurrentOffset += offset;
        CurrentParamOffset += paramOffset;

        StackSize = (byte)(StackSize + offset);

        //thanks lua for not distinguishing parameters and regular stack values!
        var Params = ParameterCount - 1;
        for (var i = 0; i < Instructions.Count; i++)
        {
            var instr = Instructions[i];

            switch (instr.OpCode)
            {
                case OpCode.Move:
                case OpCode.LoadNil:
                case OpCode.Unm:
                case OpCode.Not:
                case OpCode.Len:
                case OpCode.TestSet:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        if (instr.B > Params)
                            instr.B += offset;
                        else
                            instr.B += paramOffset;
                        break;
                    }
                case OpCode.LoadConst:
                case OpCode.LoadBool:
                case OpCode.GetGlobal:
                case OpCode.SetGlobal:
                case OpCode.GetUpval:
                case OpCode.SetUpval:
                case OpCode.Call:
                case OpCode.TailCall:
                case OpCode.Return:
                case OpCode.VarArg:
                case OpCode.Test:
                case OpCode.ForPrep:
                case OpCode.ForLoop:
                case OpCode.TForLoop:
                case OpCode.NewTable:
                case OpCode.SetList:
                case OpCode.Close:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;
                        break;
                    }
                case OpCode.GetTable:
                case OpCode.SetTable:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        if (instr.B < 255)
                        {
                            if (instr.B > Params)
                                instr.B += offset;
                            else
                                instr.B += paramOffset;
                        }

                        if (instr.C > Params)
                            instr.C += offset;
                        else
                            instr.C += paramOffset;

                        break;
                    }
                case OpCode.Add:
                case OpCode.Sub:
                case OpCode.Mul:
                case OpCode.Div:
                case OpCode.Mod:
                case OpCode.Pow:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        if (instr.B < 255)
                        {
                            if (instr.B > Params)
                                instr.B += offset;
                            else
                                instr.B += paramOffset;
                        }

                        if (instr.C < 255)
                        {
                            if (instr.C > Params)
                                instr.C += offset;
                            else
                                instr.C += paramOffset;
                        }

                        break;
                    }
                case OpCode.Concat:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        if (instr.B > Params)
                            instr.B += offset;
                        else
                            instr.B += paramOffset;

                        if (instr.C > Params)
                            instr.C += offset;
                        else
                            instr.C += paramOffset;

                        break;
                    }
                case OpCode.Self:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        if (instr.B > Params)
                            instr.B += offset;
                        else
                            instr.B += paramOffset;

                        if (instr.C < 255)
                        {
                            if (instr.C > Params)
                                instr.C += offset;
                            else
                                instr.C += paramOffset;
                        }

                        break;
                    }
                case OpCode.Eq:
                case OpCode.Lt:
                case OpCode.Le:
                    {
                        if (instr.B < 255)
                        {
                            if (instr.B > Params)
                                instr.B += offset;
                            else
                                instr.B += paramOffset;
                        }

                        if (instr.C < 255)
                        {
                            if (instr.C > Params)
                                instr.C += offset;
                            else
                                instr.C += paramOffset;
                        }

                        break;
                    }
                case OpCode.Closure:
                    {
                        if (instr.A > Params)
                            instr.A += offset;
                        else
                            instr.A += paramOffset;

                        var nProto = Functions[instr.B];

                        //fuck you lua
                        for (var i2 = 0; i2 < nProto.UpvalueCount; i2++)
                        {
                            var cInst = Instructions[i + i2 + 1];

                            if (cInst.OpCode != OpCode.Move)
                                continue;

                            if (cInst.B > Params)
                                cInst.B += offset;
                            else
                                cInst.B += paramOffset;
                        }

                        i += nProto.UpvalueCount;
                        break;
                    }
            }
        }

        return ParameterCount;
    }
}