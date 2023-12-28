using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TesteKeyworks.Models;
using TesteKeyworks.Models.Paginacao;
using TesteKeyworks.Models.ViewModels;
using TesteKeyworks.Services.Streamings;

namespace TesteKeyworks.Controllers
{
    public class StreamingsController : Controller
    {
        private readonly IStreamingService _service;
        private readonly int _pageSize = 3;

        public StreamingsController(IStreamingService service)
        {
            _service = service;
        }

        // GET: Streamings
        public async Task<IActionResult> Index(int? page, string? nome)
        {
            int pageNumber = page ?? 1;

            ViewData["page"] = page;

            ViewData["nome"] = nome;

            var streamings = string.IsNullOrEmpty(nome) ? await _service.GetAllAsync() : await _service.GetByNomeAsync(nome!);

            return View(PaginatedList<Streaming>.Create(streamings.ToList(), pageNumber, _pageSize));
        }

        // GET: Streamings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var streaming = await _service.GetByIdAsync(id.Value);
            if (streaming == null)
            {
                return NotFound();
            }

            ViewData["totalFilmes"] = streaming.Filmes.Count;

            return View(streaming);
        }

        // GET: Streamings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Streamings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Id")] StreamingView view)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(view.Nome))
                {
                    ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
                    return View(view);
                }

                Streaming streaming = view;

                try
                {
                    await _service.AddAsync(streaming);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Nome", "O nome do streaming já existe. Por favor, escolha outro nome.");
                        return View(view);
                    }
                    throw;
                }
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(view);
        }

        // GET: Streamings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StreamingView streaming = await _service.GetByIdAsync(id.Value);
            if (streaming == null)
            {
                return NotFound();
            }
            return View(streaming);
        }

        // POST: Streamings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Nome,Id")] StreamingView streaming)
        {
            if (id != streaming.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(streaming.Nome))
                {
                    ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
                    return View(streaming);
                }
                try
                {
                    await _service.UpdateAsync(streaming);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                    {
                        ModelState.AddModelError("Nome", "O nome do streaming já existe. Por favor, escolha outro nome.");
                        return View(streaming);
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(streaming);
        }

        // GET: Streamings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var streaming = await _service.GetByIdAsync(id.Value);
            if (streaming == null)
            {
                return NotFound();
            }

            return View(streaming);
        }

        // POST: Streamings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var streaming = await _service.GetByIdAsync(id);
            if (streaming != null)
            {
                await _service.DeleteAsync(streaming);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
