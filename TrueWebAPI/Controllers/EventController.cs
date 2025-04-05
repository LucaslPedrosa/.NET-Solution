using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IEventRepository _repository;

    public EventController(IEventRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var events = await _repository.GetAllAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var evento = await _repository.GetByIdAsync(id);
        if (evento == null) return NotFound();
        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Event evento)
    {
        await _repository.AddAsync(evento);
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Event evento)
    {
        if (id != evento.Id) return BadRequest();
        await _repository.UpdateAsync(evento);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}