using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TesteKeyworks.Models;
using TesteKeyworks.Models.Paginacao;
using TesteKeyworks.Models.ViewModels;
using TesteKeyworks.Services.Filmes;
using TesteKeyworks.Services.Streamings;


namespace TesteKeyworks.Controllers
{
    public class FilmesController : Controller
    {
        private readonly IFilmeService _service;
        private readonly IStreamingService _streamingService;
        private readonly int _pageSize = 3;
        public FilmesController(IFilmeService service, IStreamingService streamingService)
        {
            _service = service;
            _streamingService = streamingService;
        }

        // GET: Filmes
        public async Task<IActionResult> Index(int? page, string? genero, int? ano, string? titulo, string? streaming, int? avaliacao)
        {
            int pageNumber = page ?? 1;
            
            var filmes = (await _service.GetAllAsync(genero, ano, titulo, streaming, avaliacao)).ToList();

            ViewBag.anos = new SelectList(await _service.GetAnosLancamentoAsync());

            ViewBag.generos = new SelectList(await _service.GetGenerosAsync());

            ViewBag.streamings = new SelectList(await _streamingService.GetNomeStreamingsAsync());

            ViewBag.avaliacoes = new SelectList(Enumerable.Range(1, 5));

            ViewData["page"] = page;
            ViewData["Genero"] = genero;
            ViewData["Ano"] = ano.ToString();
            ViewData["Titulo"] = titulo;
            ViewData["Streaming"] = streaming;
            ViewData["Avaliacao"] = avaliacao.ToString();

            var filmesView = filmes.Select(x => (FilmeView)x);
            return View(PaginatedList<FilmeView>.Create(filmesView.ToList(), pageNumber, _pageSize));
        }


        // GET: Filmes/Details/5
        public async Task<IActionResult> Details(int? page, int? ano)
        {
            ViewBag.anos = new SelectList(await _service.GetAnosLancamentoAsync());

            if (!ano.HasValue) ano = DateTime.Now.Year;

            var consulta = await _service.GetAllAsync(null, ano, null, null, null);

            ViewData["page"] = page;
            ViewData["Ano"] = ano.ToString();
            ViewData["totalFilmes"] = await _service.GetTotalFilmesAsync();
            ViewData["totalGeneros"] = (await _service.GetGenerosAsync()).Count();
            ViewData["totalStreamings"] = (await _streamingService.GetNomeStreamingsAsync()).Count();
            var resultado = consulta.GroupBy(x => new { x.Genero, Ano = x.DataLancamento.Year })
                    .Select(y => new FilmeDetalheView
                    {
                        Genero = y.Key.Genero!,
                        AnoLancamento = y.Key.Ano.ToString(),
                        MediaAvaliacao = y.SelectMany(z => z.Avaliacoes).DefaultIfEmpty().Average(a => a != null ? a.Nota : 0).ToString("F2")
                    })
                    .ToList();

            return View(resultado);
        }

        // GET: Filmes/Create
        public async Task<IActionResult> Create()
        {
            FilmeView view = new();

            var streamings = (await _streamingService.GetAllAsync()).Select(x => (StreamingView)x);

            view.Streamings = streamings.ToList();

            return View(view);
        }

        // POST: Filmes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo, Genero, DataLancamento, Streamings, Id")] FilmeView view)
        {
            if (ModelState.IsValid)
            {
                Filme filme = view;
                List<Streaming> streamings = new();

                if (string.IsNullOrEmpty(view.Titulo))
                {
                    ModelState.AddModelError("Titulo", "O campo Título é obrigatório.");
                    return View(view);
                }

                if (string.IsNullOrEmpty(view.Genero))
                {
                    ModelState.AddModelError("Genero", "O campo Gênero é obrigatório.");
                    return View(view);
                }

                foreach (var streaming in view.Streamings)
                {
                    var streamingSelecionado = await _streamingService.GetByIdAsync(streaming.Id ?? Guid.Empty);

                    if (streamingSelecionado != null)
                        streamings.Add(streamingSelecionado);
                }

                filme.Streamings = streamings;

                try
                {
                    await _service.AddAsync(filme);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Titulo", "O nome do filme já existe. Por favor, escolha outro nome.");
                        return View(view);
                    }

                    throw;
                }


                return RedirectToAction(nameof(Index));
            }


            return View(view);
        }

        // GET: Filmes/Edit/5
        public async Task<IActionResult> Edit(Guid? id, int? page)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["page"] = page;

            var filme = await _service.GetByIdAsync(id.Value);
            
            if (filme == null)
            {
                return NotFound();
            }
            
            FilmeView filmeView = filme;
            
            var streamings = await _streamingService.GetAllAsync();
            
            List<StreamingView> streamingsView = new();
            foreach (var streaming in streamings)
            {
                streamingsView.Add(new()
                {
                    Id = streaming.Id,
                    Nome = streaming.Nome,
                    Selecionado = filme.Streamings.Any(x => x.Id == streaming.Id)
                });
            }
            
            filmeView.Streamings = streamingsView;

            return View(filmeView);
        }

        // POST: Filmes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Titulo,Genero, MediaAvaliacao,DataLancamento, Streamings, Id")] FilmeView filme)
        {
            if (id != filme.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var filmeAtual = await _service.GetByIdAsync(id);
                    
                    if(filmeAtual == null)
                        return NotFound();

                    if (string.IsNullOrEmpty(filme.Titulo))
                    {
                        ModelState.AddModelError("Titulo", "O campo Título é obrigatório.");
                        return View(filme);
                    }

                    if (string.IsNullOrEmpty(filme.Genero))
                    {
                        ModelState.AddModelError("Genero", "O campo Gênero é obrigatório.");
                        return View(filme);
                    }

                    filmeAtual.Titulo = filme.Titulo;
                    filmeAtual.DataLancamento = DateTime.Parse(filme.DataLancamento);
                    filmeAtual.Genero = filme.Genero;

                    filmeAtual.Streamings.Clear();

                    foreach(var streamingView in filme.Streamings)
                    {
                        if (streamingView.Selecionado)
                        {
                            var streaming = await _streamingService.GetByIdAsync(streamingView.Id ?? Guid.Empty);
                            if(streaming != null)
                                filmeAtual.Streamings.Add(streaming);
                        }
                    }

                    await _service.UpdateAsync(filmeAtual);    
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Titulo", "O nome do filme já existe. Por favor, escolha outro nome.");
                        return View(filme);
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(filme);
        }

        // GET: Filmes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filme = await _service.GetByIdAsync(id.Value);
            if (filme == null)
            {
                return NotFound();
            }

            return View(filme);
        }

        // POST: Filmes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var filme = await _service.GetByIdAsync(id);
            if (filme != null)
            {
                await _service.DeleteAsync(filme);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
