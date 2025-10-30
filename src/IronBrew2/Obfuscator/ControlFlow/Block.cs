using IronBrew2.Bytecode.IR;


namespace IronBrew2.Obfuscator.ControlFlow
{
    public class Block
    {
        public Chunk Chunk;
        public List<Instruction> Body = new List<Instruction>();
        public Block? Successor = null;

        public Block(Chunk c) =>
            Chunk = c;
    }
}