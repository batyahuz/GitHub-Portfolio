using GitHubServiceLibrary;
using Microsoft.AspNetCore.Mvc;

namespace GitHubPortfolioAPI.Controllers;

[ApiController]
[Route("api/github")]
public class GitHubController : ControllerBase
{
    private readonly IGitHubService _gitHubService;

    public GitHubController(IGitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    [HttpGet("portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        var repositories = await _gitHubService.GetRepositoriesAsync();
        return Ok(repositories.Select(repo => new
        {
            repo.Name,
            repo.Language,
            LastUpdated = repo.UpdatedAt,
            Stars = repo.StargazersCount,
            Forks = repo.ForksCount,
            repo.HtmlUrl
        }));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchRepositories([FromQuery] string name, [FromQuery] string language, [FromQuery] string user)
    {
        var result = await _gitHubService.SearchRepositoriesAsync(name, language, user);
        return Ok(result.Items.Select(repo => new
        {
            repo.Name,
            repo.Language,
            Owner = repo.Owner.Login,
            Stars = repo.StargazersCount,
            repo.HtmlUrl
        }));
    }
}
