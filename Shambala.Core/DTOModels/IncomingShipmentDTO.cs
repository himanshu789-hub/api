namespace Shambala.Core.DTOModels
{
    public class IncomingShipmentDTO
    {

        public int Id { get; set; }
        public short TotalRecievedPieces { get; set; }
        public short TotalDefectPieces { get; set; }
        public byte CaretSize { get; set; }

        public virtual ProductDTO ProductIdFkNavigation { get; set; }
    }
}