using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Helphers
{
    public class RequiredWithNonDefaultAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = base.IsValid(value);
            if (result  && GetType().IsValueType)
            {
                var type = GetType();
                if (System.Activator.CreateInstance(type) == value)
                    return false;
            }
            return result;
        }
    }
}