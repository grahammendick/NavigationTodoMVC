using Navigation;
using System;
using System.Collections.Generic;
using System.Web;

namespace NavigationTodoMVC.Models
{
	/// <summary>
	/// Todo ViewModel.
	/// </summary>
	public class TodoModel
	{
		/// <summary>
		/// Gets or sets the new Title.
		/// </summary>
		public string NewTitle { get; set; }

		/// <summary>
		/// Gets or sets the todo list.
		/// </summary>
		public IEnumerable<Todo> Todos { get; set; }

		/// <summary>
		/// Gets or sets the number of Uncompleted items.
		/// </summary>
		public int ItemsLeft { get; set; }

		/// <summary>
		/// Gets or sets the number of Completed items.
		/// </summary>
		public int CompletedCount { get; set; }

		/// <summary>
		/// Gets an empty list indicator.
		/// </summary>
		public bool Empty
		{
			get
			{
				return ItemsLeft == 0 && CompletedCount == 0;
			}
		}

		/// <summary>
		/// Gets the Toggle all status.
		/// </summary>
		public string ToggleAll
		{
			get
			{
				return ItemsLeft == 0 ? "activateAll" : "completeAll";
			}
		}

		/// <summary>
		/// Gets the Toggle all text.
		/// </summary>
		public string ToggleAllText
		{
			get
			{
				return ItemsLeft == 0 ? "Mark all as active" : "Mark all as complete";
			}
		}

		/// <summary>
		/// Gets the Toggle all Complete indicator.
		/// </summary>
		public string ToggleAllComplete
		{
			get
			{
				return ItemsLeft == 0 ? "false" : "true";
			}
		}

		/// <summary>
		/// Gets text indicating whether the specified mode is selected.
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <returns>Selected or null.</returns>
		public string GetSelected(string mode)
		{
			return StateContext.Bag.mode == mode ? "selected" : null;
		}

		/// <summary>
		/// Determines when the todo list RefreshPanel should update. That's when the
		/// toggle all or clear completed has been pressed or the mode has changed.
		/// </summary>
		public Func<HttpContextBase, NavigationData, NavigationData, bool> Changed
		{
			get
			{
				return (context, fromData, toData) => 
					context.Items["refresh"] != null || fromData.Bag.mode != toData.Bag.mode;
			}
		}

		/// <summary>
		/// Determines when the surrounding RefreshPanels should update. That's always
		/// except when a todo title's been edited. This allows a surrounding item click
		/// to be processed when saving a todo title.
		/// </summary>
		public Func<HttpContextBase, NavigationData, NavigationData, bool> Refresh
		{
			get
			{
				return (context, fromData, toData) => context.Items["edit"] == null;
			}
		}
	}
}