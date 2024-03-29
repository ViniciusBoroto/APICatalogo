﻿using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly AppDBContext _context;

    public CategoriasController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> Get()
    {
        return _context.Categorias.Take(5).AsNoTracking().ToList();
    }

    [HttpGet("Produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
    {
        return _context.Categorias.Include(x => x.Produtos.Take(5)).Take(5).ToList();
    }

    [HttpGet("{id:int}", Name ="ObterCategoria")]
    public ActionResult<Categoria> Get(int id) 
    {
        var categoria = _context.Categorias.AsNoTracking().FirstOrDefault(p => p.CategoriaId == id); 

        if (categoria == null) 
            return NotFound("Categoria não encontrada...");

        return Ok(categoria);
    }

    [HttpPost]
    public ActionResult Post(Categoria categoria)
    {
        if (categoria == null)
           return BadRequest();

        _context.Categorias.Add(categoria);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Categoria categoria) 
    {
        if (id != categoria.CategoriaId)
            return BadRequest();

        _context.Entry(categoria).State = 
            Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();
        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id) 
    {
        var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);

        if (categoria is null)
           return NotFound("Categoria não encontrada...");

        _context.Categorias.Remove(categoria);
        _context.SaveChanges();
        return Ok(categoria);
    }


}
