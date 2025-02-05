using Octokit;

namespace GitHubServiceLibrary;

public interface IGitHubService
{
    Task<IReadOnlyList<Repository>> GetRepositoriesAsync();
    Task<SearchRepositoryResult> SearchRepositoriesAsync(string name, string language, string user);
}
