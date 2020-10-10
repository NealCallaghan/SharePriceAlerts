namespace SlackNotifications.Models
{
    using System.Runtime.Serialization;

    public enum AlertLevel
    {
        [EnumMember(Value = "normal")]
        Normal,
        [EnumMember(Value = "debug")]
        Debug,
        [EnumMember(Value = "error")]
        Error,
    }
}