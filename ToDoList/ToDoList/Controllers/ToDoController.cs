using Microsoft.AspNetCore.Mvc;
using ToDoList.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;

        public ToDoController(ToDoContext context)
        {
            this.context = context;
        }

        // Get /
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.ToDoList orderby i.Id select i;

            List<TodoList> toDoList = await items.ToListAsync();

            return View(toDoList);
        }

        // GET /todo/create
        public IActionResult Create() => View();

        // Post /todo/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been added!";

                return RedirectToAction("index");
            }

            return View(item);

        }

        // Get /todo/edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // Post /todo/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been updated!";

                return RedirectToAction("index");
            }

            return View(item);
        }

        // Get /todo/delete/5
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else
            {
                context.ToDoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "The item has been deleted";
            }

            return RedirectToAction("index");
        }
    }
}
