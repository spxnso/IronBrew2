using System.Text;
using System.Text.RegularExpressions;
using IronBrew2.Bytecode.IR;

namespace IronBrew2.Obfuscator.OpCodes;

public class OpSuperOperator : VOpCode
{
    public VOpCode[] SubOpCodes { get; }

    public OpSuperOperator()
    {
        SubOpCodes = Array.Empty<VOpCode>();
    }
    
    public OpSuperOperator(VOpCode[] subOpcodes)
    {
        SubOpCodes = subOpcodes ?? Array.Empty<VOpCode>();
    }

    public override bool IsInstruction(Instruction instruction) => false;

    public bool IsInstruction(List<Instruction> instructions)
    {
        if (instructions == null) return false;
        if (instructions.Count != SubOpCodes.Length) return false;

        for (int i = 0; i < SubOpCodes.Length; i++)
        {
            var expected = SubOpCodes[i];
            if (expected is OpMutated mut)
            {
                if (!mut.Mutated.IsInstruction(instructions[i])) return false;
            }
            else if (!expected.IsInstruction(instructions[i])) return false;
        }

        return true;
    }

    public override string GetObfuscated(ObfuscationContext context)
    {
        var sb = new StringBuilder();
        var locals = new List<string>();

        for (var index = 0; index < SubOpCodes.Length; index++)
        {
            var subopcode = SubOpCodes[index];
            string s2 = subopcode.GetObfuscated(context);

            var reg = new Regex("local(.*?)[;=]");
            foreach (Match m in reg.Matches(s2))
            {
                string loc = m.Groups[1].Value.Replace(" ", "");
                if (!locals.Contains(loc)) locals.Add(loc);

                if (!m.Value.Contains(";"))
                    s2 = s2.Replace($"local{m.Groups[1].Value}", loc);
                else
                    s2 = s2.Replace($"local{m.Groups[1].Value};", "");
            }

            sb.Append(s2);
            if (index + 1 < SubOpCodes.Length)
                sb.Append("InstrPoint = InstrPoint + 1;Inst = Instr[InstrPoint];");
        }

        if (locals.Count > 0)
        {
            var localsPrefix = string.Join(", ", locals);
            return "local " + localsPrefix + ';' + sb.ToString();
        }

        return sb.ToString();
    }
}
