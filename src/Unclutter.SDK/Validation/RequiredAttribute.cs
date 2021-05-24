namespace Unclutter.SDK.Validation
{
    public class RequiredAttribute : LocalizedValidationAttribute
    {
        private readonly System.ComponentModel.DataAnnotations.RequiredAttribute _requiredAttribute;
        public bool AllowEmptyStrings
        {
            get => _requiredAttribute.AllowEmptyStrings;
            set => _requiredAttribute.AllowEmptyStrings = value;
        }
        public RequiredAttribute()
        {
            _requiredAttribute = new System.ComponentModel.DataAnnotations.RequiredAttribute();
        }
        public override bool Equals(object? obj)
        {
            return _requiredAttribute.Equals(obj);
        }
        public override string FormatErrorMessage(string name)
        {
            return _requiredAttribute.FormatErrorMessage(name);
        }
        public override int GetHashCode()
        {
            return _requiredAttribute.GetHashCode();
        }
        public override bool IsDefaultAttribute()
        {
            return _requiredAttribute.IsDefaultAttribute();
        }
        public override bool IsValid(object value)
        {
            return _requiredAttribute.IsValid(value);
        }
        public override bool Match(object? obj)
        {
            return _requiredAttribute.Match(obj);
        }
        public override bool RequiresValidationContext => _requiredAttribute.RequiresValidationContext;
        public override object TypeId => _requiredAttribute.TypeId;
        public override string ToString()
        {
            return _requiredAttribute.ToString();
        }
    }
}
