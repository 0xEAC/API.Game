using Learning.Interfaces;
using Learning.Models;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AnimalsController : ControllerBase
  {

    private readonly IAnimals _animalsRepository;
    private readonly ILogger<AnimalsController> _logger;

    public AnimalsController(ILogger<AnimalsController> logger,
      IAnimals animalsRepository)
    {
      _logger = logger;
      _animalsRepository = animalsRepository;
    }

    [HttpGet("all")]
    public IEnumerable<Animal> GetAll() => _animalsRepository.GetAll();

    [HttpGet("status/{id}")]
    public Animal Get(Guid id) => _animalsRepository.Get(id);

    [HttpPost("generate")]
    public string Generate() => _animalsRepository.Generate();

    [HttpPost("feed/{id}")]
    public string Feed(Guid id) => _animalsRepository.Feed(id);

    [HttpPost("feedAll")]
    public string FeedAll() => _animalsRepository.FeedAll();

    [HttpPost("pet/{id}")]
    public string Pet(Guid id) => _animalsRepository.Pet(id);

    [HttpPost("breed/{first}/{second}")]
    public string Breed(Guid first, Guid second) => _animalsRepository.Breed(first, second);

    [HttpDelete("{id}")]
    public string Delete(Guid id) => _animalsRepository.Delete(id);
  }
}
