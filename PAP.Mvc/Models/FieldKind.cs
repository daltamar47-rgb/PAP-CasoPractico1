namespace PAP.Mvc.Models
{
    /// <summary>
    /// How a column is rendered in the form and in the details modal.
    /// </summary>
    public enum FieldKind
    {
        Hidden,
        Text,
        TextArea,
        Number,
        Decimal,
        Date,
        DateTime,
        Checkbox,
        Select,
        Password
    }
}
