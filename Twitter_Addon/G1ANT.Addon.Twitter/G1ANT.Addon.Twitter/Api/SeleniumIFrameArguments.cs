using G1ANT.Language;

namespace G1ANT.Addon.Twitter
{
    public class SeleniumIFrameArguments : CommandArguments
    {
        [Argument(Tooltip = "Phrase to find an iframe by")]
        public TextStructure IFrameSearch { get; set; }

        [Argument(Tooltip = "Specifies an iframe selector: 'id', 'class', 'cssselector', 'tag', 'xpath', 'name', 'query', 'jquery'")]
        public TextStructure IFrameBy { get; set; } = new TextStructure(ElementSearchBy.Id.ToString().ToLower());
    }
}
