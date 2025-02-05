using Octokit;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GitHubServiceLibrary;

public class GitHubCacheManager
{
    private readonly GitHubClient _client;
    private readonly IMemoryCache _cache;
    private readonly string _username;

    public GitHubCacheManager(IOptions<GitHubOptions> options, IMemoryCache cache)
    {
        var credentials = new Credentials(options.Value.Token);
        _client = new GitHubClient(new ProductHeaderValue("GitHubPortfolioApp"))
        {
            Credentials = credentials
        };
        _username = options.Value.Username;
        _cache = cache;
    }

    public async Task<bool> IsCacheStale()
    {
        var lastCommitCacheKey = "LastGitHubCommit";
        if (!_cache.TryGetValue(lastCommitCacheKey, out DateTime lastCachedCommit))
        {
            return true;
        }

        var repositories = await _client.Repository.GetAllForUser(_username);
        var lastCommitDate = repositories.Max(r => r.UpdatedAt.DateTime);

        if (lastCommitDate > lastCachedCommit)
        {
            _cache.Set(lastCommitCacheKey, lastCommitDate, TimeSpan.FromMinutes(10));
            return true;
        }

        return false;
    }
}
