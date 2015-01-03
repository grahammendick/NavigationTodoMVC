using Navigation;

[assembly: WebActivatorEx.PreApplicationStartMethod(
    typeof(NavigationTodoMVC.FluentStateInfoConfig), "Register")]
namespace NavigationTodoMVC
{
    public class FluentStateInfoConfig
    {
        /// <summary>
        /// This method is where you configure your navigation. You can find out more
        /// about it by heading over to http://navigation.codeplex.com/documentation
        /// To get you started here's an example
        /// </summary>
        public static void Register()
        {
			StateInfoConfig.Fluent
				.Dialog("Todo", new
				{
					List = new MvcState("{mode}/{*id}", "Todo", "Index")
						.Defaults(new { id = typeof(int), mode = "all" })
						.TrackCrumbTrail(false)
				}, d => d.List)
				.Build();
		}
    }
}