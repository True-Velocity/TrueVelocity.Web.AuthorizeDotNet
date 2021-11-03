namespace Bet.Extensions.AuthorizeNet.Logging;

public class SensitiveTag
{
    public SensitiveTag(string tagName, string pattern, string replacement, bool disableMask)
    {
        TagName = tagName;
        Pattern = pattern;
        Replacement = replacement;
        DisableMask = disableMask;
    }

    public string TagName { get; set; }

    public string Pattern { get; set; }

    public string Replacement { get; set; }

    public bool DisableMask { get; set; }
}
