using System;
namespace Shambala.Repository
{
    static class Utility
    {
        internal static bool NumericOnly(TypeCode typeCode)
        {
            bool isNumeric = false;
            switch (typeCode)
            {
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    isNumeric = true; break;
                default:
                    break;
            }
            return isNumeric;
        }
    }
}