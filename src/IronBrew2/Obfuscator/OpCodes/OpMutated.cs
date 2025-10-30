using IronBrew2.Bytecode.IR;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpMutated : VOpCode
    {
        public static Random rand = new Random();

        public required VOpCode Mutated;
        public required int[] Registers;

        public static string[] RegisterReplacements = { "OP__A", "OP__B", "OP__C" };

        public override bool IsInstruction(Instruction instruction) =>
            false;

        public bool CheckInstruction() => rand.Next(1, 15) == 1;

        public override string GetObfuscated(ObfuscationContext context)
        {
            return Mutated.GetObfuscated(context);
        }

        public override void Mutate(Instruction instruction)
        {
            Mutated.Mutate(instruction);
        }
    }
}