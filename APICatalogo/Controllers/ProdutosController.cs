using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;
[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly AppDBContext _context;

    public ProdutosController(AppDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _context.Produtos.ToList();
        if (produtos is null) 
        {
            return NotFound("Produtos não encontrados...");
        }
        return produtos;
    }

    [HttpGet("{id:int}", Name ="ObterProduto")]
    public IActionResult Get(int id)
    {
        var produto = _context.Produtos.Find(id);
        if (produto is null) return NotFound("Produto não encontrado");
        return Ok(produto);
    }

    public ActionResult Post(Produto produto)
    {
        if (produto is null) return BadRequest();

        _context.Produtos.Add(produto);
        _context.SaveChanges();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = produto.ProdutoId }, produto);
    }
}
