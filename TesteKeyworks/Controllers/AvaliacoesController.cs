using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TesteKeyworks.Models;
using TesteKeyworks.Models.Paginacao;
using TesteKeyworks.Models.ViewModels;
using TesteKeyworks.Services.Avaliacoes;
using TesteKeyworks.Services.Filmes;

namespace TesteKeyworks.Controllers
{
    public class AvaliacoesController : Controller
    {
        private readonly IAvaliacaoService _service;
        private readonly IFilmeService _filmeService;
        private readonly int _pageSize = 3;

        public AvaliacoesController(IAvaliacaoService service, IFilmeService filmeService)
        {
            _service = service;
            _filmeService = filmeService;
        }

        // GET: Avaliacoes
        public async Task<IActionResult> Index(Guid? filmeId, string? tituloFilme, int? pageIndex, int? page, bool habilitarBusca = false)
        {
            habilitarBusca = !string.IsNullOrEmpty(tituloFilme) || habilitarBusca;

            var filme = !habilitarBusca
                ? await _filmeService.GetByIdAsync(filmeId!.Value)
                : (await _filmeService.GetByTituloAsync(tituloFilme!))?.FirstOrDefault();

            ViewData["habilitarBusca"] = habilitarBusca;

            if (filme == null) return View(PaginatedList<AvaliacaoView>.Create(new List<AvaliacaoView>(), 1, _pageSize));
            ViewData["page"] = page;
            ViewData["TituloFilme"] = filme.Titulo;
            ViewData["FilmeId"] = filme.Id;

            int pageNumber = pageIndex ?? 1;

            var avaliacoes = filme.Avaliacoes != null ? filme.Avaliacoes?.Select(x => (AvaliacaoView)x) : [];

            return View(PaginatedList<AvaliacaoView>.Create(avaliacoes!.ToList(), pageNumber, _pageSize));
        }

        // GET: Avaliacoes/Details/5
        public async Task<IActionResult> Details(Guid? id, bool habilitarBusca)
        {
            ViewData["habilitarBusca"] = habilitarBusca;
            
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _service.GetByIdAsync(id.Value);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // GET: Avaliacoes/Create
        public async Task<IActionResult> Create(Guid filmeId)
        {
            var filme = await _filmeService.GetByIdAsync(filmeId);

            if (filme == null) return RedirectToAction(nameof(Index), new { habilitarBusca = true }); ;

            AvaliacaoView avaliacao = new();

            avaliacao.FilmeId = filmeId;
            avaliacao.TituloFilme = filme?.Titulo ?? string.Empty;

            return View(avaliacao);
        }

        // POST: Avaliacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nota,Comentario, TituloFilme, FilmeId,Id")] AvaliacaoView view)
        {
            if (ModelState.IsValid)
            {
                Avaliacao avaliacao = view;
                await _service.AddAsync(avaliacao);
                return RedirectToAction(nameof(Index), new { filmeId = view.FilmeId });
            }
            
            return View(view);
        }

        // GET: Avaliacoes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AvaliacaoView avaliacao = await _service.GetByIdAsync(id.Value);
            if (avaliacao == null)
            {
                return NotFound();
            }
            
            return View(avaliacao);
        }

        // POST: Avaliacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Nota,Comentario,TituloFilme, FilmeId,Id")] AvaliacaoView avaliacao)
        {
            if (id != avaliacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(avaliacao);
                
                return RedirectToAction(nameof(Index), new {filmeId = avaliacao.FilmeId});
            }
            
            return View(avaliacao);
        }

        // GET: Avaliacoes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _service.GetByIdAsync(id.Value);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // POST: Avaliacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, bool habilitarBusca)
        {
            ViewData["habilitarBusca"] = habilitarBusca;
            var avaliacao = await _service.GetByIdAsync(id);
            if (avaliacao != null)
            {
                await _service.DeleteAsync(avaliacao);
            }
            return RedirectToAction(nameof(Index), new {filmeId = avaliacao?.FilmeId ?? Guid.Empty});
        }
    }
}
