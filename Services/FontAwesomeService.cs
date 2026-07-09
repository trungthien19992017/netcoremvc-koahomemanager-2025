public class FontAwesomeService
{
  public HashSet<string> Icons { get; }

  public FontAwesomeService(IWebHostEnvironment env)
  {
    var path = Path.Combine(
        env.WebRootPath,
        "fontawesome-free-icons.txt");

    Icons = File.ReadAllLines(path)
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Select(x => x.Trim())
        .ToHashSet(StringComparer.OrdinalIgnoreCase);
  }
}
