using System;
using System.Reflection;
using System.Web.Mvc;

namespace NavigationTodoMVC
{
	/// <summary>
	/// Allows the same route to be served by different Actions.
	/// </summary>
	public class ActionSelectorAttribute : ActionNameSelectorAttribute
	{
		/// <summary>
		/// Determines which Action processes the request by matching the value
		/// of an incoming action parameter against the Action name.
		/// </summary>
		/// <param name="controllerContext">Controller context.</param>
		/// <param name="actionName">Action Name.</param>
		/// <param name="methodInfo">Action method information.</param>
		/// <returns>True if the Action matches, false otherwise.</returns>
		public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
		{
			if (!controllerContext.IsChildAction)
				actionName = controllerContext.Controller.ValueProvider.GetValue("action").AttemptedValue;
			return StringComparer.OrdinalIgnoreCase.Compare(actionName, methodInfo.Name) == 0;
		}
	}
}