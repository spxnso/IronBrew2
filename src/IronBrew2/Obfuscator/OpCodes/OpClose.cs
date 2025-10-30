using IronBrew2.Bytecode.IR;
using IronBrew2.Bytecode.Library;

namespace IronBrew2.Obfuscator.OpCodes
{
    public class OpClose : VOpCode
    {
        public override bool IsInstruction(Instruction instruction) =>
            instruction.OpCode == OpCode.Close;

        public override string GetObfuscated(ObfuscationContext context) =>
            "local A=Inst[OP_A];local Cls={};for Idx=1,#Lupvals do local List=Lupvals[Idx];for Idz=0,#List do local Upv=List[Idz];local NStk=Upv[1];local Pos=Upv[2]; if NStk==Stk and Pos>=A then Cls[Pos]=NStk[Pos];Upv[1]=Cls;end;end;end;";
    }
}