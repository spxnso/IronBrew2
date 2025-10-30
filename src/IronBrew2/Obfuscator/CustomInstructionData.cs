namespace IronBrew2.Obfuscator;

public class CustomInstructionData
{
    public CustomInstructionData(VOpCode opCode, VOpCode? writtenOpCode = null)
    {
        OpCode = opCode;
        WrittenOpCode = writtenOpCode;
    }

    public VOpCode OpCode { get; set; }
    public VOpCode? WrittenOpCode { get; set;  }
}