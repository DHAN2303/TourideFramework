using System.Text.RegularExpressions;

namespace Touride.Abstraction.Constants
{
    public class ValidationConstants
    {
        public const string NotNull = " can not be null.";
        public const string NotEmpty = " can not be empty.";
        public const string Greater = " must be greater than 0.";
        public static readonly Regex EmojiPattern = new Regex(@"^(?!.*(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])).*$");
    }
}