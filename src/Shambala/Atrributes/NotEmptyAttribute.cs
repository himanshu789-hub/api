using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Attributes
{
    public class NotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // if value is complex then it should not be null
            string Name = value.GetType().Name;

            if (value is null)
            {
                this.ErrorMessage = $"The Field {Name} cannot be null";
                return false;
            }
            var type = value.GetType();
            if (type.IsValueType)
            {
                var defaultValue = System.Activator.CreateInstance(type);
                bool result = value.Equals(defaultValue);
                if (result)
                    this.ErrorMessage = $"The field {Name} cannot be Empty";
                return !result;
            }
            return true;
        }
    }
}