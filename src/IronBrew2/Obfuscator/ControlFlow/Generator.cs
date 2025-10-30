using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;


namespace IronBrew2.Obfuscator.ControlFlow
{
    public class CFGenerator
    {
        public Random Random = new Random();

        public Instruction NextJMP(Chunk lc, Instruction Reference) =>
            new Instruction(lc, OpCode.Jmp, Reference);

        public Instruction BelievableRandom(Chunk lc)
        {
            Instruction ins = new Instruction(lc, (OpCode)Random.Next(0, 37));

            ins.A = Random.Next(0, 128);
            ins.B = Random.Next(0, 128);
            ins.C = Random.Next(0, 128);

            while (true)
            {
                switch (ins.OpCode)
                {
                    case OpCode.LoadConst:
                    case OpCode.GetGlobal:
                    case OpCode.SetGlobal:
                    case OpCode.Jmp:
                    case OpCode.ForLoop:
                    case OpCode.TForLoop:
                    case OpCode.ForPrep:
                    case OpCode.Closure:
                    case OpCode.GetTable:
                    case OpCode.SetTable:
                    case OpCode.Add:
                    case OpCode.Sub:
                    case OpCode.Mul:
                    case OpCode.Div:
                    case OpCode.Mod:
                    case OpCode.Pow:
                    case OpCode.Test:
                    case OpCode.TestSet:
                    case OpCode.Eq:
                    case OpCode.Lt:
                    case OpCode.Le:
                    case OpCode.Self:
                        ins.OpCode = (OpCode)Random.Next(0, 37);
                        continue;

                    default:
                        return ins;
                }
            }
        }

        public Constant GetOrAddConstant(Chunk chunk, ConstantType type, dynamic constant, out int constantIndex)
        {
            var current =
                chunk.Constants.FirstOrDefault(c => c.Type == type &&
                                                    c.Data == constant); // type checking to prevent errors i guess
            if (current != null)
            {
                constantIndex = chunk.Constants.IndexOf(current);
                return current;
            }

            Constant newConst = new Constant(type, constant);


            constantIndex = chunk.Constants.Count;

            chunk.Constants.Add(newConst);
            chunk.ConstantMap.Add(newConst, constantIndex);

            return newConst;
        }
    }
}