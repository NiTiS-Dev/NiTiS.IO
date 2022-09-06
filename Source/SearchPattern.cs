using Microsoft.Extensions.FileSystemGlobbing;

namespace NiTiS.IO;

public ref struct SearchPattern
{
	private readonly Matcher matcher;

	public SearchPattern()
	{
		matcher = new();
	}
	public SearchPattern Include(string pattern)
	{
		matcher.AddInclude(pattern);

		return this;
	}
	public SearchPattern Exclude(string pattern)
	{
		matcher.AddExclude(pattern);

		return this;
	}
}
