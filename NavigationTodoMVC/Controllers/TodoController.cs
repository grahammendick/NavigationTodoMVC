using Navigation;
using NavigationTodoMVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NavigationTodoMVC.Controllers
{
	/// <summary>
	/// Todo maintenance Controller.
	/// </summary>
    public class TodoController : Controller
    {
		private static int Id = 1;

		/// <summary>
		/// Store the Todos in Session.
		/// </summary>
		private List<Todo> Todos
		{
			get
			{
				if (Session["todos"] == null)
					Session["todos"] = new List<Todo>();
				return (List<Todo>) Session["todos"];
			}
		}

		/// <summary>
		/// To keep finely-grained Actions in a server-rendered progressively enhanced
		/// SPA the work of building the ViewModel must be delegated to a Child Action.
		/// </summary>
		/// <returns>Index View that executes the _Content Child Action.</returns>
		[ActionSelector]
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// The Child Action that builds the ViewModel for rendering the Todo page.
		/// </summary>
		/// <param name="mode">Todo filter. Can be all, active or completed.</param>
		/// <returns>Todo ViewModel.</returns>
		[ChildActionOnly]
		public ActionResult _Content(string mode)
		{
			var model = new TodoModel { Todos = Todos };
			model.ItemsLeft = model.Todos.Count(t => !t.Completed);
			model.CompletedCount = model.Todos.Count(t => t.Completed);
			if (mode != "all")
				model.Todos = model.Todos.Where(t => t.Completed == (mode == "completed"));
			return View(model);
		}

		/// <summary>
		/// Adds a new todo. Sets the id of this new todo in Context to indicate this
		/// todo's RefreshPanel should be updated.
		/// </summary>
		/// <param name="todoModel">Todo.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Add(TodoModel todoModel)
		{
			if (!string.IsNullOrWhiteSpace(todoModel.NewTitle))
			{
				StateContext.Bag.id = null;
				var todo = new Todo {
					Id = Id++,
					Title = todoModel.NewTitle.Trim()
				};
				Todos.Add(todo);
				HttpContext.Items["todoId"] = todo.Id;
			}
			return View();
		}

		/// <summary>
		/// Edits a todo's title. Sets edit to true in Context to indicate the
		/// surrounding RefreshPanels shouldn't be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <param name="cancel">Cancel indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Edit(Todo todo, bool cancel = false)
		{
			HttpContext.Items["edit"] = true;
			StateContext.Bag.id = null;
			var title = todo.Title;
			todo = Todos.FirstOrDefault(t => t.Id == todo.Id);
			if (todo != null && !cancel && !string.IsNullOrWhiteSpace(title))
				todo.Title = title.Trim();
			return View();
		}

		/// <summary>
		/// Toggles the todo's Completed status. Sets the todo's id in Context to
		/// indicate this todo's RefreshPanel should be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <param name="complete">Status indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Toggle(Todo todo, bool complete)
		{
			HttpContext.Items["todoId"] = StateContext.Bag.id;
			StateContext.Bag.id = null;
			todo = Todos.FirstOrDefault(t => t.Id == todo.Id);
			if (todo != null)
				todo.Completed = complete;
			return View();
		}

		/// <summary>
		/// Deletes a todo. Sets the todo's id in Context to indicate this todo's
		/// RefreshPanel should be updated.
		/// </summary>
		/// <param name="todo">Todo.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult Delete(Todo todo)
		{
			HttpContext.Items["todoId"] = StateContext.Bag.id;
			todo = Todos.FirstOrDefault(t => t.Id == todo.Id);
			if (todo != null)
				Todos.Remove(todo);
			return View();
		}

		/// <summary>
		/// Toggles all todo's Completed status. Sets refresh to true in Context to
		/// indicate the todo list should be updated.
		/// </summary>
		/// <param name="complete">Complete indicator.</param>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult ToggleAll(bool complete)
		{
			HttpContext.Items["refresh"] = true;
			StateContext.Bag.id = null;
			Todos.ForEach(t => t.Completed = complete);
			return View();
		}

		/// <summary>
		/// Deletes all Completed todos. Sets refresh to true in Context to
		/// indicate the todo list should be updated.
		/// </summary>
		/// <returns>View.</returns>
		[ActionSelector]
		public ActionResult ClearCompleted()
		{
			HttpContext.Items["refresh"] = true;
			StateContext.Bag.id = null;
			var completed = Todos.Where(t => t.Completed).ToList();
			completed.ForEach(t => Todos.Remove(t));
			return View();
		}
	}
}