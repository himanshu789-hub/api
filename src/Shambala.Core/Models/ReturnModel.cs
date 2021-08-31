namespace Shambala.Core.Models
{
    public class ResultModel
    {
        int code;
        string name;

        public bool IsValid;
        public object Content { get ;set; }
        public int Code { get { return code; } set { if (IsValid) code = 0; else code = value; } }
        public string Name { get { return  name;} set { if (IsValid) name = null; else name = value; } }
    }

    class ProductFlavourElement
    {
        public int ProductId{get;set;}
        public short FlavourId{get;set;}
    }
}