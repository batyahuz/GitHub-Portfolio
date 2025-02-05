using Octokit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

namespace GitHubServiceLibrary;

public class GitHubService : IGitHubService
{
    private readonly GitHubClient _client;
    private readonly string _username;
    private readonly IMemoryCache _cache;

    public GitHubService(IOptions<GitHubOptions> options, IMemoryCache cache)
    {
        var credentials = new Credentials(options.Value.Token);
        _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolioApp"))
        {
            Credentials = credentials
        };
        _username = options.Value.Username;
        _cache = cache;
    }

    public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
    {
        if (_cache.TryGetValue("GitHubPortfolio", out IReadOnlyList<Repository> cachedRepositories))
        {
            return cachedRepositories;
        }

        var repositories = await _client.Repository.GetAllForUser(_username);
        _cache.Set("GitHubPortfolio", repositories, TimeSpan.FromMinutes(10));
        return repositories;
    }

    public async Task<SearchRepositoryResult> SearchRepositoriesAsync(string name, string language, string user)
    {
        var request = new SearchRepositoriesRequest(name)
        {
            Language = Enum.TryParse<Language>(language, true, out var lang) ? lang : null,
            User = user
        };

        return await _client.Search.SearchRepo(request);
    }
}
