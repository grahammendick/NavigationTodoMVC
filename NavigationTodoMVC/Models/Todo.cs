using Navigation;
using System;
using System.Web;

namespace NavigationTodoMVC.Models
{
	/// <summary>
	/// Todo Item ViewModel.
	/// </summary>
	public class Todo
	{
		/// <summary>
		/// Gets of sets the Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets of sets the Title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the Completed status.
		/// </summary>
		public bool Completed { get; set; }

		/// <summary>
		/// Gets the Toggle status.
		/// </summary>
		public string Toggle
		{
			get
			{
				return Completed ? "activate" : "complete";
			}
		}

		/// <summary>
		/// Gets the Toggle text.
		/// </summary>
		public string ToggleText
		{
			get
			{
				return Completed ? "Mark as active" : "Mark as complete";
			}
		}

		/// <summary>
		/// Gets the Toggle Complete indicator.
		/// </summary>
		public string ToggleComplete
		{
			get
			{
				return Completed ? "false" : "true";
			}
		}

		/// <summary>
		/// Determines when a todo's RefreshPanel should update. That's when the
		/// todo has been added, deleted or the todo's editing status has changed.
		/// </summary>
		public Func<HttpContextBase, NavigationData, NavigationData, bool> Changed
		{
			get
			{
				return (context, fromData, toData) => (int?)context.Items["todoId"] == Id ||
					(fromData.Bag.id != toData.Bag.id && (fromData.Bag.id == Id || toData.Bag.id == Id));
			}
		}
	}
}