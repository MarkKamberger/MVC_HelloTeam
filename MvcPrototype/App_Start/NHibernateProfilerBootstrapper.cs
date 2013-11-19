using HibernatingRhinos.Profiler.Appender.NHibernate;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcPrototype.App_Start.NHibernateProfilerBootstrapper), "PreStart")]
namespace MvcPrototype.App_Start
{
	public static class NHibernateProfilerBootstrapper
	{
        /// <summary>
        /// Pres the start.
        /// </summary>
		public static void PreStart()
		{
			NHibernateProfiler.Initialize();
		}
	}
}

