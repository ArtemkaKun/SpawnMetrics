namespace SharedConstants;

public static class GitConstants
{
    public const int ShortCommitHashLength = 8;

    public const int MinBaseCommitGitHubUrlLength = 30;
    public const string BaseCommitGitHubUrlShorterErrorMessage = "Base GitHub commit URL can't be shorter than 30 characters since it's always \'https://github.com/*/*/commit/\'";

    public const int MinCommitGitHubUrlLength = MinBaseCommitGitHubUrlLength + ShortCommitHashLength;
    public const int MaxCommitGitHubUrlLength = 100;
    public const string CommitGitHubUrlShorterErrorMessage = "GitHub commit URL can't be shorter than 38 characters since it's always \'https://github.com/*/*/commit/\' + 8 characters of short commit hash";

    public const string BaseGitHubUrlCheckRegex = @"https:\/\/github\.com\/[^\/]+\/[^\/]+\/commit\/";
    public const string GitHubUrlCheckRegex = BaseGitHubUrlCheckRegex + @"[\da-fA-F]{8}";
}