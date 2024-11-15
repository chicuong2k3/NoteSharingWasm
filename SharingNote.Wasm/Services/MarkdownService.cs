using Markdig;
using Markdig.Syntax;
using System.Text.RegularExpressions;
using System.Text;

namespace SharingNote.Wasm.Services;

public class Heading
{
    public string Name { get; set; } = string.Empty;
    public string Anchor { get; set; } = string.Empty;
    public List<Heading> Children { get; set; } = new();
}
public class MarkdownService
{
    private MarkdownDocument GetMarkdownDocument(string markdownContent)
    {
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.Parse(markdownContent, pipeline);
    }
    private string ExtractHeadingText(HeadingBlock heading)
    {
        if (heading == null || heading.Inline == null)
        {
            return string.Empty;
        }

        var text = new StringBuilder();
        foreach (var inline in heading.Inline)
        {
            text.Append(inline.ToString());
        }
        return text.ToString().Trim();
    }
    private string GenerateAnchorName(string headingText)
    {
        var anchor = headingText.ToLower().Replace(" ", "-");
        anchor = Regex.Replace(anchor, @"-+", "-");
        anchor = Regex.Replace(anchor, @"[^a-z0-9\-]", "");

        return anchor;
    }
    public List<Heading> GetHeadingsFromMarkdown(string markdown)
    {

        var markdownDocument = GetMarkdownDocument(markdown);
        var headingList = markdownDocument.Descendants<HeadingBlock>().ToList();

        if (!headingList.Any())
        {
            return [];
        }
        var h1List = new List<Heading>();

        var currentHeadings = new Dictionary<int, Heading>();
        var prevLevel = 0;

        var minLevel = headingList.Min(h => h.Level);

        foreach (var heading in headingList)
        {
            var headingText = ExtractHeadingText(heading);
            var anchor = GenerateAnchorName(headingText.Trim());
            var currentLevel = heading.Level;

            var newHeading = new Heading
            {
                Name = headingText,
                Anchor = anchor,
                Children = new List<Heading>()
            };

            if (currentLevel == minLevel)
            {
                currentHeadings.Clear();
                h1List.Add(newHeading);
                currentHeadings[1] = newHeading;
            }
            else
            {
                var parentLevel = currentLevel - 1;
                Heading? parentHeading = null;
                while (parentLevel >= minLevel &&
                    !currentHeadings.TryGetValue(parentLevel, out parentHeading))
                {
                    parentLevel--;
                }

                if (parentHeading != null)
                {
                    // Add new heading as a child of the identified parent
                    parentHeading.Children.Add(newHeading);
                }

                currentHeadings[currentLevel] = newHeading;
            }

            prevLevel = currentLevel;
        }

        return h1List;
    }
    public string AddNumberingToMarkdownHeadings(string markdownContent)
    {
        var lines = markdownContent.Split('\n');
        var headingCount = new int[6]; // Track counts for each heading level
        var result = new StringBuilder();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (trimmedLine.StartsWith("#"))
            {
                // Determine the heading level based on the number of '#' characters
                var level = trimmedLine.TakeWhile(c => c == '#').Count();

                if (level >= 1 && level <= 6)
                {
                    // Reset the counts for deeper levels when we encounter a higher-level heading
                    for (var i = level; i < 6; i++)
                    {
                        headingCount[i] = 0;
                    }

                    // Increment the current level's count
                    headingCount[level - 1]++;

                    // Build the numbering by joining counts for all levels up to the current heading level
                    var number = string.Join(".", headingCount.Take(level).Where(n => n > 0));

                    // Add the numbered heading to the result
                    result.AppendLine($"{new string('#', level)} {number}. {trimmedLine.Substring(level).Trim()}");
                }
            }
            else
            {
                // For non-heading lines, simply append them
                result.AppendLine(line);
            }
        }

        return result.ToString();
    }

}
